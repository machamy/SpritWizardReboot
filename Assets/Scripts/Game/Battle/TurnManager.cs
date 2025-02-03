using System;
using System.Collections;
using DefaultNamespace;
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
        [SerializeField]private bool isReadyToEndPlayerTurn = false;
        [SerializeField]private IntVariableSO remainEnemyTurnSO;
        private int remainEnemyTurn
        {
            get => remainEnemyTurnSO.Value;
            set => remainEnemyTurnSO.Value = value;
        } 
        [FormerlySerializedAs("waitsForEndPlayerTurn")] [SerializeField]private IntVariableSO waitsForEndPlayerTurnSO;
        private int playerUsedCost;
        public int CurrentTurn => currentRawTurn;
        private bool isPlayerTurn = false;
        public bool IsPlayerTurn => isPlayerTurn;
        public bool IsPlayerTurnStrict => isPlayerTurn && !isReadyToEndPlayerTurn;
        public bool IsEnemyTurn => !isPlayerTurn;
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
            isPlayerTurn = true;
            currentRawTurn = 1;
            playerTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
        }
        
        /// <summary>
        /// 턴을 종료할 준비를 한다.
        /// TODO : 현재로선 바로 종료됨. 애니메이션/모든 행동 이 끝난 후 종료되도록 수정 필요
        /// </summary>
        public void ReadyToEndPlayerTurn(int usedCost)
        {
            if (IsPlayerTurn)
            {
                isReadyToEndPlayerTurn = true;
                playerUsedCost = usedCost;
                waitsForEndPlayerTurnSO.OnValueChanged -= CheckWaitsForEndPlayerTurnSo;
                waitsForEndPlayerTurnSO.OnValueChanged += CheckWaitsForEndPlayerTurnSo;
                CheckWaitsForEndPlayerTurnSo(waitsForEndPlayerTurnSO.Value);
            }
        }
        
        private void CheckWaitsForEndPlayerTurnSo(int val)
        {
            if (isReadyToEndPlayerTurn && val <= 0)
            {
                EndPlayerTurn(playerUsedCost); // 순서 바뀌면 무한 재귀에 빠짐
                waitsForEndPlayerTurnSO.Value = 0;
            }
        }
        
        /// <summary>
        /// 대기열에 플레이어 턴 종료를 기다리는 수를 추가한다.
        /// </summary>
        public void AddWaitsForEndPlayerTurn()
        {
            waitsForEndPlayerTurnSO.Value += 1;
        }
        /// <summary>
        /// 대기열에 플레이어 턴 종료를 기다리는 수를 제거한다.
        /// </summary>
        public void RemoveWaitsForEndPlayerTurn()
        {
            waitsForEndPlayerTurnSO.Value -= 1;
        }
        
        /// <summary>
        /// 플레이어의 턴을 끝낸다.
        /// </summary>
        private void EndPlayerTurn(int usedCost)
        {
            if (isReadyToEndPlayerTurn)
            {
                isReadyToEndPlayerTurn = false;
                playerTurnExitEvent.RaiseTurnEvent(CurrentTurn);
                remainEnemyTurn = usedCost;
                isPlayerTurn = false;
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
                GameManager.Instance.GUIManager.OnEnemyTurnTicking(time, enemyTurnTime);
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
                if(remainEnemyTurn > 0)
                {
                    remainEnemyTurn--;
                    enemyTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
                }
                else
                {
                    currentRawTurn++;
                    isPlayerTurn = true;
                    playerTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
                }
                
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