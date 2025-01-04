using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardSO card { get; set; }
    
    [SerializeField] private CardDisplay cardDisplay;
    [SerializeField] private CardSelect cardSelect;

    public event Action<CardSO> OnCardDrawn;
    
    public CardDisplay CardDisplay => cardDisplay;
    public CardSelect CardSelect => cardSelect;

    private void Awake()
    {
        cardDisplay = GetComponent<CardDisplay>();
        cardSelect = GetComponent<CardSelect>();
    }
    
    public void RaiseCardDrawn(CardSO cardSO)
    {
        OnCardDrawn?.Invoke(cardSO);
    }
}
