using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    public List<CardMetaData> CardList;
    // public List<Relic> relicList;
}
