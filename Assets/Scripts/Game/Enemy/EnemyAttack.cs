using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "Enemy/EnemyAttack")]
public class EnemyAttack : EnemyBehavior
{
    [SerializeField]
    int _attackMoveDistance;
    [SerializeField]
    int _attackDamage;
}
