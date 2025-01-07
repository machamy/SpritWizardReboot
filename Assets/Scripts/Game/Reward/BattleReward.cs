using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleReward : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    [Header("RewardChance")]
    [SerializeField] private RewardChanceSO normalRewardChance;
    [SerializeField] private RewardChanceSO eliteRewardChance;
    [SerializeField] private RewardChanceSO bossRewardChance;

    [Header("RewardAmount")]
    [SerializeField] private RewardAmountSO normalRewardAmount;
    [SerializeField] private RewardAmountSO eliteRewardAmount;
    [SerializeField] private RewardAmountSO bossRewardAmount;

    [Header("AddCardWeight")]
    [SerializeField] private AddCardWeightSO normalAddCardWeight;
    [SerializeField] private AddCardWeightSO eliteAddCardWeight;
    [SerializeField] private AddCardWeightSO bossAddCardWeight;

    private IntRandomSelector<RewardType> normalRewardSelector;
    private IntRandomSelector<RewardType> eliteRewardSelector;
    private IntRandomSelector<RewardType> bossRewardSelector;

    private IntWeightedRandomSelector<Rarity> normalCardRaritySelector;
    private IntWeightedRandomSelector<Rarity> eliteCardRaritySelector;
    private IntWeightedRandomSelector<Rarity> bossCardRaritySelector;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        InitChanceSelector();
        InitCardRaritySelector();
    }

    private void InitChanceSelector()
    {
        RewardType[] rewardTypes = (RewardType[])Enum.GetValues(typeof(RewardType));
        List<RewardType> rewardList = new List<RewardType>(rewardTypes); // None값 제거
        rewardList.RemoveAt(0);
        rewardTypes = rewardList.ToArray();
        normalRewardSelector = new IntRandomSelector<RewardType>(rewardTypes, normalRewardChance.GetAllChances());
        eliteRewardSelector = new IntRandomSelector<RewardType>(rewardTypes, eliteRewardChance.GetAllChances());
        bossRewardSelector = new IntRandomSelector<RewardType>(rewardTypes, bossRewardChance.GetAllChances());
    }

    private void InitCardRaritySelector()
    {
        Rarity[] rarities = (Rarity[])Enum.GetValues(typeof(Rarity));
        normalCardRaritySelector = new IntWeightedRandomSelector<Rarity>(rarities, normalAddCardWeight.GetAllCardWeight());
        eliteCardRaritySelector = new IntWeightedRandomSelector<Rarity>(rarities, eliteAddCardWeight.GetAllCardWeight());
        bossCardRaritySelector = new IntWeightedRandomSelector<Rarity>(rarities, bossAddCardWeight.GetAllCardWeight());
    }

    public RewardType[] GetRewardType(SeedType seed)
    {
        return seed switch
        {
            SeedType.normal => normalRewardSelector.GetRandomChoice().ToArray(),
            SeedType.hard => normalRewardSelector.GetRandomChoice().ToArray(),
            SeedType.elite => eliteRewardSelector.GetRandomChoice().ToArray(),
            SeedType.boss => bossRewardSelector.GetRandomChoice().ToArray(),
            _ => new RewardType[0]
        };
    }

    public Rarity GetNewCardRarity(SeedType seed)
    {
        return seed switch
        {
            SeedType.normal => normalCardRaritySelector.GetRandomChoice(),
            SeedType.hard => normalCardRaritySelector.GetRandomChoice(),
            SeedType.elite => eliteCardRaritySelector.GetRandomChoice(),
            SeedType.boss => bossCardRaritySelector.GetRandomChoice(),
            _ => Rarity.common
        };
    }

    public RewardAmountSO GetRewardAmountSO(SeedType seed)
    {
        if (seed == SeedType.normal || seed == SeedType.hard)
        {
            return normalRewardAmount;
        }
        else if (seed == SeedType.elite)
        {
            return eliteRewardAmount;
        }
        else if (seed == SeedType.boss)
        {
            return bossRewardAmount;
        }
        else
        {
            return ScriptableObject.CreateInstance<RewardAmountSO>();
        }
    }
}
