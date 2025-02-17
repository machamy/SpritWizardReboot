
using System;
using System.Collections.Generic;
using DataStructure;
using Game;
using NUnit.Framework;
using UnityEngine;




public class Database : Singleton<Database>
{
    public bool isLoaded = false;
    [field:SerializeField] public List<CardMetaData> allCardMetas { get; private set; } = new List<CardMetaData>();
    [field:SerializeField] public List<CardData> allCardData { get; private set; } = new List<CardData>();
    [field: SerializeField] public List<CardMetaData> allSmithedCardMetas { get; private set; } = new List<CardMetaData>();
    [field: SerializeField] public List<CardData> allSmithedCardData { get; private set; } = new List<CardData>();
    [field: SerializeField] public SerializableDict<SeedType, RewardChanceData> allRewardChance { get; private set; } = new SerializableDict<SeedType, RewardChanceData>();
    [field: SerializeField] public SerializableDict<SeedType, RewardAmountData> allRewardAmount { get; private set; } = new SerializableDict<SeedType, RewardAmountData>();
    [field: SerializeField] public SerializableDict<SeedType, AddCardWeightData> allAddCardWeight { get; private set; } = new SerializableDict<SeedType, AddCardWeightData>();
    [field:SerializeField] public List<StorePriceData> allCardPrice { get; private set; } = new List<StorePriceData>();
    [field:SerializeField] public List<StorePriceData> allEditCardPrice { get; private set; } = new List<StorePriceData>();
    [field:SerializeField] public List<EnemyData> allEnemyData { get; private set; } = new List<EnemyData>();
    [field:SerializeField] public List<EnemyPatternData> allEnemyPatternData { get; private set; } = new List<EnemyPatternData>();
    [field: SerializeField] public List<EnemyActionData> allEnemyActionData { get; private set; } = new List<EnemyActionData>();
    public static List<CardMetaData> AllCardMetas => Instance.allCardMetas;
    public static List<CardData> AllCardData => Instance.allCardData;
    public static List<CardMetaData> AllSmithedCardMetas => Instance.allSmithedCardMetas;
    public static List<CardData> AllSmithedCardData => Instance.allSmithedCardData;
    public static Dictionary<SeedType, RewardChanceData> AllRewardChance => Instance.allRewardChance;
    public static Dictionary<SeedType, RewardAmountData> AllRewardAmount => Instance.allRewardAmount;
    public static Dictionary<SeedType, AddCardWeightData> AllAddCardWeight => Instance.allAddCardWeight;
    public static List<StorePriceData> AllCardPrice => Instance.allCardPrice;
    public static List<StorePriceData> AllEditCardPrice => Instance.allEditCardPrice;
    public static List<EnemyData> AllEnemyData => Instance.allEnemyData;
    public static List<EnemyPatternData> AllEnemyPatternData => Instance.allEnemyPatternData;
    public static List<EnemyActionData> AllEnemyActionData => Instance.allEnemyActionData;


    [Header("Data")]
    [SerializeField] private GoogleSheetSO googleSheetSO;
    [Header("Factory")]
    [SerializeField] private MagicCardDataFactorySO magicCardDataFactory;
    [SerializeField] private RuneCardDataFactorySO runeCardDataFactory;
    [SerializeField] private SmithedMagicCardDataFactorySO smithedMagicCardDataFactory;
    [SerializeField] private SmithedRuneCardDataFactory smithedRuneCardDataFactory;
    [SerializeField] private RewardChanceDataFactorySO rewardChanceDataFactory;
    [SerializeField] private RewardAmountDataFactorySO rewardAmountDataFactory;
    [SerializeField] private AddCardWeightDataFactorySO addCardWeightDataFactory;
    [SerializeField] private StorePriceDataFactorySO storePriceDataFactory;
    [SerializeField] private EnemyDataFactorySO enemyDataFactory;
    [SerializeField] private EnemyPatternFactorySO enemyPatternFactory;
    [SerializeField] private EnemyActionFactorySO enemyActionDataFactory;

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
        allCardMetas.Clear();
        allCardData.Clear();
        allRewardChance.Clear();
        allRewardAmount.Clear();
        allAddCardWeight.Clear();
        allCardPrice.Clear();
        allEditCardPrice.Clear();
        allEnemyData.Clear();
        allEnemyActionData.Clear();
        allEnemyPatternData.Clear();

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
        foreach (var rawCard in googleSheetSO.RawSmithedMagicCardList)
        {
            var card = smithedMagicCardDataFactory.Create(rawCard);
            allSmithedCardMetas.Add(card);
            allSmithedCardData.Add(card.cardData);
        }
        foreach (var rawCard in googleSheetSO.RawSmithedRuneCardList)
        {
            var card = smithedRuneCardDataFactory.Create(rawCard);
            allSmithedCardMetas.Add(card);
            allSmithedCardData.Add(card.cardData);
        }
        for (int i = 0; i < googleSheetSO.RawStorePriceList.Count; i++)
        {
            var rawStorePrice = googleSheetSO.RawStorePriceList[i];

            var storePrice = storePriceDataFactory.Create(rawStorePrice);
            if (i < 4)
            {
                allCardPrice.Add(storePrice);
            }
            else
            {
                allEditCardPrice.Add(storePrice);
            }
        }
        for (int i = 0; i < googleSheetSO.RawRewardChanceList.Count;)
        {
            var rawRewardChance = googleSheetSO.RawRewardChanceList[i];
            var rawAddCardWeight = googleSheetSO.RawAddCardWeightList[i++];
            SeedType seedType = (SeedType)((i * i + i) / 2); // 0 -> 1 (normal), 1 -> 3 (elite), 2 -> 6 (boss)
            var rewardChance = rewardChanceDataFactory.Create(rawRewardChance);
            allRewardChance.Add(seedType, rewardChance);

            var rewardAmount = rewardAmountDataFactory.Create(rawRewardChance);
            allRewardAmount.Add(seedType, rewardAmount);

            var addCardWeight = addCardWeightDataFactory.Create(rawAddCardWeight);
            allAddCardWeight.Add(seedType, addCardWeight);
        }
        foreach (var rawEnemy in googleSheetSO.RawMonsterList)
        {
            var enemy = enemyDataFactory.Create(rawEnemy);
            allEnemyData.Add(enemy);
        }
        foreach (var rawEnemyAction in googleSheetSO.RawMonsterActionList)
        {
            var Action = enemyActionDataFactory.Create(rawEnemyAction);
            allEnemyActionData.Add(Action);
        }
        foreach (var rawEnemyPattern in googleSheetSO.RawMonsterPatternList)
        {
            var pattern = enemyPatternFactory.Create(rawEnemyPattern);
            allEnemyPatternData.Add(pattern);
        }
    }
}
