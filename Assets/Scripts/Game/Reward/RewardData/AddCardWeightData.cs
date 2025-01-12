using System;

[Serializable]
public class AddCardWeightData : IntWeightedRandomSelector<Rarity>
{
    public int commonCardWeight;
    public int rareCardWeight;

    public void SetValue()
    {
        SetValue(new int[] { commonCardWeight, rareCardWeight });
    }
}