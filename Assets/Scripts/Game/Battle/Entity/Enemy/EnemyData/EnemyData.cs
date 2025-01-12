using System;
using UnityEngine;

[Serializable]
public class EnemyData
{
    [Header("EnemyData")]
    public string name;
    public int id;
    public string sprite;
    public int size;
    public int hpMiddle;
    public int hpVariation;
    public string[] pattern;
    public string initPattern;
    public int phaseSwitchHpRatio;
}
