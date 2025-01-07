using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private BattleReward battleReward;

    private RewardType[] rewardTypes;

    public void ReceiveReward(SeedType seed)
    {
        rewardTypes = battleReward.GetRewardType(seed);
        foreach (RewardType rewardType in rewardTypes)
        {
            switch (rewardType)
            {
                case RewardType.addRuneCard:
                    AddCard(seed, CardType.Rune);
                    Debug.Log("룬 카드 추가");
                    break;
                case RewardType.addAttackCard:
                    AddCard(seed, CardType.Attack);
                    Debug.Log("공격 카드 추가");
                    break;
                case RewardType.destroyCard:
                    DestroyCard();
                    Debug.Log("카드 제거");
                    break;
                case RewardType.upgradeCard:
                    UpgradeCard();
                    Debug.Log("카드 강화");
                    break;
                case RewardType.gateHpRestore:
                    GameManager.Instance.GateHP += battleReward.GetRewardAmountSO(seed).gateHpRestoreAmount.GetRandomInRange();
                    Debug.Log("회복");
                    break;
            }
        }
        Debug.Log(battleReward.GetRewardAmountSO(seed).gold.GetRandomInRange() + "원 획득");
    }

    // 카드 관련 코드는 기획이 확실히 나온 이후 작성
    private void AddCard(SeedType seed, CardType cardType)
    {
        Rarity rarity = battleReward.GetNewCardRarity(seed);
    }

    private void DestroyCard()
    {
        
    }

    private void UpgradeCard()
    {

    }
}
