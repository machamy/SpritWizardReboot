using System;
using Game;
using UnityEngine;
using UnityEngine.Serialization;

public class CardObject : MonoBehaviour
{
    private HandDeckManager _handDeckManager;
    
    [Header("Initialization")]
    public bool hasToBeInitializeDisplay = true;
    [SerializeField] private CardDisplay cardDisplayPrefab;
    [Header("References")]
    [SerializeField] private CardMetaData cardMetaData;
    [SerializeField] private CardDisplay cardDisplay;
    [SerializeField] private CardSelect cardSelect;

    public CardMetaData CardMetaData => cardMetaData;
    public event Action<CardMetaData> OnCardDrawn;
    
    public CardDisplay CardDisplay => cardDisplay;
    public CardSelect CardSelect => cardSelect;

    private void Awake()
    {
        
        if(!hasToBeInitializeDisplay) 
            return;
        Transform cardDisplayParent = CardDisplayParent.Instance?.transform;
        if (cardDisplayParent == null)
        {
            cardDisplayParent = FindFirstObjectByType<Canvas>().transform;
        }
        cardDisplay = Instantiate(cardDisplayPrefab,cardDisplayParent);
        cardDisplay.cardObject = this;
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
