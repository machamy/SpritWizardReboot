using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private BattleReward battleReward;

    private RewardType rewardType;

    public void ReceiveReward(SeedType seed)
    {
        rewardType = battleReward.GetRewardType(seed);
        switch (rewardType)
        {
            case RewardType.addRuneCard:
                Debug.Log("룬 카드 추가");
                break;
            case RewardType.addAttackCard:
                Debug.Log("공격 카드 추가");
                break;
            case RewardType.destroyCard:
                Debug.Log("카드 제거");
                break;
            case RewardType.upgradeCard:
                Debug.Log("카드 강화");
                break;
            case RewardType.gateHpRestore:
                Debug.Log("회복");
                GameManager.Instance.GateHP += battleReward.GetHpRestoreAmount(seed);
                break;
        }
        //Debug.Log(battleReward.GetGoldAmount(seed) + "원 획득");
    }

    // 카드 관련 코드는 기획이 확실히 나온 이후 작성
    private void AddCard(CardType cardType)
    {
        
    }

    private void DestroyCard()
    {
        
    }

    private void UpgradeCard()
    {

    }
}
