
using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;




public class Database : MonoBehaviour
{
    public bool isLoaded = false;
    private static Database instance;
    [field:SerializeField] public List<CardMetaData> allCardMetas { get; private set; } = new List<CardMetaData>();
    [field:SerializeField] public List<CardData> allCardData { get; private set; } = new List<CardData>();
    public static List<CardMetaData> AllCardMetas => instance.allCardMetas;
    public static List<CardData> AllCardData => instance.allCardData;
    
    
    [Header("Data")]
    [SerializeField] private GoogleSheetSO googleSheetSO;
    [Header("Factory")]
    [SerializeField] private MagicCardDataFactorySO magicCardDataFactory;
    [SerializeField] private RuneCardDataFactorySO runeCardDataFactory;
    private void Awake()
    {
        if(isLoaded)
            return;
        isLoaded = true;
        LoadData();
    }

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    [ContextMenu("Load Data")]
    public void LoadData()
    {
        instance = this;
        allCardMetas.Clear();
        allCardData.Clear();
        foreach (var rawCard in googleSheetSO.RawMagicCardList)
        {
            var card = magicCardDataFactory.Create(rawCard);
            allCardMetas.Add(card);
            allCardData.Add(card.cardData);
        }
        foreach (var rawCard in googleSheetSO.RawRuneCardList)
        {
            var card = runeCardDataFactory.Create(rawCard);
            allCardMetas.Add(card);
            allCardData.Add(card.cardData);
        }
    }
}
