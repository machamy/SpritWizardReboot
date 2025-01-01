using System;
using UnityEngine;

namespace Game
{
    public class TurnManager : MonoBehaviour
    {
        public int currentRawTurn = 1;
        public int CurrentTurn => currentRawTurn / 2;
        public bool IsPlayerTurn => currentRawTurn % 2 == 1;
        public bool IsEnemyTurn => currentRawTurn % 2 == 0;
        [Header("Event Channels")]
        [SerializeField] private EventChannel.TurnEventChannelSO turnEnterEvent;
        [SerializeField] private EventChannel.TurnEventChannelSO turnExitEvent;
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
                turnExitEvent.RaiseTurnEvent(CurrentTurn);
                Debug.Log($"Player Turn {CurrentTurn}");
            }
            else
            {
                turnEnterEvent.RaiseTurnEvent(CurrentTurn);
                Debug.Log($"Enemy Turn {CurrentTurn}");
            }
        }
        
        public void EndPlayerTurn()
        {
            if (IsPlayerTurn)
            {
                NextTurn();
            }
        }
        
        public void EndEnemyTurn()
        {
            if (IsEnemyTurn)
            {
                NextTurn();
            }
        }
    }
}