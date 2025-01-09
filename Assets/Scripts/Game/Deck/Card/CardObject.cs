using System;
using System.Collections;
using DG.Tweening;
using Game;
using UnityEditor;
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
    public event Action<CardObject> OnCardDiscarded; 
    
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
    
    public void Initialize(CardMetaData cardMetaData)
    {
        name = cardMetaData.cardName;
        this.cardMetaData = cardMetaData;
        cardDisplay.Initialize();
    }
    
    public void RaiseCardDrawn(CardMetaData cardMeta)
    {
        OnCardDrawn?.Invoke(cardMeta);
    }
    
    public void Discard()
    {
        Debug.Log($"[CardObject::Discard] : {cardMetaData.cardName}");
        OnCardDiscarded?.Invoke(this);
    }
    
}
