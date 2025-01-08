using DefaultNamespace;
using UnityEngine;


/// <summary>
/// 플레이어의 턴이 끝나는 것을 기다리는 오브젝트에 붙인다.
/// 비활성화 될 시 waitsForEndPlayerTurn이 1 감소한다.
/// </summary>
public class PlayerTurnHolder : MonoBehaviour
{
    [SerializeField] private IntVariableSO waitsForEndPlayerTurn;
    
    private void OnEnable()
    {
        waitsForEndPlayerTurn.Value += 1;
    }

    private void OnDisable()
    {
        waitsForEndPlayerTurn.Value -= 1;
    }
}
