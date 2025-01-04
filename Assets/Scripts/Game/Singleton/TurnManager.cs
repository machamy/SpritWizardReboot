using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class TurnManager : Singleton<TurnManager>
    {
        [Header("Time")]
        [SerializeField] private float enemyTurnTime = 1.05f;
        public float EnemyTurnTime => enemyTurnTime;
        [Header( "Turn" )]
        public int currentRawTurn = 1;
        public int notMovedEntity = 0;
        [FormerlySerializedAs("readyToEndPlayerTurn")] [FormerlySerializedAs("ready2endTurn")] [SerializeField]private bool isReadyToEndPlayerTurn = false;
        public int CurrentTurn => currentRawTurn / 2;
        public bool IsPlayerTurn => currentRawTurn % 2 == 0;
        public bool IsEnemyTurn => currentRawTurn % 2 == 1;
        [Header("Event Channels")]
        [SerializeField] private EventChannel.TurnEventChannelSO playerTurnEnterEvent;
        [SerializeField] private EventChannel.TurnEventChannelSO playerTurnExitEvent;
        [SerializeField] private EventChannel.TurnEventChannelSO enemyTurnEnterEvent;
        [SerializeField] private EventChannel.TurnEventChannelSO enemyTurnExitEvent;
        

        // /// <summary>
        // /// 테스트용, 사용금지
        // /// </summary>
        // public void NextTurn()
        // {
        //     currentRawTurn++;
        //     if (IsPlayerTurn)
        //     {
        //         playerTurnExitEvent.RaiseTurnEvent(CurrentTurn);
        //         enemyTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
        //     }
        //     else
        //     {
        //         enemyTurnExitEvent.RaiseTurnEvent(CurrentTurn);
        //         playerTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
        //     }
        // }

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            currentRawTurn = 2;
            playerTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
        }
        
        public void ReadyToEndPlayerTurn()
        {
            if (IsPlayerTurn)
            {
                isReadyToEndPlayerTurn = true;
                //TODO 애니메이션 기다리고 턴 끝내야함
                EndPlayerTurn();
            }
        }
        
        private void EndPlayerTurn()
        {
            if (isReadyToEndPlayerTurn)
            {
                isReadyToEndPlayerTurn = false;
                playerTurnExitEvent.RaiseTurnEvent(CurrentTurn);
                currentRawTurn++;
                enemyTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
            }
        }
        
        private IEnumerator EnemyTurnTimer(float time)
        {
            while (time > 0)
            {
                GameManager.Instance.GUIManager.OnEnemyTurnTicking(time);
                time -= Time.deltaTime;
                yield return null;
            }
            if(IsEnemyTurn)
            {
                EndEnemyTurn();
            }
        }
        
        private void EndEnemyTurn()
        {
            if (IsEnemyTurn)
            {
                enemyTurnExitEvent.RaiseTurnEvent(CurrentTurn);
                currentRawTurn++;
                playerTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
            }
        }

        private void OnEnable()
        {
            enemyTurnEnterEvent.OnTurnEventRaised += OnEnemyTurnEnter;
            playerTurnEnterEvent.OnTurnEventRaised += OnPlayerTurnEnter;
            playerTurnExitEvent.OnTurnEventRaised += OnPlayerTurnExit;
            enemyTurnExitEvent.OnTurnEventRaised += OnEnemyTurnExit;
        }

        private void OnDisable()
        {
            enemyTurnEnterEvent.OnTurnEventRaised -= OnEnemyTurnEnter;
            playerTurnEnterEvent.OnTurnEventRaised -= OnPlayerTurnEnter;
            playerTurnExitEvent.OnTurnEventRaised -= OnPlayerTurnExit;
            enemyTurnExitEvent.OnTurnEventRaised -= OnEnemyTurnExit;
        }

        private void OnEnemyTurnEnter(int turn)
        {
            StartCoroutine(EnemyTurnTimer(enemyTurnTime));
        }

        private void OnPlayerTurnEnter(int turn)
        {
            
        }

        private void OnPlayerTurnExit(int turn)
        {
            
        }

        private void OnEnemyTurnExit(int turn)
        {
            
        }
        


        public void ReadyEndPlayerTurn()
        {
            if (IsPlayerTurn)
            {
                isReadyToEndPlayerTurn = true;
            }
        }
        
    }
}