﻿using System;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class HandDeckManager : MonoBehaviour
{
    [FormerlySerializedAs("deckContainer")]
    [FormerlySerializedAs("deckManager")]
    [FormerlySerializedAs("deckController")]
    [FormerlySerializedAs("deck")]
    [Header("References")]
    [SerializeField] private BattleDeckContainer battleDeckContainer;
    [SerializeReference] private BaseCardHolder cardHolder;
    [SerializeReference] private BaseCardSellectableHolder baseCardSellectableHolder;
    [Header("Variables")]
    [SerializeField] private int maxRerollCount = 2;
    [SerializeField] private IntVariableSO rerollCount;
    [Header("Settings")]
    [SerializeField] int handSize = 3;



    /// <summary>
    /// 현재 소지한 카드들로 덱을 설정한다.
    /// </summary>
    /// <param name="cardDataList"></param>
    public void InitDeck(List<CardMetaData> cardDataList)
    {
        foreach (var cardData in cardDataList)
        {
            Debug.Log($"[HandDeckManager::InitDeck] : {cardData.cardName}");
            battleDeckContainer.AddCard(cardData);
        }
    }
    
    public CardObject AddCardToHand(CardMetaData cardMetaData)
    {
        var res = cardHolder.AddCardWithSlot(cardMetaData);
        return res;
    }
    
    public void DrawCard()
    {
        CardMetaData cardMetaData = battleDeckContainer.DrawCard();
        if (cardMetaData == null)
        {
            Debug.Log("더이상 뽑을 수 없습니다.");
            return;
        }
        CardObject cardObject = AddCardToHand(cardMetaData);
        // 만약 Pool을 사용한다면, 함수로 바꾸어 구독 해제 기능도 넣어야함
        cardObject.OnCardDiscarded += co => battleDeckContainer.AddCardToDiscardPool(co.CardMetaData);
        cardObject.RaiseCardDrawn(cardMetaData);
    }
    
    public void ShowInitialDeck()
    {
        ShowCardList(battleDeckContainer.CardDataListRef);
    }
    
    public void ShowDiscardDeck()
    {
        ShowCardList(new List<CardMetaData>(battleDeckContainer.DiscardCardQueueRef));
    }
    
    public void ShowDrawDeck()
    {
        ShowCardList(new List<CardMetaData>(battleDeckContainer.DrawCardQueueRef));   
    }
    
    public void ShowCardList(List<CardMetaData> cardDataList)
    {
        baseCardSellectableHolder.Enable();
        baseCardSellectableHolder.Initialize(cardDataList);
    }
    
    /// <summary>
    /// 설정된 덱으로 뽑을카드 더미와 버릴카드 더미를 만든다.
    /// </summary>
    public void SetupForBattle()
    {
        rerollCount.Value = maxRerollCount;
        battleDeckContainer.SetupForBattle();
        battleDeckContainer.ShuffleDrawPool();
        for (int i = 0; i < handSize; i++)
        {
            DrawCard();
        }
    }

    public void RerollCard()
    {
        // TODO : 플레이어 턴이 아니더라도 실행 가능함...
        if (rerollCount.Value <= 0)
        {
            Debug.Log("더이상 뽑을 수 없습니다.");
            return;
        }
        // while로 처리했다간, 어떤 오류땜에 무한 루프에 빠질 수 있음
        int cardCount = cardHolder.CardCount;
        for (int i = 0; i < cardCount; i++)
        {
            CardObject cardObject = cardHolder[0];
            cardObject.Discard();
        }
        for (int i = 0; i < handSize; i++)
        {
            DrawCard();
        }
        rerollCount.Value--;
    }
    
    // /// <summary>
    // /// 해당 카드를 버릴 카드 더미로 넣는다.
    // /// TODO : 카드 자체에서 할 수 있지 않을까? metadata에 deck정보 넣기는 좀...
    // /// </summary>
    // /// <param name="cardMetaData"></param>
    // public void DiscardCard(CardMetaData cardMetaData)
    // {
    //     deck.AddCardToDiscardPool(cardMetaData);
    // }

    public void OnPlayerTurnEnd()
    {
        while(cardHolder.CardCount > 0)
        {
            CardObject cardObject = cardHolder[0];
            cardObject.Discard();
        }
    }
    
    public void OnPlayerTurnEnter()
    {
        for (int i = cardHolder.CardCount; i < handSize; i++)
        {
            DrawCard();
        }
    }
}
