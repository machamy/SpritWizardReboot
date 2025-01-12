using System;
using System.Collections.Generic;

[Serializable]
public class EnemyPatternData
{
    public string id;
    public int range;
    public string[] actionSequence;
    public int phase;
    public int weight;
}
