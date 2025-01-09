using System.Collections.Generic;
using UnityEngine;


public class HandDeckManager : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private BaseCardHolder cardHolder;
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
            deck.AddCard(cardData);
        }
    }
    
    public CardObject AddCardToHand(CardMetaData cardMetaData)
    {
        var res = cardHolder.AddCardWithSlot(cardMetaData);
        return res;
    }
    
    public void DrawCard()
    {
        CardMetaData cardMetaData = deck.DrawCard();
        AddCardToHand(cardMetaData).RaiseCardDrawn(cardMetaData);
    }
    
    /// <summary>
    /// 설정된 덱으로 뽑을카드 더미와 버릴카드 더미를 만든다.
    /// </summary>
    public void SetupForBattle()
    {
        deck.SetupForBattle();
        deck.ShuffleDrawPool();
        for (int i = 0; i < handSize; i++)
        {
            DrawCard();
        }
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
