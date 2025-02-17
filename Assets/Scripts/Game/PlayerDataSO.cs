using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    public IntVariableSO gold;
    public FloatVariableSO gateHp;
    public IntVariableSO clearCount;
    
    public List<CardMetaData> CardList;
    // public List<Relic> relicList;
}
