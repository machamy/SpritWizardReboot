using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Time")]
        [SerializeField] private float enemyTurnTime = 1.0f;
        public float EnemyTurnTime => enemyTurnTime;
        [Header( "Turn" )]
        public int currentRawTurn = 1;
        public int notMovedEntity = 0;
        [SerializeField]private bool ready2endTurn = false;
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
        
        public void ReadyEndPlayerTurn()
        {
            if (IsPlayerTurn)
            {
                ready2endTurn = true;
            }
        }
        
        public void ReadyEndEnemyTurn()
        {
            if (IsEnemyTurn)
            {
                ready2endTurn = true;
            }
        }
        
        private IEnumerable CheckTurn()
        {
            yield return new WaitWhile( ()=>(ready2endTurn && notMovedEntity > 0));
            ready2endTurn = false;
            NextTurn();
        }
    }
}