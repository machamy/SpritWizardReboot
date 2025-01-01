using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedSO", menuName = "Scriptable Objects/SeedSO")]
public class SeedSO : ScriptableObject
{
    public List<List<SeedType>> weekSeed = new List<List<SeedType>>();
}
