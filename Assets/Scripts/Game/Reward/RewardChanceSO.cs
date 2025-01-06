using UnityEngine;

[CreateAssetMenu(fileName = "RewardChance", menuName = "Reward/RewardChance")]
public class RewardChanceSO : ScriptableObject
{
    public int addRuneCardChance;
    public int addAttackCardChance;
    public int destroyCaredChance;
    public int upgradeCardChance;
    public int gateHpRestoreChance;

    public int[] GetAllChances()
    {
        return new int[] { addRuneCardChance, addAttackCardChance, destroyCaredChance, upgradeCardChance, gateHpRestoreChance };
    }
}
