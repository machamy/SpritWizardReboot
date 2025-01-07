using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardData cardData;
    [SerializeField] private CardDisplay cardDisplay;
    [SerializeField] private CardSelect cardSelect;

    public CardData CardData => cardData;
    public event Action<CardData> OnCardDrawn;
    
    public CardDisplay CardDisplay => cardDisplay;
    public CardSelect CardSelect => cardSelect;

    private void Awake()
    {
        cardDisplay = GetComponent<CardDisplay>();
        cardSelect = GetComponent<CardSelect>();
    }
    
    public void RaiseCardDrawn(CardData card)
    {
        cardData = card;
        OnCardDrawn?.Invoke(card);
    }
}
