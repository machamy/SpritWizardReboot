using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardDisplay[] cards;

    private Deck deck;
    private Rune rune;
    private Queue<CardSO> deckQueue = new Queue<CardSO>();


    private static CardManager instance;
    public static CardManager Instance
    {
        get
        {
            if (instance == null) return null;
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        deck = GetComponent<Deck>();
        rune = GetComponent<Rune>();
    }

    public void CardDraw()
    {
        for (int i = 0; i < 3; i++)
        {
            if (deckQueue.Count <= 0)
                deckQueue = deck.GetDeckQueue();
            cards[i].DisplayCard(deckQueue.Dequeue());
        }
    }

    public void UseCard(CardSO card)
    {
        if (card == null)
        {
            Debug.Log("선택된 카드 없음");
            return;
        }
        if (card.cardType == CardType.Attack)
        {
            if (card.attackType == AttackType.Ice)
            {
                Debug.Log("얼음공격");
            }
            else if (card.attackType == AttackType.Wind)
            {
                Debug.Log("바람공격");
            }
            else if (card.attackType == AttackType.Fire)
            {
                Debug.Log("불공격");
            }
            else
            {
                Debug.Log("공격카드 아님");
            }
        }
        else if (card.cardType == CardType.Rune)
        {
            rune.AddRuneEffect(card.damageCalculateType, card.damage, card.attackCntCalculateType, card.attackCnt);
        }
        else Debug.Log("카드타입오류");
    }
}
