using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName;
    public int enemyId;
    public string spriteName;
    public int size;
    public int hpMiddle;
    public int hpVariation;
    public List<EnemyPatternSO> patternList;
    public EnemyPatternSO initPattern;
    public int phaseSwitchHpRatio;
}