using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyPattern", menuName = "Enemy/EnemyPattern")]
public class EnemyPattern : ScriptableObject
{
    [SerializeField]
    private List<EnemyBehavior> _enemyBehaviors;
}
