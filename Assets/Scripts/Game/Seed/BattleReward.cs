using UnityEngine;

public class BattleReward : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Deck deck;

    public void GetReward()
    {
        switch (Random.Range(0, 7))
        {
            case 0:
                AddCard(CardType.Rune);
                break;
            case 1:
                AddCard(CardType.Attack);
                break;
            case 2:
                RemoveCard();
                break;
            case 3:
                EnforceCard();
                break;
            case 4:
                GetGold();
                break;
            case 5:
                Heal();
                break;

        }
    }

    // TODO -> deck스크립트에서 플레이어덱과 전체덱 구분시키기

    private void AddCard(CardType cardType)
    {
        CardData card;
        if (cardType == CardType.Rune)
        {
            card = deck.allCards[Random.Range(0, deck.allCards.Count + 1)]; // 룬카드 풀로 수정해야함
        }
        else
        {
            card = deck.allCards[Random.Range(0, deck.allCards.Count + 1)]; // 공격카드 풀로 수정해야함
        }
        deck.allCards.Add(card);
    }
    private CardData SelectOneCard()
    {
        return null; // TODO -> 전체 덱을 보여주고 하나 선택하는 기능 만들기
    }

    private void RemoveCard()
    {
        deck.allCards.Remove(SelectOneCard());
    }

    private void EnforceCard()
    {
        
    }
    
    private void GetGold()
    {

    }

    private void Heal()
    {

    }
}
