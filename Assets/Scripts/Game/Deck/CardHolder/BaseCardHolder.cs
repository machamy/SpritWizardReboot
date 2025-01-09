using System.Collections.Generic;
using UnityEngine;


public class BaseCardHolder : MonoBehaviour
{
    [SerializeField] private GameObject cardSlotPrefab;
    
    protected List<CardObject> cardObjects;
    
    
    public void Initialize(List<CardData> initialCards)
    {
        cardObjects = new List<CardObject>();
        foreach (var cardData in initialCards)
        {
            var slot = Instantiate(cardSlotPrefab, transform);
            cardObjects.Add(slot.GetComponentInChildren<CardObject>());
        }

        foreach (var cardObject in cardObjects)
        {
            cardObject.CardSelect.OnDragStart += OnCardDraggStart;
            cardObject.CardSelect.OnDragging += OnCardDragging;
            cardObject.CardSelect.OnDragEnd += OnCardDragEnd;
        }
    }

    protected virtual void OnCardDraggStart(CardSelect cardObject)
    {
        
    }
    
    protected virtual void OnCardDragging(CardSelect cardObject)
    {
        
    }
    
    protected virtual void OnCardDragEnd(CardSelect cardObject)
    {
        
    }

    public void AddCard(CardData cardData)
    {
        var slot = Instantiate(cardSlotPrefab, transform);
        cardObjects.Add(slot.GetComponentInChildren<CardObject>());
    }
    
    public void RemoveCardFromHolder(CardObject cardObject)
    {
        cardObjects.Remove(cardObject);
    }
    
    public void DestoryCard(CardObject cardObject)
    {
        cardObjects.Remove(cardObject);
        Destroy(cardObject.gameObject);
    }
    
}
