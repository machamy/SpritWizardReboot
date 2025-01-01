using Game.Entity;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyAttack", menuName = "Enemy/Behaviour/EnemyAttack")]
public class EnemyAttackSO : EnemyBehaviourSO
{
    public EnemyProjectile enemyProjectile;
    public override void Execute(Entity entity)
    {
        if (action == EnemyBehaviour.MeleeAttack)
        {
            GameManager.Instance.GateHP -= value;
        }
        else if (action == EnemyBehaviour.RangeAttack)
        {
            EnemyProjectile obj = Instantiate(enemyProjectile);
            obj.GetComponent<Entity>().Initialize(Board.Instance, new Vector2Int(entity.Position.x - 1, entity.Position.y));
            obj._dmg = value;
        }
    }
}

