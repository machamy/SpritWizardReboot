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
        
        [SerializeField,Tooltip("전체 카드를 현재 카드 리스트로 적용")] private bool initCardDataListByAllCardDataList = false;
        [SerializeField,Tooltip("현재 게임에서의, 플레이어의 덱")] private List<CardMetaData> currentCardDataList;

        /// <summary>
        /// 현재 게임에서의, 플레이어의 덱
        /// </summary>
        public List<CardMetaData> CurrentCardDataList => currentCardDataList;
        [Header("Battle")]
        [SerializeField] private bool isOnBattle = false;
        [Header("Channel")]
        [SerializeField] private TurnEventChannelSO playerTurnEnterEvent;
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

        private void Start()
        {
            // 게임 시작할때 전체 카드를 받아올지?
            if(initCardDataListByAllCardDataList)
                InitCardDataListByAllCardDataList();
        }
        private void OnEnable()
        {
            playerTurnEnterEvent.OnTurnEventRaised += OnPlayerTurnEnter;
        }
        
        private void OnDisable()
        {
            playerTurnEnterEvent.OnTurnEventRaised -= OnPlayerTurnEnter;
        }

        /// <summary>
        /// 덱을 현재 카드 리스트로 세팅하고, 초기화한다.
        /// 그후 전투를 시작한다.
        /// </summary>
        public void StartBattle()
        {
            isOnBattle = true;
            handDeckManager.InitDeck(currentCardDataList);
            handDeckManager.SetupForBattle();
            turnManager.StartGame();
        }

        private void OnPlayerTurnEnter(int turn)
        {
            if(IsOnBattle == false)
                return;
            handDeckManager.OnPlayerTurnEnter();
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
        
        
        
        [ContextMenu("InitCardDataListByAllCardDataList")]
        public void InitCardDataListByAllCardDataList()
        {
            currentCardDataList = new List<CardMetaData>(Database.AllCardMetas);
        }
    }
}