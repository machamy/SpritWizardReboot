using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _hp;
    [SerializeField]
    private List<EnemyPattern> rangePatternList;
    [SerializeField]
    private List<EnemyPattern> meleePatternList;
}