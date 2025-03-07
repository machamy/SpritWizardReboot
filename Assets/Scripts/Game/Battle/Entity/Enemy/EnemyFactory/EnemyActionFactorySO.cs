using UnityEngine;

[CreateAssetMenu(fileName = "EnemyActionFactorySO", menuName = "EnemyFactory/EnemyActionFactorySO")]
public class EnemyActionFactorySO : ScriptableObject
{
    public EnemyActionData Create(RawMonsterAction rawEnemyAction)
    {
        var enemyAction = new EnemyActionData
        {
            id = rawEnemyAction.id,
            action = rawEnemyAction.action.ToLowerInvariant() switch
            {
                "move" => EnemyBehaviour.Move,
                "meleeattack" => EnemyBehaviour.MeleeAttack,
                "rangedattack" => EnemyBehaviour.RangeAttack,
                "rest" => EnemyBehaviour.Rest,
                _ => EnemyBehaviour.Rest
            },
            value = rawEnemyAction.value
        };
        return enemyAction;
    }
}
