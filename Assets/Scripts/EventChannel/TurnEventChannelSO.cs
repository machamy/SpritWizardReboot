using UnityEngine;

namespace EventChannel
{
    
    [CreateAssetMenu(menuName = "EventChannel/TurnEventChannel")]
    public class TurnEventChannelSO : ScriptableObject
    {
        public delegate void TurnEvent(int turn);
        public event TurnEvent OnTurnEventRaised;
        
        public void RaiseTurnEvent(int turn)
        {
            OnTurnEventRaised?.Invoke(turn);
        }
    }
}