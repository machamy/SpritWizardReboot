using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData cardData { get; set; }
    
    [SerializeField] private CardDisplay cardDisplay;
    [SerializeField] private CardSelect cardSelect;

    public event Action<CardData> OnCardDrawn;
    
    public CardDisplay CardDisplay => cardDisplay;
    public CardSelect CardSelect => cardSelect;

    private void Awake()
    {
        cardDisplay = GetComponent<CardDisplay>();
        cardSelect = GetComponent<CardSelect>();
    }
    
    public void RaiseCardDrawn(CardData cardSO)
    {
        OnCardDrawn?.Invoke(cardSO);
    }
}
