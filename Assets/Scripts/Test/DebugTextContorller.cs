using EventChannel;
using TMPro;
using UnityEngine;

namespace Test
{
    public class DebugTextContorller : MonoBehaviour
    {
        public TextMeshProUGUI CurrentTurnText;
        
        public TurnEventChannelSO turnEnterEvent;
        public TurnEventChannelSO turnExitEvent;
        
        private void OnEnable()
        {
            turnEnterEvent.OnTurnEventRaised += OnTurnEnter;
            turnExitEvent.OnTurnEventRaised += OnTurnExit;
        }
        
        private void OnDisable()
        {
            turnEnterEvent.OnTurnEventRaised -= OnTurnEnter;
            turnExitEvent.OnTurnEventRaised -= OnTurnExit;
        }
        
        private void OnTurnEnter(int turn)
        {
            CurrentTurnText.text = $"Player Turn {turn}";
        }
        
        private void OnTurnExit(int turn)
        {
            CurrentTurnText.text = $"Enemy Turn {turn}";
        }
    }
}