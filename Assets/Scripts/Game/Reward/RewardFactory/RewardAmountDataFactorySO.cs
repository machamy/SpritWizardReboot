using UnityEngine;

[CreateAssetMenu(fileName = "RewardAmountFactory", menuName = "RewardFactory/RewardAmountFactory")]
public class RewardAmountDataFactorySO : ScriptableObject
{
    public RewardAmountData Create(RawRewardChance rawRewardChance)
    {
        var amount = new RewardAmountData
        {
            gateHpRestoreAmount = new RangeValue(rawRewardChance.gateHpRestoreAmountMiddle, rawRewardChance.gateHpRestoreAmountVariation),
            gold = new RangeValue(rawRewardChance.goldMiddle, rawRewardChance.goldVariation)
        };

        return amount;
    }
}
