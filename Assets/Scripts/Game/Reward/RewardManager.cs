using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    //[SerializeField] private BattleReward battleReward;

    private SeedType[] rewardExistInSeed = { SeedType.normal, SeedType.elite, SeedType.boss };
    private RewardType[] rewardTypes;

    private void Start()
    {
    }

    public void ReceiveReward(SeedType seed)
    {
        if (seed == SeedType.hard) seed = SeedType.normal; // hard와 normal은 보상이 같음
        if (!Array.Exists(rewardExistInSeed, v => v == seed)) return; // rest, store는 보상 없음

        rewardTypes = Database.AllRewardChance[seed].GetRandomChoice().ToArray();
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
                    GameManager.Instance.GateHP += Database.AllRewardAmount[seed].gateHpRestoreAmount.GetRandomInRange();
                    Debug.Log("회복");
                    break;
            }
        }
        Debug.Log(Database.AllRewardAmount[seed].gold.GetRandomInRange() + "원 획득");
    }

    // 카드 관련 코드는 기획이 확실히 나온 이후 작성
    private void AddCard(SeedType seed, CardType cardType)
    {
        Rarity rarity = Database.AllAddCardWeight[seed].GetRandomChoice();
    }

    private void DestroyCard()
    {
        
    }

    private void UpgradeCard()
    {

    }
}
