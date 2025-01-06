using UnityEngine;

[CreateAssetMenu(fileName = "AddCardWeight", menuName = "Reward/AddCardWeight")]
public class AddCardWeightSO : ScriptableObject
{
    public int rareCardWeight;
    public int commonCardWeight;

    public int[] GetAllCardWeight()
    {
        return new int[] { rareCardWeight, commonCardWeight };
    }
}
