using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    public IntVariableSO gold;
    public FloatVariableSO gateHp;
    public IntVariableSO clearCount;

    [SerializeField] 
    private Deck deck;
    public Deck Deck => deck;
    
    public void InitDeckByDB()
    {
        deck.SetCardList(new List<CardMetaData>(Database.AllCardMetas));
    }
}
