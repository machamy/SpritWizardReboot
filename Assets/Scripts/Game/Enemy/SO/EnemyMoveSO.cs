using UnityEngine;
using Game.Entity;

[CreateAssetMenu(fileName = "EnemyMove", menuName = "Enemy/Behaviour/EnemyMove")]
public class EnemyMoveSO : EnemyBehaviourSO
{
    public override void Execute(Entity entity)
    {
        entity.MoveLeft(value);
    }
}
