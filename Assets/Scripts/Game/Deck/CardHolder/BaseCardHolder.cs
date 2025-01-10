using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


public class BaseCardHolder : MonoBehaviour
{
    /// <summary>
    /// 비활성화 될시 비활성화 될 오브젝트
    /// null 이면 자기 자신이 비활성화 됨
    /// 현재 구현은 BaseCardSellectableHolder에서만 사용함
    /// </summary>
    [SerializeField,Tooltip("비활성화 될시 비활성화 될 오브젝트\n null 이면 자기 자신이 비활성화 됨\n현재 구현은 BaseCardSellectableHolder에서만 사용함")] protected GameObject parent;
    [SerializeField] private CardSettingSO cardSetting;
    [SerializeField] private GameObject cardobjectPrefab;
    [SerializeField] private GameObject cardSlotPrefab;
    
    protected List<CardObject> cardObjects = new List<CardObject>();
    public int CardCount => cardObjects.Count;
    

    /// <summary>
    /// 해당 카드들로 카드를 초기화함. 필수아님
    /// </summary>
    /// <param name="initialCards"></param>
    public virtual void Initialize(List<CardMetaData> initialCards)
    {
        foreach (var co in cardObjects)
        {
            Destroy(co.CardDisplay.gameObject);
            if(co.transform.parent.CompareTag("Slot"))
                Destroy(co.transform.parent.gameObject);
            else
                Destroy(co.gameObject);
        }
        cardObjects = new List<CardObject>();
        foreach (var cardData in initialCards)
        {
            AddCardWithSlot(cardData);
        }
    }
    
    public virtual void Enable()
    {
        if(parent != null)
            parent.SetActive(true);
        else
            gameObject.SetActive(true);

        foreach (var co in cardObjects)
        {
            co.CardDisplay.gameObject.SetActive(true);
        }
    }
    
    public virtual void Disable()
    {
        if(parent != null)
            parent.SetActive(false);
        else
            gameObject.SetActive(false);
        foreach (var co in cardObjects)
        {
            co.CardDisplay.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 슬롯과 함께 카드를 추가함.
    /// </summary>
    /// <param name="cardData">생성된 카드 오브젝트</param>
    /// <returns></returns>
    public CardObject AddCardWithSlot(CardMetaData cardData)
    {
        var slot = Instantiate(cardSlotPrefab, transform);
        var cardObject = slot.GetComponentInChildren<CardObject>();
        cardObjects.Add(cardObject);
        cardObject.Initialize(cardData,cardSetting);
        cardObject.CardSelect.OnFocus += OnFocusBase;
        cardObject.CardSelect.OnUnfocus += OnUnfocusBase;
        cardObject.CardSelect.OnDragStart += OnCardDraggStartBase;
        cardObject.CardSelect.OnDragging += OnCardDraggingBase;
        cardObject.CardSelect.OnDragEnd += OnCardDragEndBase;
        cardObject.OnCardDiscarded += OnCardDiscardBase;
        cardObject.CardSelect.OnPointerDown += OnCardPointerDownBase;
        cardObject.CardSelect.OnPointerUp += OnCardPointerUpBase;
        
        return cardObject;
    }
    
    
    /// <summary>
    /// 카드를 슬롯과 홀더에서 빼내고, 슬롯을 삭제한다.
    /// </summary>
    /// <param name="cardObject"></param>
    public void RemoveCardFromHolder(CardObject cardObject)
    {
        if (!cardObjects.Contains(cardObject))
        {
            Debug.LogWarning("해당 카드는 존재하지 않습니다.");
            return;
        }
        GameObject slot = cardObject.transform.parent.gameObject;
        cardObjects.Remove(cardObject);
        cardObject.CardSelect.OnFocus -= OnFocusBase;
        cardObject.CardSelect.OnUnfocus -= OnUnfocusBase;
        cardObject.CardSelect.OnDragStart -= OnCardDraggStartBase;
        cardObject.CardSelect.OnDragging -= OnCardDraggingBase;
        cardObject.CardSelect.OnDragEnd -= OnCardDragEndBase;
        cardObject.OnCardDiscarded -= OnCardDiscardBase;
        cardObject.CardSelect.OnPointerDown -= OnCardPointerDownBase;
        cardObject.CardSelect.OnPointerUp -= OnCardPointerUpBase;
        
        cardObject.transform.SetParent(transform.parent);
        Destroy(slot);
    }

    /// <summary>
    /// 카드와 슬롯을 함께 삭제한다.
    /// </summary>
    /// <param name="cardObject"></param>
    public void DestoryCard(CardObject cardObject)
    {
        // int index = cardObjects.IndexOf(cardObject);
        var parent = cardObject.transform.parent;
        Destroy(cardObject.CardDisplay.gameObject);
        if(parent.CompareTag("Slot"))
            Destroy(parent.gameObject);
        else
            Destroy(cardObject.gameObject);
        
    }
    
    public CardObject this[int index]
    {
        get => cardObjects[index];
    }
    
    /// <summary>
    /// CardSlot 순서에 따라 Display 순서를 배치
    /// </summary>
    public void UpdateAllCardIndex()
    {
        foreach (var cardObject in cardObjects)
        {
            var cardDisplay = cardObject.CardDisplay;
            cardDisplay.UpdateTransformIndex();
        }
    }
    #region Event Handlers
    

    
    private CardSelect _focusedCardSelect;
    protected virtual void OnFocus(CardSelect cardSelect)
    {
        _focusedCardSelect = cardSelect;
    }
    
    protected virtual void OnUnfocus(CardSelect cardSelect)
    {
        _focusedCardSelect = null;
        
    }

    protected virtual void OnCardDraggStart(CardSelect cardSelect)
    {
        
    }
    
    protected virtual void OnCardDragging(CardSelect cardSelect)
    {
        
    }
    
    protected virtual void OnCardDragEnd(CardSelect cardSelect)
    {
        
    }
    
    protected virtual void OnCardDiscard(CardObject cardObject)
    {
        RemoveCardFromHolder(cardObject);
        StartCoroutine(CheckForDiscardCard(cardObject));
        IEnumerator CheckForDiscardCard(CardObject cardObject)
        {
            CardDisplay cardDisplay = cardObject.CardDisplay;
            yield return new WaitWhile(() => DOTween.IsTweening(cardObject.transform) || cardDisplay.IsAnimating);
            Destroy(cardDisplay.gameObject);
            Destroy(cardObject.gameObject);
        }
    }
    
    protected virtual void OnCardPointerDown(CardSelect cardSelect)
    {
        
    }
    
    protected virtual void OnCardPointerUp(CardSelect cardSelect,bool isClick)
    {
        
    }
    
    private void OnFocusBase (CardSelect cardSelect) => OnFocus(cardSelect);
    private void OnUnfocusBase (CardSelect cardSelect) => OnUnfocus(cardSelect);
    private void OnCardDraggStartBase (CardSelect cardSelect) => OnCardDraggStart(cardSelect);
    private void OnCardDraggingBase (CardSelect cardSelect) => OnCardDragging(cardSelect);
    private void OnCardDragEndBase (CardSelect cardSelect) => OnCardDragEnd(cardSelect);
    private void OnCardDiscardBase(CardObject cardSelect) => OnCardDiscard(cardSelect);
    
    private void OnCardPointerDownBase(CardSelect cardSelect) => OnCardPointerDown(cardSelect);
    
    private void OnCardPointerUpBase(CardSelect cardSelect,bool isClick) => OnCardPointerUp(cardSelect,isClick);
    
    #endregion

    
}
