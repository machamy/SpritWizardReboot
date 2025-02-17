using System;
using System.Collections.Generic;
using DefaultNamespace;
using EventChannel;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class BattleManager : Singleton<BattleManager>
    {
        public Board Board => board;
        [Header("Managers")]
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private CardCastManager cardCastManager;
        [SerializeField] private HandDeckManager handDeckManager;
        public TurnManager TurnManager => turnManager;
        public CardCastManager CardCastManager => cardCastManager;
        public HandDeckManager HandDeckManager => handDeckManager;
        [Header("References")]
        [SerializeField] private Board board;
        [Header("Slime")]
        [SerializeField] private Slime iceSlime;
        [SerializeField] private Slime grassSlime;
        [SerializeField] private Slime fireSlime;
        [Header("Deck")]
        [SerializeField] private bool initDeckByDBonEnable = false;
        [SerializeField] private PlayerDataSO playerDataSO;

        /// <summary>
        /// 현재 게임에서의, 플레이어의 덱
        /// </summary>
        public Deck CurrentDeck => playerDataSO.Deck;
        [Header("Battle")]
        [SerializeField] private bool startOnEnable = false;
        [SerializeField] private bool isOnBattle = false;
        [Header("Channel")]
        [SerializeField] private TurnEventChannelSO playerTurnEnterEvent;
        [SerializeField] private TurnEventChannelSO playerTurnEndEvent;
        public bool IsOnBattle => isOnBattle;
        
        private void Awake()
        {
            if(turnManager == null)
                turnManager = FindAnyObjectByType<TurnManager>();
            if(cardCastManager == null)
                cardCastManager = FindAnyObjectByType<CardCastManager>();
            if(handDeckManager == null)
                handDeckManager = FindAnyObjectByType<HandDeckManager>();
            if(board == null)
                board = FindAnyObjectByType<Board>();
        }
        
        private void OnEnable()
        {
            playerTurnEnterEvent.OnTurnEventRaised += OnPlayerTurnEnter;
            playerTurnEndEvent.OnTurnEventRaised += OnPlayerTurnEnd;
            if(initDeckByDBonEnable)
                playerDataSO.InitDeckByDB();
            if(startOnEnable)
                StartBattle();
        }
        
        private void OnDisable()
        {
            playerTurnEnterEvent.OnTurnEventRaised -= OnPlayerTurnEnter;
            playerTurnEndEvent.OnTurnEventRaised -= OnPlayerTurnEnd;
        }

        /// <summary>
        /// 덱을 현재 카드 리스트로 세팅하고, 초기화한다.
        /// 그후 전투를 시작한다.
        /// </summary>
        public void StartBattle()
        {
            isOnBattle = true;
            handDeckManager.InitDeck(CurrentDeck.GetCopidList());
            handDeckManager.SetupForBattle();
            turnManager.StartGame();
        }

        private void OnPlayerTurnEnter(int turn)
        {
            if(IsOnBattle == false)
                return;
            handDeckManager.OnPlayerTurnEnter();
        }
        
        private void OnPlayerTurnEnd(int turn)
        {
            if(IsOnBattle == false)
                return;
            handDeckManager.OnPlayerTurnEnd();
        }
        /// <summary>
        /// 해당 슬라임의 객체를 반환한다.
        /// </summary>
        /// <param name="caster"></param>
        /// <returns></returns>
        public Slime GetSlime(SkillCaster caster)
        {
            return caster switch
            {
                SkillCaster.Ice => iceSlime,
                SkillCaster.Grass => grassSlime,
                SkillCaster.Fire => fireSlime,
                _ => null
            };
        }
    }
}