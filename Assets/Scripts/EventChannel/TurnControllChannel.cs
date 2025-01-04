using UnityEngine;

namespace EventChannel
{
    [CreateAssetMenu(menuName = "EventChannel/TurnControllChannel")]
    public class TurnControllChannel : ScriptableObject
    {
        public delegate bool PlayerTurnEnd();
        public delegate void PlayerTurnStart();
        public event PlayerTurnEnd OnPlayerTurnEnd;
        public event PlayerTurnStart OnPlayerTurnStart;
        
        public bool RaisePlayerTurnEnd()
        {
            if (OnPlayerTurnEnd == null) return false;
            OnPlayerTurnEnd();
            return true;
        }
        
        public void RaisePlayerTurnStart()
        {
            OnPlayerTurnStart?.Invoke();
        }
    }
}