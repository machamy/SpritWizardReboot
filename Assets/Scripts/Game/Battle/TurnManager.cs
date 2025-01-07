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

        /// <summary>
        /// 게임시작 상태로 턴을 세팅하고, 카드를 뽑도록 이벤트를 호출한다.
        /// </summary>
        public void StartGame()
        {
            currentRawTurn = 2;
            playerTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
        }
        
        /// <summary>
        /// 턴을 종료할 준비를 한다.
        /// TODO : 현재로선 바로 종료됨. 애니메이션/모든 행동 이 끝난 후 종료되도록 수정 필요
        /// </summary>
        public void ReadyToEndPlayerTurn()
        {
            if (IsPlayerTurn)
            {
                isReadyToEndPlayerTurn = true;
                // 애니메이션 조건
                // 행동 조건
                EndPlayerTurn();
            }
        }
        
        /// <summary>
        /// 플레이어의 턴을 끝낸다.
        /// </summary>
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
        
        /// <summary>
        /// 적 턴이 시작된 후 자동으로 종료되는 타이머 루틴
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// 적 턴을 끝낸다
        /// </summary>
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
        
    }
}