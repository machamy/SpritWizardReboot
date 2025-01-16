using UnityEngine;

namespace EventChannel
{
    [CreateAssetMenu(menuName = "EventChannel/PhaseEventChannel")]
    public class PhaseEventChannelSO : ScriptableObject
    {
        public delegate void PhaseEvent(SeedType seed);
        public event PhaseEvent OnPhaseEventRaised;

        public void RaisePhaseEvent(SeedType seed)
        {
            OnPhaseEventRaised?.Invoke(seed);
        }
    }
}
