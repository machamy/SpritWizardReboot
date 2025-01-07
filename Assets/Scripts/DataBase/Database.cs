
using System;
using System.Collections.Generic;
using DataBase.DataClasses;
using UnityEngine;


public class Database : MonoBehaviour
{
    public bool isLoaded = false;
    public static List<CardData> allCards { get; private set; } = new List<CardData>();
    public static List<MagicCardData> allMagicCards { get; private set; } = new List<MagicCardData>();
    
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
    
    private void LoadData()
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
