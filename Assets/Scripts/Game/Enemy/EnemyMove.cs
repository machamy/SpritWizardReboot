using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMove", menuName = "Enemy/EnemyMove")]
public class EnemyMove : EnemyBehavior
{
    [SerializeField]
    private int _moveDistance;
}
