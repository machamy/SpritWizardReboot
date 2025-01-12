using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AddCardWeightFactory", menuName = "RewardFactory/AddCardWeightFactory")]
public class AddCardWeightDataFactorySO : ScriptableObject
{
    public AddCardWeightData Create(RawAddCardWeight rawAddCardWeight)
    {
        var weight = new AddCardWeightData()
        {
            commonCardWeight = rawAddCardWeight.commonCardWeight,
            rareCardWeight = rawAddCardWeight.rareCardWeight,
        };
        weight.SetValue();

        return weight;
    }
}