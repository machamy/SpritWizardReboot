using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    [FormerlySerializedAs("cardData")] [SerializeField] private CardMetaData cardMetaData;
    [SerializeField] private CardDisplay cardDisplay;
    [SerializeField] private CardSelect cardSelect;

    public CardMetaData CardMetaData => cardMetaData;
    public event Action<CardMetaData> OnCardDrawn;
    
    public CardDisplay CardDisplay => cardDisplay;
    public CardSelect CardSelect => cardSelect;

    private void Awake()
    {
        cardDisplay = GetComponent<CardDisplay>();
        cardSelect = GetComponent<CardSelect>();
    }
    
    public void RaiseCardDrawn(CardMetaData cardMeta)
    {
        cardMetaData = cardMeta;
        OnCardDrawn?.Invoke(cardMeta);
    }
}
