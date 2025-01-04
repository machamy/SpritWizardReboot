using System;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Card), typeof(CardSelect))]
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
        public float drageDcayHeightStartCoefficient => displaySetting.drageDcayHeightStartCoefficient;
        private float dragDecayHeightMaxCoeefcient => displaySetting.dragDecayHeightMaxCoeefcient;
        private float dragDecayScale => displaySetting.dragDecayScale;
         private float decayDuration => displaySetting.decayDuration;
    [Header("Display Setting")]
    [SerializeField] private CardDisplaySettingSO displaySetting;
    
    [Header("References")]
    [SerializeField] private Card cardData;
    private CardSelect cardSelect => cardData.CardSelect;
    [SerializeField] private RectTransform cardHolder;
    [Header("Rendering")] 
    [SerializeField] private Image image;
    private Material material;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI damageText;

    
    
    private int _DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    
    private void Awake()
    {
        cardData = GetComponent<Card>();
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

    public void DisplayCard(CardSO card)
    {
        cardData.card = card;
        nameText.text = card.cardName;
        descriptionText.text = card.description;
        image.sprite = card.image;
        costText.text = card.cost.ToString();
        damageText.text = card.damage.ToString();
    }

    private void OnEnable()
    {
        cardData.OnCardDrawn += OnCardDrawn;
        cardSelect.OnFocus += OnFocused;
        cardSelect.OnUnfocus += OnUnfocused;
        cardSelect.OnDragStart += OnDragStart;
        cardSelect.OnDragging += OnDragging;
        cardSelect.OnDragEnd += OnDragEnd;
    }
    
    private void OnDisable()
    {
        cardData.OnCardDrawn -= OnCardDrawn;
        cardSelect.OnFocus -= OnFocused;
        cardSelect.OnUnfocus -= OnUnfocused;
        cardSelect.OnDragStart -= OnDragStart;
        cardSelect.OnDragging -= OnDragging;
        cardSelect.OnDragEnd -= OnDragEnd;
    }
    
    private void OnCardDrawn(CardSO cardSO)
    {
        DisplayCard(cardSO);
        ShowDecay(0);
        transform.localScale = unfocusedScale * Vector3.one;
        transform.position = cardHolder.position;
    }

    public void OnFocused()
    {
        if(cardSelect.IsUsed)
            return;
        transform.DOScale(focusedScale, focusDuration);
    }
    
    public void OnUnfocused()
    {
        if(cardSelect.IsUsed)
            return;
        if(!cardSelect.IsDragging)
            transform.DOScale(unfocusedScale, unfocusDuration);
    }

    public void OnDragStart()
    {
        if(cardSelect.IsUsed)
            return;
        transform.DOScale(dragScale, dragScaleDuration); 
    }
    
    public void OnDraggingUpdate(Vector3 mousePos)
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
            float decayStartHeight = cardHolder.position.y + cardHolder.position.y * drageDcayHeightStartCoefficient;
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
    
    public void ShowDecay(float decayScale)
    {
        if(dacayDOTweener is { active: true })
            dacayDOTweener.Kill();
        material.SetFloat(_DissolveAmount, decayScale);
    }
    
    private Tween dacayDOTweener;
    
    [ContextMenu("ShowDecayDelayed")]
    public void ShowDecayDelayed() => ShowDecayDelayed(1f,decayDuration);
    public void ShowDecayDelayed(float decayScale) => ShowDecayDelayed(decayScale, decayDuration);
    public void ShowDecayDelayed(float decayScale, float duration)
    {
        if(dacayDOTweener is { active: true })
            dacayDOTweener.Kill();
        var anim = material.DOFloat(decayScale, _DissolveAmount, duration);
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
        ShowDecay(0f);
    }
}
