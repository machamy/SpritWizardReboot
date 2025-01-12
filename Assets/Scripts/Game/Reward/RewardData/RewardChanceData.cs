using System;

[Serializable]
public class RewardChanceData : IntRandomSelector<RewardType>
{
    public int addRuneCardChance;
    public int addMagicCardChance;
    public int destroyCaredChance;
    public int upgradeCardChance;
    public int gateHpRestoreChance;

    public void SetValue()
    {
        SetValue(new int[] { 0, addRuneCardChance, addMagicCardChance, destroyCaredChance, upgradeCardChance, gateHpRestoreChance });
    }
}
