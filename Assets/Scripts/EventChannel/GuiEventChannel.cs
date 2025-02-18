using UnityEngine;

namespace EventChannel
{
    [CreateAssetMenu(fileName = "GUIEventChannel", menuName = "EventChannel/GUIEventChannel")]
    public class GUIEventChannelSO : ScriptableObject
    {
        public delegate void UpdateRuneDelegate(Define.RuneEffectType runeType, int value);
        
        // 기본 룬 값 업데이트 이벤트 (예: 시전 후 최종 값)
        public event UpdateRuneDelegate OnUpdateRune;
        // 임시 룬 값 업데이트 이벤트 (예: 호버 시 예상 값)
        public event UpdateRuneDelegate OnUpdateTempRune;
        
        public void RaiseUpdateRune(Define.RuneEffectType runeType, int value)
        {
            // Debug.Log($"RaiseUpdateRune : {runeType} : {value}");
            OnUpdateRune?.Invoke(runeType, value);
        }
        
        public void RaiseUpdateTempRune(Define.RuneEffectType runeType, int value)
        {
            // Debug.Log($"RaiseUpdateRune : {runeType} : {value}");
            OnUpdateTempRune?.Invoke(runeType, value);
        }
    }
}