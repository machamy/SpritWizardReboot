using System.Collections.Generic;
using UnityEngine;


public class HandDeckManager : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private List<CardObject> cards;
    
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
    
    /// <summary>
    /// 설정된 덱으로 뽑을카드 더미와 버릴카드 더미를 만든다.
    /// </summary>
    public void SetupForBattle()
    {
        deck.SetupForBattle();
        deck.ShuffleDrawPool();
    }
    
    /// <summary>
    /// 해당 카드를 버릴 카드 더미로 넣는다.
    /// TODO : 카드 자체에서 할 수 있지 않을까? metadata에 deck정보 넣기는 좀...
    /// </summary>
    /// <param name="cardMetaData"></param>
    public void DiscardCard(CardMetaData cardMetaData)
    {
        deck.AddCardToDiscardPool(cardMetaData);
    }
    
    public void OnPlayerTurnEnter()
    {
        // TODO 카드 버리기/ 뽑기 분리
        for (int i = 0; i < 3; i++)
        {
            var card = cards[i];
            var drawnCard = deck.DrawCard();
            if(card.CardMetaData != null && !card.CardSelect.IsUsed)
                card.Discard();
            
            if (drawnCard == null)
            {
                Debug.Log($"[HandDeckManager::OnPlayerTurnEnter] no card to draw");
                card.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"[HandDeckManager::OnPlayerTurnEnter] draw card : {drawnCard.cardName}");
                card.gameObject.SetActive(true);
                cards[i].RaiseCardDrawn(drawnCard);
            }
           
        }
    }
}
