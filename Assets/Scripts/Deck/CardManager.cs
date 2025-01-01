using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using Test;
using Game.Entity;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardDisplay[] cards;

    private Deck deck;
    private Rune rune;
    private Queue<CardSO> deckQueue = new Queue<CardSO>();

    [SerializeField] private Board board;
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

    private bool CheckCanSlimeMove(Slime slime, Vector2Int targetPosition)
    {
        Vector2Int slimePosition = slime.GetComponent<Entity>().Position;
        int deltaX = targetPosition.x - slimePosition.x;
        int deltaY = targetPosition.y - slimePosition.y;
        if (deltaX == deltaY || deltaX == -deltaY || deltaX * deltaY == 0) return true;
        return false;
    }

    public void UseCard(CardSO card, Vector2Int targetPosition)
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
                if (CheckCanSlimeMove(iceSlime, targetPosition))
                {
                    Debug.Log("얼음공격");
                    iceSlime.CastCard(attackCard, targetPosition);
                }
            }
            else if (attackCard.skillCaster == SkillCaster.Grass)
            {
                if (CheckCanSlimeMove(grassSlime, targetPosition))
                {
                    Debug.Log("풀공격");
                    grassSlime.CastCard(attackCard, targetPosition);
                }
            }
            else if (attackCard.skillCaster == SkillCaster.Fire)
            {
                if (CheckCanSlimeMove(fireSlime, targetPosition))
                {
                    Debug.Log("불공격");
                    fireSlime.CastCard(attackCard, targetPosition);
                }
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
