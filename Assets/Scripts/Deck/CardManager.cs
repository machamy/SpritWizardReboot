using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using Test;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardDisplay[] cards;

    private Deck deck;
    private Rune rune;
    private Queue<CardSO> deckQueue = new Queue<CardSO>();

    [SerializeField] private Slime iceSlime;
    [SerializeField] private Slime grassSlime;
    [SerializeField] private Slime fireSlime;

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
            AttackCardSO attackCard = (AttackCardSO)card;
            Dictionary<string, int> runeEffect = rune.GetRuneEffect(); // key -> damage, attackCnt
            int damage = card.damage + runeEffect["damage"];
            int attackCnt = card.attackCnt + runeEffect["attackCnt"];
        
            Vector2Int randomPos = new Vector2Int(Random.Range(0, 15), Random.Range(0, 6));
            
            if (attackCard.skillCaster == SkillCaster.Ice)
            {
                Debug.Log("얼음공격");
                Debug.Log(attackCard.attackRange);
                iceSlime.CastCard(attackCard, randomPos);
            }
            else if (attackCard.skillCaster == SkillCaster.Grass)
            {
                Debug.Log("바람공격");
                grassSlime.CastCard(attackCard, randomPos);
            }
            else if (attackCard.skillCaster == SkillCaster.Fire)
            {
                Debug.Log("불공격");
                fireSlime.CastCard(attackCard, randomPos);
            }
            else
            {
                Debug.Log("공격카드 아님");
            }
        }
        else if (card.cardType == CardType.Rune)
        {
            RuneCardSO runeCard = (RuneCardSO)card;
            rune.AddRuneEffect(runeCard.damageCalculateType, card.damage, runeCard.attackCntCalculateType, card.attackCnt);
        }
        else Debug.Log("카드타입오류");
        
        CardDraw();
        //TODO : 턴종료, 모든 애니메이션 종료 후 적 턴으로
    }
}
