using EventChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerDataSO;
    [SerializeField] private BaseCardSellectableHolder baseCardSellectableHolder;
    [SerializeField] private PhaseEventChannelSO endPhaseEvent;

    private SeedType[] rewardExistInSeed = { SeedType.normal, SeedType.elite, SeedType.boss };
    private Queue<RewardType> rewardQueue;
    private RewardType currentRewardType;
    private SeedType currentSeed;
    private CardType currentCardType;
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

    /// <summary>
    /// 보상 부여 시작
    /// </summary>
    public void InitReward(SeedType seed)
    {
        if (seed == SeedType.hard) seed = SeedType.normal; // hard와 normal은 보상이 같음
        if (!Array.Exists(rewardExistInSeed, v => v == seed)) return; // rest, store는 보상 없음

        currentSeed = seed;
        rewardQueue = new Queue<RewardType>(Database.AllRewardChance[seed].GetRandomChoice());

        ProcessRewards();
    }

    /// <summary>
    /// 보상 부여 작업
    /// </summary>
    private void ProcessRewards()
    {
        if (rewardQueue == null) return;

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
        if (baseCardSellectableHolder.sellectedCardObjects.Count > 0)
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
        }

        isProcessingReward = false;
        Invoke(nameof(ProcessRewards), 1); // 카드 홀더 닫자마자 바로 불러오기 하면 오류가 생기기에 각 보상 사이에 1초 지연시간 줌
    }

    /// <summary>
    /// 카드 선책창 띄우기
    /// </summary>
    private void ShowCardList(List<CardMetaData> cards)
    {
        baseCardSellectableHolder.Enable();
        baseCardSellectableHolder.Initialize(cards);
    }
    
    /// <summary>
    /// 게임 내 전체 카드에서 선택할 카드 불러오기
    /// </summary>
    private void SelectCard(CardType cardType)
    {
        currentCardType = cardType;
        Rarity rarity = Database.AllAddCardWeight[currentSeed].GetRandomChoice();
        ShowCardList(Database.AllCardMetas.Where(e => e.cardType == cardType && e.rarity == rarity).ToList());
    }

    /// <summary>
    /// 카드 리롤 기능
    /// </summary>
    public void ReRollCard()
    {
        SelectCard(currentCardType);
    }

    /// <summary>
    /// 플레이어가 가진 카드 중에서 선택할 카드 불러오기
    /// </summary>
    private void SelectFronMyCard()
    {
        ShowCardList(playerDataSO.CardList); // TODO => 전투 종료 후 카드들이 어떻게 될건지 기획 나오고 그에 따라 수정 -> 아래의 카드제어 기능들도 마찬가지
    }

    /// <summary>
    /// 카드 추가
    /// </summary>
    private void AddCard(CardMetaData cardMeta)
    {
        Debug.Log("카드추가" + cardMeta.cardKoreanName);
        playerDataSO.CardList.Add(cardMeta);
    }

    /// <summary>
    /// 카드 제거
    /// </summary>
    private void DestroyCard(CardMetaData cardMeta)
    {
        Debug.Log("카드제거" + cardMeta.cardKoreanName);
        playerDataSO.CardList.Remove(cardMeta);
    }

    /// <summary>
    /// 기존 카드 제거 후 새 강화 카드 추가
    /// </summary>
    private void UpgradeCard(CardMetaData cardMeta)
    {
        Debug.Log("카드강화" + cardMeta.cardKoreanName);
        DestroyCard(cardMeta);
        AddCard(Database.AllSmithedCardMetas.Where(e => e.isSmithed && e.cardId == cardMeta.cardId).ToList()[0]);
    }
}
