using System;
using System.Collections;
using DG.Tweening;
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
    
    [Header("Focus Animation")]
    public float focusedScale => displaySetting.focusedScale;
    private float focusDuration => displaySetting.focusDuration;
    public bool focusCardVisible => displaySetting.focusCardVisible;
    public float unfocusedScale => displaySetting.unfocusedScale;
    private float unfocusDuration => displaySetting.unfocusDuration;
    [Header("Drag Animation")]
    private float dragScale => displaySetting.dragScale;
    private float dragScaleDuration => displaySetting.dragScaleDuration;
    private float FollowSpeed => displaySetting.followSpeed;
    private float dragReturnDuration => displaySetting.dragReturnDuration;
    private float dragMaxHeightCoefficient => displaySetting.dragMaxHeightCoefficient;
    [Header("Decay Animation")]
    public float DrageDecayHeightStartCoefficient => displaySetting.drageDecayHeightStartCoefficient;
    private float dragDecayHeightMaxCoeefcient => displaySetting.dragDecayHeightMaxCoeefcient;
    private float dragDecayScale => displaySetting.dragDecayScale;
     private float decayDuration => displaySetting.decayDuration;
    [Header("Display Setting (SO)")]
    [SerializeField] private CardDisplaySettingSO displaySetting;

    [Header("Display Setting (Current)")] 
    public bool DoFollowAnimation = true;
    [Tooltip("따라가지 않는다")]public bool DoNotFollow = false;
    [FormerlySerializedAs("card")]
    [FormerlySerializedAs("cardData")]
    [Header("References")]
    [SerializeField] public CardObject cardObject;
    private CardSelect cardSelect => cardObject.CardSelect;
    [Header("Rendering")] 
    [SerializeField] private Image image;
    private Material material;
    [SerializeField] private bool isAnimating = false;
    public bool IsAnimating => isAnimating || DOTween.IsTweening(transform) || DOTween.IsTweening(image);
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI moveText;


    private Vector3 cardObjectPos;
    
    
    private int _DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        material = new Material(image.material);
        image.material = material;
    }

    private void Start()
    {
        cardObjectPos = cardObject.transform.position;
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
    
    private void FollowCard()
    {
        if(cardSelect.IsUsed || DoNotFollow)
            return;
        if (!displaySetting.followAnimation || !DoFollowAnimation)
        {
            transform.position = cardObjectPos;
            print($"{cardObjectPos} {transform.position}");
            return;
        }
        Vector3 targetPos = cardObjectPos;
        if (cardSelect.IsDragging)
        {
            var dragMaxHeight = cardObjectPos.y + cardObjectPos.y * dragMaxHeightCoefficient;
            var rawTargetPos = Vector3.Lerp(transform.position, targetPos, FollowSpeed * Time.deltaTime);
            float clampedY = Mathf.Clamp(rawTargetPos.y, 0, dragMaxHeight);
            targetPos = new Vector3(rawTargetPos.x, clampedY, rawTargetPos.z);
            
            if(cardObjectPos.y > dragMaxHeight)
            {
                float decayStartHeight = cardObjectPos.y + cardObjectPos.y * DrageDecayHeightStartCoefficient;
                float decayMaxHeight = cardObjectPos.y + cardObjectPos.y * dragDecayHeightMaxCoeefcient;
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
            targetPos = Vector3.Lerp(transform.position, targetPos, FollowSpeed * Time.deltaTime);
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

    public float GetMinVisiblePos(float postion){
        return Mathf.Max(postion, rectTransform.rect.height * 0.5f * focusedScale);
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
    }
    #endregion

    #region 이벤트 핸들러
    
    private void OnCardObjectDrawn(CardMetaData cardMetaSo)
    {
        DisplayCard(cardMetaSo);
        ShowDecay(0);
        image.DOFade(1, 0);
        transform.localScale = unfocusedScale * Vector3.one;
    }

    private void OnFocused(CardSelect cardSelect)
    {
        if(cardSelect.IsUsed || cardSelect.IsDragging)
            return;
        // print($"{name} focused");
        transform.DOScale(focusedScale, focusDuration);
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
            transform.DOScale(unfocusedScale, unfocusDuration);
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
        ShowDecay(0f);
    }
    
    private void OnPointerTileEnter(Tile tile)
    {
        if(cardSelect.IsUsed)
            return;
        var meta = cardObject.CardMetaData;
        if(meta.cardType == CardType.Rune)
        {
            tile.Focus(displaySetting.tileFocusOkColor);
        }
        else
        {
            var slime = BattleManager.Instance.GetSlime(meta.cardData.skillCaster);
            if(meta.cardData.CanCastTo(slime.GetComponent<Entity>().Coordinate, tile.Coordinates, CardCastManager.Instance.GetMoveCntRuneEffect(meta.cardData)))
            {
                tile.Focus(displaySetting.tileFocusOkColor);
            }
            else
            {
                tile.Focus(displaySetting.tileFocusNoColor);
            }
        }

        
    }
    
    public void OnPointerTileExit(Tile tile)
    {
        if(cardSelect.IsUsed)
            return;
        tile.Unfocus();   
    }
    
    public void OnPointerTileUpdate(Tile tile)
    {
        
    }
    #endregion

}
