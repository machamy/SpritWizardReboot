using System;

[Serializable]
public class RewardChanceData
{
    public int addRuneCardChance;
    public int addMagicCardChance;
    public int destroyCaredChance;
    public int upgradeCardChance;
    public int gateHpRestoreChance;

    public int[] GetAllChances()
    {
        return new int[] { addRuneCardChance, addMagicCardChance, destroyCaredChance, upgradeCardChance, gateHpRestoreChance };
    }
}
