using System;
using System.Collections.Generic;
using DataBase.DataClasses;
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
    private RuneEffectHolder _runeEffectHolder;
    private Queue<CardData> deckQueue = new Queue<CardData>();

    [SerializeField] private Board board;
    [SerializeField] private Slime iceSlime;
    [SerializeField] private Slime grassSlime;
    [SerializeField] private Slime fireSlime;
    [Header("Channel")]
    [SerializeField] private TurnEventChannelSO playerTurnEnterEvent;

    private void Start()
    {
        deck = GetComponent<Deck>();
        _runeEffectHolder = GetComponent<RuneEffectHolder>();
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
    
    
    public bool UseCard(CardData cardData, Vector2Int targetPosition)
    {
        if (cardData == null)
        {
            Debug.Log("No card selected");
            return false;
        }
        Debug.Log($"Cast Card : {cardData.cardName}");
        // bool isSuccessful = false;
        // if (card.cardType == CardType.Attack)
        // {
        //     AttackCardSO attackCard = (AttackCardSO)card;
        //     Dictionary<RuneEffect, int> runeEffect = rune.GetRuneEffect(); // key -> damage, attackCnt
        //     int damage = card.damage + runeEffect[RuneEffect.damage];
        //     int attackCnt = card.attackCnt + runeEffect[RuneEffect.attackCnt];

        // CardAction action = cardData.cardType switch
        // {
        //     CardType.Attack => new AttackAction(DetermineSlime((AttackCardData)cardData)),
        //     _ => null
        // };

        // if (action != null)
        // {
        //     action.Execute(cardData, targetPosition);
        //     TurnManager.Instance.ReadyToEndPlayerTurn();
        //     return true;
        // }
        //
        // Debug.Log("Invalid card type");
        // return false;
        return false;
    }

    private Slime DetermineSlime(MagicCardData cardData)
    {
        return cardData.skillCaster switch
        {
            SkillCaster.Ice => iceSlime,
            SkillCaster.Grass => grassSlime,
            SkillCaster.Fire => fireSlime,
            _ => null
        };
    }
}
