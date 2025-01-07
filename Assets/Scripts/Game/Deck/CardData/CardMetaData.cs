
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CardMetaData
{
    [Header("CardMetaData")]
    /// <summary></summary>
    public string cardId;

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
    [Header("CardData")]
    public CardData cardData;
    //널체크 오버로딩
    public static bool operator ==(CardMetaData a, CardMetaData b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        {
            return true;
        }
        if (ReferenceEquals(b, null))
        {
            if(String.IsNullOrWhiteSpace(a.cardId))
            {
                return true;
            }
            return false;
        }
        if (ReferenceEquals(a, null))
        {
            if(String.IsNullOrWhiteSpace(b.cardId))
            {
                return true;
            }
            return false;
        }
        

        return a.cardId == b.cardId;
    }

    public static bool operator !=(CardMetaData a, CardMetaData b)
    {
        return !(a == b);
    }
}