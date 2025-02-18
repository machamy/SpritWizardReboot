using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventChannel;
using Game;
using Game.Entity;
using Game.World;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private RectTransform rectTransform;

    #region Setting 접근용
    [Header("Focus Animation")]
    public float focusedScale => setting.focusedScale;
    private float focusDuration => setting.focusDuration;
    public bool focusCardVisible => setting.focusCardVisible;
    public float unfocusedScale => setting.unfocusedScale;
    private float unfocusDuration => setting.unfocusDuration;
    [Header("Drag Animation")]
    private float dragScale => setting.dragScale;
    private float dragScaleDuration => setting.dragScaleDuration;
    private float FollowSpeed => setting.followSpeed;
    private float dragReturnDuration => setting.dragReturnDuration;
    private float dragMaxHeightCoefficient => setting.dragMaxHeightCoefficient;
    [Header("Decay Animation")]
    public float DrageDecayHeightStartCoefficient => setting.drageDecayHeightStartCoefficient;
    private float dragDecayHeightMaxCoeefcient => setting.dragDecayHeightMaxCoeefcient;
    private float dragDecayScale => setting.dragDecayScale;
     private float decayDuration => setting.decayDuration;
     #endregion
    
    [Header("Display Setting (SO)")]
    private CardSettingSO setting => cardObject.cardSetting;

    [Header("Display Setting (Current)")] 
    public bool DoFollowAnimation = true;
    public Vector3 RawDefaultSize => Vector3.one * unfocusedScale;
    [Tooltip("따라가지 않는다")]public bool DoNotFollow = false;
    [FormerlySerializedAs("card")]
    [FormerlySerializedAs("cardData")]
    [Header("References")]
    [SerializeField] public CardObject cardObject;
    private CardSelect cardSelect => cardObject.CardSelect;
    [Header("Rendering")] 
    [SerializeField] private Image image;
    private Material material;
    private Outline outline;
    [SerializeField] private bool isAnimating = false;
    public bool IsAnimating => isAnimating || DOTween.IsTweening(transform) || DOTween.IsTweening(image);
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI moveText;
    [Header("Channel")]
    [SerializeField] GUIEventChannelSO guiEventChannel;

    private Vector3 cardObjectPos;
    
    
    private int _DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        outline = GetComponent<Outline>();
        material = new Material(image.material);
        image.material = material;
    }

    private void Start()
    {
        cardObjectPos = cardObject.transform.position;
        InitializeDisplayState();
    }

    private void Update()
    {
        cardObjectPos = cardObject.transform.position;
        FollowCard();
    }

    public void Initialize()
    {
        DisplayCard(cardObject.CardMetaData);
    }

    public void DisplayCard(CardMetaData cardMetaData)
    {
        if(cardMetaData == null)
        {
            Debug.LogError("Card is null");
            return;
        }
        nameText.text = cardMetaData.cardName;
        descriptionText.text = cardMetaData.description;
        image.sprite = cardMetaData.backImage;
        costText.text = cardMetaData.cost.ToString();
        if(cardMetaData.cardType == CardType.Attack)
        {
            moveText.text = cardMetaData.cardData.move.ToString();
        }
        else
        {
            moveText.text = String.Empty;
        }
    }
    
    [ContextMenu("InitializeDisplayState")]
    public void InitializeDisplayState()
    {
        ShowDecay(0);
        image.DOFade(1, 0);
        transform.localScale = unfocusedScale * Vector3.one;
    }
    
    private void FollowCard()
    {
        if(cardSelect.IsUsed || DoNotFollow)
            return;
        if (!setting.followAnimation || !DoFollowAnimation)
        {
            transform.position = cardObjectPos;
            print($"{cardObjectPos} {transform.position}");
            return;
        }

        Vector3 targetPos;
        float GetHeightByCoefficient(float coefficient) => cardObjectPos.y + cardObjectPos.y * coefficient;
        if (cardSelect.IsDragging)
        {
            targetPos = Input.mousePosition;
            var dragMaxHeight = GetHeightByCoefficient(dragMaxHeightCoefficient);
            var rawTargetPos = Vector3.Lerp(transform.position, targetPos, FollowSpeed * Time.deltaTime);
            // print($"{dragMaxHeight} {rawTargetPos.y}");
            float clampedY = Mathf.Clamp(rawTargetPos.y, 0, dragMaxHeight);
            targetPos = new Vector3(rawTargetPos.x, clampedY, rawTargetPos.z);
            
            if(cardObjectPos.y > dragMaxHeight)
            {
                float decayStartHeight = GetHeightByCoefficient(DrageDecayHeightStartCoefficient);
                float decayMaxHeight = GetHeightByCoefficient(dragDecayHeightMaxCoeefcient);
                float range = decayMaxHeight - decayStartHeight;

                var decayScale = Mathf.Lerp(0f, dragDecayScale, (targetPos.y - decayStartHeight) / range);
                ShowDecay(decayScale);
            }
            else
            {
                ShowDecay(0);
            }
        }
        else
        {
            targetPos = Vector3.Lerp(transform.position, cardObjectPos, FollowSpeed * Time.deltaTime);
            ShowDecay(0);
        }
        transform.position = targetPos;
    }

    
    // private Vector3 smoothedRotation = Vector3.zero;
    // /// <summary>
    // /// 이동 거리에 따라 카드의 기울기를 조절한다.
    // /// </summary>
    // private void FollowTilt()
    // {
    //     Vector3 delta = transform.position - previousPosition;
    //     Vector3 rotation = delta * displaySetting.followTiltAmount;
    //     smoothedRotation = Vector3.Lerp(smoothedRotation, rotation, Time.deltaTime * displaySetting.followTiltSpeed);
    //     transform.eulerAngles = smoothedRotation;
    // }
    
    public void SetTransformIndex(int index)
    {
        rectTransform.SetSiblingIndex(index);
    }
    
    public void UpdateTransformIndex()
    {
        rectTransform.SetSiblingIndex(cardObject.transform.parent.GetSiblingIndex());
    }
    
    public void CancelUse()
    {
        
    }

    public float GetClampedVisiblePos(float postion){
        return Mathf.Clamp(postion
            , rectTransform.rect.height * 0.5f * focusedScale
            , Screen.height - rectTransform.rect.height * 0.5f * focusedScale);
    }

    #region 이벤트 등록

    

    
    private void OnEnable()
    {
        
        StartCoroutine(DelayedOnEnable());
        IEnumerator DelayedOnEnable()
        {
            yield return new WaitForEndOfFrame();
            
            cardObject.OnCardDrawn += OnCardObjectDrawn;
            cardSelect.OnFocus += OnFocused;
            cardSelect.OnUnfocus += OnUnfocused;
            cardSelect.OnDragStart += OnDragStart;
            cardSelect.OnDragging += OnDragging;
            cardSelect.OnDragEnd += OnDragEnd;
            cardSelect.OnPointerTileEnter += OnPointerTileEnter;
            cardSelect.OnPointerTileExit += OnPointerTileExit;
            cardSelect.OnPointerDown += OnPointerDown;
            cardSelect.OnPointerUp += OnPointerUp;
            
        }
    }
    
    private void OnDisable()
    {
        cardObject.OnCardDrawn -= OnCardObjectDrawn;
        cardSelect.OnFocus -= OnFocused;
        cardSelect.OnUnfocus -= OnUnfocused;
        cardSelect.OnDragStart -= OnDragStart;
        cardSelect.OnDragging -= OnDragging;
        cardSelect.OnDragEnd -= OnDragEnd;
        cardSelect.OnPointerTileEnter -= OnPointerTileEnter;
        cardSelect.OnPointerTileExit -= OnPointerTileExit;
        cardSelect.OnPointerDown -= OnPointerDown;
        cardSelect.OnPointerUp -= OnPointerUp;
        UnfocusTargetTiles();
    }
    #endregion

    #region 이벤트 핸들러
    
    private void OnCardObjectDrawn(CardMetaData cardMetaSo)
    {
        DisplayCard(cardMetaSo);
        InitializeDisplayState();
    }

    private void OnFocused(CardSelect cardSelect)
    {
        if(cardSelect.IsUsed || cardSelect.IsDragging)
            return;
        // print($"{name} focused");
        transform.DOScale(focusedScale, focusDuration);
        
        OnHoverd();
        // if(focusCardVisible)
        // {
        //     var targetY = GetMinVisiblePos(cardObjectPos.y); 
        //     transform.DOMoveY(targetY, .15f);
        // }
    }
    
    private void OnUnfocused(CardSelect cardSelect)
    {
        if(cardSelect.IsUsed)
            return;
        if(!cardSelect.IsDragging)
        {
            transform.DOKill();
            transform.DOScale(unfocusedScale, unfocusDuration);
            OnUnhoverd();
        }
    }

    private void OnHoverd()
    {
        // TODO : 룬카드/마법카드 호버링 처리
        switch (cardObject.CardMetaData.cardType)
        {
            case CardType.Attack:
                break;
            case CardType.Rune:
                RuneEffectHolder runeEffectHolder = CardCastManager.Instance.RuneEffectHolder;
                runeEffectHolder.StackTempRuneEffectRange(cardObject.CardMetaData.cardData.runeEffectAmounts);
                break;
        }
    }
    
    private void OnUnhoverd()
    {
        switch (cardObject.CardMetaData.cardType)
        {
            case CardType.Attack:
                break;
            case CardType.Rune:
                RuneEffectHolder runeEffectHolder = CardCastManager.Instance.RuneEffectHolder;
                runeEffectHolder.ResetAllTempRuneEffect();
                break;
        }
    }

    private void OnDragStart(CardSelect cardSelect)
    {
        if(cardSelect.IsUsed)
            return;
        transform.DOScale(dragScale, dragScaleDuration);
        image.DOFade(0.4f, dragScaleDuration);
    }
    
    private void ShowDecay(float decayScale)
    {
        if(dacayDOTweener is { active: true })
            dacayDOTweener.Kill();
        image.materialForRendering.SetFloat(_DissolveAmount, decayScale);
    }
    
    private Tween dacayDOTweener;
    
    [ContextMenu("ShowDecayDelayed")]
    public void ShowDecayDelayed() => ShowDecayDelayed(1f,decayDuration);
    public void ShowDecayDelayed(float decayScale) => ShowDecayDelayed(decayScale, decayDuration);
    public void ShowDecayDelayed(float decayScale, float duration)
    {
        if(dacayDOTweener is { active: true })
            dacayDOTweener.Kill();
        isAnimating = true;
        var anim = image.materialForRendering
            .DOFloat(decayScale, _DissolveAmount, duration)
            .OnComplete(() => isAnimating = false);
        dacayDOTweener = anim;
    }
    
    public void OnDragging(CardSelect cardSelect)
    {
        
    }
    
    public void OnDragEnd(CardSelect cardSelect)
    {
        if(cardSelect.IsUsed)
            return;
        transform.DOScale(unfocusedScale, dragScaleDuration);
        // transform.DOMove(cardHolder.position, dragReturnDuration);
        image.DOFade(1f, dragScaleDuration);
        OnHoverd();
        ShowDecay(0f);
    }
    
    private List<Tile> targetTiles = new List<Tile>();
    private void OnPointerTileEnter(Tile tile)
    {
        if(cardSelect.IsUsed)
            return;
        var meta = cardObject.CardMetaData;
        if(meta.cardType == CardType.Rune)
        {
            // tile.Focus(setting.tileFocusOkColor, Tile.FocusState.MoveOk);
        }
        else
        {
            var slime = BattleManager.Instance.GetSlime(meta.cardData.skillCaster);
            var runeEffectHolder = CardCastManager.Instance.RuneEffectHolder;
            if(meta.cardData.CanCastTo(slime.GetComponent<Entity>().Coordinate, tile.Coordinates, runeEffectHolder))
            {
                targetTiles = meta.cardData.GetTargetTiles(tile.Coordinates, runeEffectHolder);
                foreach (var t in targetTiles)
                {
                    t.Focus(setting.tileFocusTargetColor, Tile.FocusState.Target);
                }
                tile.Focus(setting.tileFocusOkColor, Tile.FocusState.MoveOk);
            }
            else
            {
                tile.Focus(setting.tileFocusNoColor, Tile.FocusState.Error);
            }
        }
    }
    
    public void OnPointerTileExit(Tile tile)
    {
        if(cardSelect.IsUsed)
            return;
        tile.Unfocus();
        UnfocusTargetTiles();
    }
    
    private void UnfocusTargetTiles()
    {
        foreach (var t in targetTiles)
        {
            t.Unfocus();
        }
    }
    
    
    public void OnPointerDown(CardSelect cardSelect)
    {
        
    }
    public void OnPointerUp(CardSelect cardSelect, bool isClick)
    {
        if(!outline || !setting.sellectOutline)
            return;
        
        if (isClick && cardSelect.IsSelected)
        {
            
            outline.enabled = true;
            outline.effectColor = setting.selectOutlineColor;
            outline.DOScale(setting.selectOutlineWidth * Vector3.one, setting.selectAnimationDuration);
        }
        else
        {
            outline.DOScale(Vector2.zero, setting.selectAnimationDuration)
                .OnComplete(() => outline.enabled = false);
        }
    }
    #endregion

}
