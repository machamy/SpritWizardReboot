using System.Collections.Generic;
using UnityEngine;


public class HandDeckManager : MonoBehaviour
{
    [SerializeField] private Deck deck;
    // [SerializeField] private Hand hand;

    [SerializeField] private List<Card> cards;
    
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
    
    public void OnPlayerTurnEnter()
    {
        for (int i = 0; i < 3; i++)
        {
            var card = cards[i];
            // if(card.CardSelect.IsUsed)
            deck.AddCardToDiscardPool(card.CardMetaData);
            var drawnCard = deck.DrawCard();
            
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
