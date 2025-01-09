using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class BaseCardHolder : MonoBehaviour
{
    [SerializeField] private GameObject cardobjectPrefab;
    [SerializeField] private GameObject cardSlotPrefab;
    
    protected List<CardObject> cardObjects;
    public int CardCount => cardObjects.Count;

    private void Awake()
    {
        cardObjects = new List<CardObject>();
    }

    /// <summary>
    /// 해당 카드들로 카드를 초기화함. 필수아님
    /// </summary>
    /// <param name="initialCards"></param>
    public void Initialize(List<CardMetaData> initialCards)
    {
        cardObjects = new List<CardObject>();
        foreach (var cardData in initialCards)
        {
            var slot = Instantiate(cardSlotPrefab, transform);
            AddCardWithSlot(cardData);
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
        cardObject.Initialize(cardData);
        cardObject.CardSelect.OnFocus += OnFocusBase;
        cardObject.CardSelect.OnUnfocus += OnUnfocusBase;
        cardObject.CardSelect.OnDragStart += OnCardDraggStartBase;
        cardObject.CardSelect.OnDragging += OnCardDraggingBase;
        cardObject.CardSelect.OnDragEnd += OnCardDragEndBase;
        cardObject.OnCardDiscarded += OnCardDiscardBase;
        
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
        cardObjects.Remove(cardObject);
        Destroy(cardObject.transform.parent.gameObject);
    }
    
    public CardObject this[int index]
    {
        get => cardObjects[index];
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
    
    private void OnFocusBase (CardSelect cardSelect) => OnFocus(cardSelect);
    private void OnUnfocusBase (CardSelect cardSelect) => OnUnfocus(cardSelect);
    private void OnCardDraggStartBase (CardSelect cardSelect) => OnCardDraggStart(cardSelect);
    private void OnCardDraggingBase (CardSelect cardSelect) => OnCardDragging(cardSelect);
    private void OnCardDragEndBase (CardSelect cardSelect) => OnCardDragEnd(cardSelect);
    private void OnCardDiscardBase(CardObject cardSelect) => OnCardDiscard(cardSelect);
    
    #endregion

    
}
