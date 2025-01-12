using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataFactorySO", menuName = "EnemyFactory/EnemyDataFactorySO")]
public class EnemyDataFactorySO : ScriptableObject
{
    public EnemyData Create(RawMonster rawEnemy)
    {
        string[] splitPattern;
        splitPattern = rawEnemy.pattern.Split(", ");
        var enemy = new EnemyData
        {
            name = rawEnemy.name,
            id = rawEnemy.id,
            sprite = rawEnemy.sprite,
            size = rawEnemy.size,
            hpMiddle = rawEnemy.hpMiddle,
            hpVariation = rawEnemy.hpVariation,
            pattern = splitPattern,
            initPattern = rawEnemy.pattern,
            phaseSwitchHpRatio = rawEnemy.phaseSwitchHpRatio
        };
        return enemy;
    }
}