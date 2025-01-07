using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyPattern", menuName = "Enemy/EnemyPattern")]
public class EnemyPatternSO : ScriptableObject
{
    public int id;
    public int range;
    public List<EnemyBehaviourSO> actionSequence;
    public int phase;
    public int weight;
}
