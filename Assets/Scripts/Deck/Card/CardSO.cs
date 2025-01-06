using System;
using UnityEngine;

public class CardSO : ScriptableObject
{
    public CardData cardData;


    private void OnValidate()
    {
        name = cardData.cardName;
    }
}
