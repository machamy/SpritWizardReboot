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

    [Header("RewardAmount")]
    [SerializeField] private RewardAmountSO normalRewardAmount;
    [SerializeField] private RewardAmountSO eliteRewardAmount;

    [Header("AddCardWeight")]
    [SerializeField] private AddCardWeightSO normalAddCardWeight;
    [SerializeField] private AddCardWeightSO eliteAddCardWeight;
    [SerializeField] private AddCardWeightSO bossAddCardWeight;

    private IntWeightedRandomSelector<RewardType> normalRewardSelector;
    private IntWeightedRandomSelector<RewardType> eliteRewardSelector;

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
        List<RewardType> rewardList = new List<RewardType>(rewardTypes);
        rewardList.RemoveAt(0);
        rewardTypes = rewardList.ToArray();
        normalRewardSelector = new IntWeightedRandomSelector<RewardType>(rewardTypes, normalRewardChance.GetAllChances());
        eliteRewardSelector = new IntWeightedRandomSelector<RewardType>(rewardTypes, eliteRewardChance.GetAllChances());
    }

    private void InitCardRaritySelector()
    {
        Rarity[] rarities = (Rarity[])Enum.GetValues(typeof(Rarity));
        normalCardRaritySelector = new IntWeightedRandomSelector<Rarity>(rarities, normalAddCardWeight.GetAllCardWeight());
        eliteCardRaritySelector = new IntWeightedRandomSelector<Rarity>(rarities, eliteAddCardWeight.GetAllCardWeight());
        bossCardRaritySelector = new IntWeightedRandomSelector<Rarity>(rarities, bossAddCardWeight.GetAllCardWeight());
    }

    public RewardType GetRewardType(SeedType seed)
    {
        return seed switch
        {
            SeedType.normal => normalRewardSelector.GetRandomChoice(),
            SeedType.hard => normalRewardSelector.GetRandomChoice(),
            SeedType.elite => eliteRewardSelector.GetRandomChoice(),
            SeedType.boss => RewardType.None, // TODO 보스 후 보상 기획
            _ => RewardType.None
        };
    }
    
    public int GetHpRestoreAmount(SeedType seed)
    {
        RewardAmountSO rewardAmount = GetRewardAmountSO(seed);
        if (rewardAmount == null)
        {
            Debug.Log("시드 오류");
            return 0;
        }

        return GetRandomInVariation(rewardAmount.gateHpRestoreAmountMiddle, rewardAmount.gateHpRestoreAmountVariation);
    }

    public int GetGoldAmount(SeedType seed)
    {
        RewardAmountSO rewardAmount = GetRewardAmountSO(seed);
        if (rewardAmount == null) return 0;

        return GetRandomInVariation(rewardAmount.goldMiddle, rewardAmount.goldVariation);
    }

    private RewardAmountSO GetRewardAmountSO(SeedType seed)
    {
        if (seed == SeedType.normal || seed == SeedType.hard)
        {
            return normalRewardAmount;
        }
        else if (seed == SeedType.elite)
        {
            return eliteRewardAmount;
        }
        else
        {
            return null;
        }
    }

    private int GetRandomInVariation(int middle, int variation)
    {
        return Random.Range(middle - variation, middle + variation + 1);
    }
}
