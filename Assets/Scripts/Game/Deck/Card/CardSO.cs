using System;
using UnityEngine;
using UnityEngine.Serialization;

[Obsolete("Use CardMetaData instead")]
[CreateAssetMenu(fileName = "New Card", menuName = "CardSO")]
public class CardSO : ScriptableObject
{
    [FormerlySerializedAs("cardData")] [SerializeField] public CardMetaData cardMetaData;
    
}
