using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class TurnManager : MonoBehaviour
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
        private void Awake()
        {
            currentRawTurn = 1;
        }

        /// <summary>
        /// 테스트용, 사용금지
        /// </summary>
        public void NextTurn()
        {
            currentRawTurn++;
            if (IsPlayerTurn)
            {
                playerTurnExitEvent.RaiseTurnEvent(CurrentTurn);
                enemyTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
            }
            else
            {
                enemyTurnExitEvent.RaiseTurnEvent(CurrentTurn);
                playerTurnEnterEvent.RaiseTurnEvent(CurrentTurn);
            }
        }

        private void OnEnable()
        {
            enemyTurnEnterEvent.OnTurnEventRaised += OnEnemyTurnEnter;
        }
        
        private void OnDisable()
        {
            enemyTurnEnterEvent.OnTurnEventRaised -= OnEnemyTurnEnter;
        }
        
        private void OnEnemyTurnEnter(int turn)
        {
            StartCoroutine(EnemyTurnTimer(enemyTurnTime));
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
                NextTurn();
        }

        public void ReadyEndPlayerTurn()
        {
            if (IsPlayerTurn)
            {
                isReadyToEndPlayerTurn = true;
            }
        }
        
        /// <summary>
        /// TODO : 모든 애니메이션 종료 조건 추가 필요
        /// </summary>
        /// <returns></returns>
        private IEnumerable CheckPlayerTurnEnd()
        {
            yield return new WaitWhile(()=>isReadyToEndPlayerTurn);
            isReadyToEndPlayerTurn = false;
            NextTurn();
        }
    }
}