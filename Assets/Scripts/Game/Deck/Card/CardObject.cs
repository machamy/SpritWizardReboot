using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

public class CardObject : MonoBehaviour
{
    private HandDeckManager _handDeckManager;
    
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

    private void Start()
    {
        _handDeckManager = BattleManager.Instance.HandDeckManager;
    }

    public void RaiseCardDrawn(CardMetaData cardMeta)
    {
        cardMetaData = cardMeta;
        OnCardDrawn?.Invoke(cardMeta);
    }
    
    public void Discard()
    {
        _handDeckManager.DiscardCard(cardMetaData);
    }
}
