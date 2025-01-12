using UnityEngine;

[CreateAssetMenu(fileName = "RewardChanceFactory", menuName = "RewardFactory/RewardChanceFactory")]
public class RewardChanceDataFactorySO : ScriptableObject
{
    public RewardChanceData Create(RawRewardChance rawRewardChance)
    {
        var chance = new RewardChanceData
        {
            addRuneCardChance = rawRewardChance.addRuneCardChance,
            addMagicCardChance = rawRewardChance.addMagicCardChance,
            destroyCaredChance = rawRewardChance.destroyCardChance,
            upgradeCardChance = rawRewardChance.upgradeCardChance,
            gateHpRestoreChance = rawRewardChance.gateHpRestoreChance,
        };
        chance.SetValue();

        return chance;
    }
}