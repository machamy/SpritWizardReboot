using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "EnemyPatternFactorySO", menuName = "EnemyFactory/EnemyPatternFactorySO")]
public class EnemyPatternFactorySO : ScriptableObject
{
    public EnemyPatternData Create(RawMonsterPattern rawEnemyPattern)
    {
        string[] split_pattern;
        split_pattern = rawEnemyPattern.actionSequence.Split(", ");
        var enemyPattern = new EnemyPatternData
        {
            id = rawEnemyPattern.id,
            range = rawEnemyPattern.range,
            phase = rawEnemyPattern.phase,
            weight = rawEnemyPattern.weight,
            actionSequence = split_pattern
        };
        return enemyPattern;
    }
    
}
