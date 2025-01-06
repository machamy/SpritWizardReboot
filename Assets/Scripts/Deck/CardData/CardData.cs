
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public abstract class CardData
{
    /// <summary></summary>
    public int cardId;
    public string cardKoreanName;
    public string cardName;
    public string description;
    public CardType cardType;
    /// <summary></summary>
    public Rarity rarity;
    /// <summary></summary>
    public int cost;
    
    public Sprite frontImage;
    public Sprite backImage;

}