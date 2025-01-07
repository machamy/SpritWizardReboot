
using System;
using System.Collections.Generic;
using UnityEngine;



[ExecuteAlways]
public class Database : MonoBehaviour
{
    public bool isLoaded = false;
    [field:SerializeField] public List<CardData> allCards { get; private set; } = new List<CardData>();
    [field:SerializeField] public List<MagicCardData> allMagicCards { get; private set; } = new List<MagicCardData>();
    
    [Header("Data")]
    [SerializeField] private GoogleSheetSO googleSheetSO;
    [Header("Factory")]
    [SerializeField] private MagicCardDataFactorySO magicCardDataFactory;
    private void Awake()
    {
        if(isLoaded)
            return;
        isLoaded = true;
        LoadData();
    }
    

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        foreach (var cardData in googleSheetSO.RawMagicCardList)
        {
            allMagicCards.Add(magicCardDataFactory.CreateMagicCard(cardData));
        }
        
        foreach (var cardData in allMagicCards)
        {
            allCards.Add(cardData);
        }
    }
}
