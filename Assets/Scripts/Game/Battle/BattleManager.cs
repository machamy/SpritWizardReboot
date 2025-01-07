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
        [SerializeField] private bool initCardDataListByAllCardDataList = false;
        [SerializeField] private List<CardMetaData> currentCardDataList;
        
        public List<CardMetaData> CurrentCardDataList => currentCardDataList;
        [Header("Battle")]
        [SerializeField] private bool isOnBattle = false;
        [SerializeField] private IntVariableSO mana;
        [SerializeField] private IntVariableSO maxMana;
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

        public void StartBattle()
        {
            isOnBattle = true;
            handDeckManager.InitDeck(currentCardDataList);
            handDeckManager.SetupForBattle();
            turnManager.StartGame();
        }

        public void OnPlayerTurnEnter(int turn)
        {
            if(IsOnBattle == false)
                return;
            handDeckManager.OnPlayerTurnEnter();
        }
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