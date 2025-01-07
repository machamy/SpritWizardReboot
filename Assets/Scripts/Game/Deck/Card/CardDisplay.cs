using System;
using DG.Tweening;
using Game;
using Game.Entity;
using Game.World;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(CardObject), typeof(CardSelect))]
public class CardDisplay : MonoBehaviour
{
    [Header("Focus Animation")]
    private float focusedScale => displaySetting.focusedScale;
    private float focusDuration => displaySetting.focusDuration;
     private float unfocusedScale => displaySetting.unfocusedScale;
    private float unfocusDuration => displaySetting.unfocusDuration;
    [Header("Drag Animation")]
    private float dragScale => displaySetting.dragScale;
    private float dragScaleDuration => displaySetting.dragScaleDuration;
    private float dragFollowSpeed => displaySetting.dragFollowSpeed;
    private float dragReturnDuration => displaySetting.dragReturnDuration;
    private float dragMaxHeightCoefficient => displaySetting.dragMaxHeightCoefficient;
    [Header("Decay Animation")]
    public float DrageDecayHeightStartCoefficient => displaySetting.drageDecayHeightStartCoefficient;
    private float dragDecayHeightMaxCoeefcient => displaySetting.dragDecayHeightMaxCoeefcient;
    private float dragDecayScale => displaySetting.dragDecayScale;
     private float decayDuration => displaySetting.decayDuration;
    [Header("Display Setting")]
    [SerializeField] private CardDisplaySettingSO displaySetting;
    
    [FormerlySerializedAs("card")]
    [FormerlySerializedAs("cardData")]
    [Header("References")]
    [SerializeField] private CardObject cardObject;
    private CardSelect cardSelect => cardObject.CardSelect;
    [SerializeField] private RectTransform cardHolder;
    [Header("Rendering")] 
    [SerializeField] private Image image;
    private Material material;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI moveText;

    
    
    private int _DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    
    private void Awake()
    {
        cardObject = GetComponent<CardObject>();
        material = new Material(image.material);
        image.material = material;
    }

    private void Update()
    {
        if (cardSelect.IsDragging)
        {
            OnDraggingUpdate(Input.mousePosition);
        }
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
    public void CancelUse()
    {
        
    }
    #region 이벤트 등록

    

    
    private void OnEnable()
    {
        cardObject.OnCardDrawn += OnCardObjectDrawn;
        cardSelect.OnFocus += OnFocused;
        cardSelect.OnUnfocus += OnUnfocused;
        cardSelect.OnDragStart += OnDragStart;
        cardSelect.OnDragging += OnDragging;
        cardSelect.OnDragEnd += OnDragEnd;
        cardSelect.OnPointerTileEnter += OnPointerTileEnter;
        cardSelect.OnPointerTileExit += OnPointerTileExit;
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
        transform.position = cardHolder.position;
    }

    private void OnFocused()
    {
        if(cardSelect.IsUsed)
            return;
        transform.DOScale(focusedScale, focusDuration);
    }
    
    private void OnUnfocused()
    {
        if(cardSelect.IsUsed)
            return;
        if(!cardSelect.IsDragging)
            transform.DOScale(unfocusedScale, unfocusDuration);
    }

    private void OnDragStart()
    {
        if(cardSelect.IsUsed)
            return;
        transform.DOScale(dragScale, dragScaleDuration);
        image.DOFade(0.4f, dragScaleDuration);
    }
    
    private void OnDraggingUpdate(Vector3 mousePos)
    {
        if(cardSelect.IsUsed)
            return;
        var dragMaxHeight = cardHolder.position.y + cardHolder.position.y * dragMaxHeightCoefficient;
        var rawTargetPos = Vector3.Lerp(transform.position, mousePos, dragFollowSpeed * Time.deltaTime);
        float clampedY = Mathf.Clamp(rawTargetPos.y, 0, dragMaxHeight);
        var targetPos = new Vector3(rawTargetPos.x, clampedY, rawTargetPos.z);
        transform.position = targetPos;
        if(rawTargetPos.y > dragMaxHeight)
        {
            float decayStartHeight = cardHolder.position.y + cardHolder.position.y * DrageDecayHeightStartCoefficient;
            float decayMaxHeight = cardHolder.position.y + cardHolder.position.y * dragDecayHeightMaxCoeefcient;
            float range = decayMaxHeight - decayStartHeight;

            var decayScale = Mathf.Lerp(0f, dragDecayScale, (mousePos.y - decayStartHeight) / range);
                ShowDecay(decayScale);
        }
        else
        {
            ShowDecayDelayed(0f, decayDuration);
        }
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
        var anim = image.materialForRendering.DOFloat(decayScale, _DissolveAmount, duration);
        dacayDOTweener = anim;
    }
    
    public void OnDragging()
    {
        
    }
    
    public void OnDragEnd()
    {
        if(cardSelect.IsUsed)
            return;
        transform.DOScale(unfocusedScale, dragScaleDuration);
        transform.DOMove(cardHolder.position, dragReturnDuration);
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
            if(meta.cardData.CanCastTo(slime.GetComponent<Entity>().Coordinate, tile.Coordinates))
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
