using EventChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private BaseCardSellectableHolder baseCardSellectableHolder;
    [SerializeField] private PhaseEventChannelSO endPhaseEvent;

    private SeedType[] rewardExistInSeed = { SeedType.normal, SeedType.elite, SeedType.boss };
    private Queue<RewardType> rewardQueue;
    private RewardType currentRewardType;
    private SeedType currentSeed;
    private bool isProcessingReward = false;

    private void OnEnable()
    {
        baseCardSellectableHolder.OnExitSuccessfully += OnCardSelected;
        endPhaseEvent.OnPhaseEventRaised += InitReward;
    }

    private void OnDisable()
    {
        baseCardSellectableHolder.OnExitSuccessfully -= OnCardSelected;
        endPhaseEvent.OnPhaseEventRaised -= InitReward;
    }

    public void InitReward(SeedType seed)
    {
        if (seed == SeedType.hard) seed = SeedType.normal; // hard와 normal은 보상이 같음
        if (!Array.Exists(rewardExistInSeed, v => v == seed)) return; // rest, store는 보상 없음

        currentSeed = seed;
        rewardQueue = new Queue<RewardType>(Database.AllRewardChance[seed].GetRandomChoice());

        ProcessRewards();
    }

    private void ProcessRewards()
    {
        if (rewardQueue == null) return;
        Debug.Log("부여프로세스");
        while (rewardQueue.Count > 0 && !isProcessingReward)
        {
            isProcessingReward = true;

            currentRewardType = rewardQueue.Dequeue();
            switch (currentRewardType)
            {
                case RewardType.addRuneCard:
                    SelectCard(CardType.Rune);
                    return;
                case RewardType.addAttackCard:
                    SelectCard(CardType.Attack);
                    return;
                case RewardType.destroyCard:
                case RewardType.upgradeCard:
                    SelectFronMyCard();
                    return;
                case RewardType.gateHpRestore:
                    GameManager.Instance.GateHP += Database.AllRewardAmount[currentSeed].gateHpRestoreAmount.GetRandomInRange();
                    isProcessingReward = false;
                    break;
            }
        }

        if (rewardQueue.Count == 0)
        {
            Debug.Log(Database.AllRewardAmount[currentSeed].gold.GetRandomInRange() + "원 획득");
            isProcessingReward = false;
            rewardQueue = null;
        }
    }

    /// <summary>
    /// 보상 종류에 따른 선택된 카드 관리
    /// </summary>
    private void OnCardSelected(BaseCardSellectableHolder baseCardSellectableHolder)
    {
        CardMetaData cardMetaData = baseCardSellectableHolder.sellectedCardObjects[0].CardMetaData;
        switch (currentRewardType)
        {
            case RewardType.addRuneCard:
            case RewardType.addAttackCard:
                AddCard(cardMetaData);
                break;
            case RewardType.destroyCard:
                DestroyCard(cardMetaData);
                break;
            case RewardType.upgradeCard:
                UpgradeCard(cardMetaData);
                break;
        }

        isProcessingReward = false;
        Invoke(nameof(ProcessRewards), 1);
    }

    private void ShowCardList(List<CardMetaData> cards)
    {
        baseCardSellectableHolder.Enable();
        baseCardSellectableHolder.Initialize(cards);
    }

    private void SelectCard(CardType cardType)
    {
        Rarity rarity = Database.AllAddCardWeight[currentSeed].GetRandomChoice();
        ShowCardList(Database.AllCardMetas.Where(e => e.cardType == cardType && e.rarity == rarity).ToList());
    }

    private void SelectFronMyCard() // 덱부분 제대로 안가져와서인지 못불러옴
    {
        //ShowCardList(); // TODO => 전투 종료 후 카드들이 어떻게 될건지 기획 나오고 그에 따라 수정 -> 아래의 카드제어 기능들도 마찬가지
    }

    private void AddCard(CardMetaData cardMeta)
    {
        Debug.Log("카드추가" + cardMeta.cardKoreanName);
        //deck.AddCardToDrawPool(cardMeta);
    }

    private void DestroyCard(CardMetaData cardMeta)
    {
        Debug.Log("카드제거" + cardMeta.cardKoreanName);
        // 카드 제거 기능 필요
    }

    private void UpgradeCard(CardMetaData cardMeta)
    {
        DestroyCard(cardMeta); // 기존 제거 후 새 강화 카드 추가
        CardMetaData newCard = Database.AllSmithedCardMetas.Where(e => e.isSmithed && e.cardId == cardMeta.cardId).ToList()[0];
        Debug.Log("카드강화" + cardMeta.cardKoreanName);
        // TODO => 딜 상승 등등 카드 강화효과 적용 => 기획 나와야함
    }
}
