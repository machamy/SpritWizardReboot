using System;
using System.Collections.Generic;
using EventChannel;
using Game;
using Game.Player;
using UnityEngine;
using Test;
using Game.Entity;
using Random = UnityEngine.Random;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] private Card[] cards;

    private Deck deck;
    private Rune rune;
    private Queue<CardSO> deckQueue = new Queue<CardSO>();

    [SerializeField] private Board board;
    [SerializeField] private Slime iceSlime;
    [SerializeField] private Slime grassSlime;
    [SerializeField] private Slime fireSlime;
    [Header("Channel")]
    [SerializeField] private TurnEventChannelSO playerTurnEnterEvent;

    private void Start()
    {
        deck = GetComponent<Deck>();
        rune = GetComponent<Rune>();
    }

    private void OnEnable()
    {
        playerTurnEnterEvent.OnTurnEventRaised += OnPlayerTurnEnter;
    }

    private void OnDisable()
    {
        playerTurnEnterEvent.OnTurnEventRaised -= OnPlayerTurnEnter;
    }


    private void OnPlayerTurnEnter(int turn)
    {
        CardDraw();
    }
    
    public void CardDraw()
    {
        for (int i = 0; i < 3; i++)
        {
            if (deckQueue.Count <= 0)
                deckQueue = deck.GetDeckQueue();
            cards[i].RaiseCardDrawn(deckQueue.Dequeue());
        }
    }



    public bool UseCard(CardSO card, Vector2Int targetPosition)
    {
        if (card == null)
        {
            Debug.Log("선택된 카드 없음");
            return false;
        }
        Debug.Log($"Cast Card : {card.name}");
        bool isSuccessful = false;
        if (card.cardType == CardType.Attack)
        {
            AttackCardSO attackCard = (AttackCardSO)card;
            Dictionary<RuneEffect, int> runeEffect = rune.GetRuneEffect(); // key -> damage, attackCnt
            int damage = card.damage + runeEffect[RuneEffect.damage];
            int attackCnt = card.attackCnt + runeEffect[RuneEffect.attackCnt];

            Slime slime = attackCard.skillCaster switch
            {
                SkillCaster.Ice => iceSlime,
                SkillCaster.Grass => grassSlime,
                SkillCaster.Fire => fireSlime,
                _ => null
            };
            if (slime == null)
            {
                Debug.Log("슬라임 선택 오류");
                return false;
            }
            if(attackCard.CheckCanSlimeMove(slime.GetComponent<Entity>().Position, targetPosition, attackCard.move))
            {
                slime.CastCard(attackCard, targetPosition);
                isSuccessful = true;
            }
        }
        else if (card.cardType == CardType.Rune)
        {
            RuneCardSO runeCard = (RuneCardSO)card;
            rune.AddRuneEffect(runeCard.damageCalculateType, card.damage, runeCard.attackCntCalculateType, card.attackCnt);
            //TODO 룬카드 구현 
            isSuccessful = true;
        }
        else Debug.Log("카드타입오류");
        
        if(isSuccessful)
            TurnManager.Instance.ReadyToEndPlayerTurn();
        return isSuccessful;
    }
}
