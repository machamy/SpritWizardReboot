using UnityEngine;

[CreateAssetMenu(fileName = "RewardAmount", menuName = "Reward/RewardAmount")]
public class RewardAmountSO : ScriptableObject
{
    public RangeValue gateHpRestoreAmount;
    public RangeValue gold;
}
