
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CardMetaData
{
    protected bool Equals(CardMetaData other)
    {
        return cardId == other.cardId && cardKoreanName == other.cardKoreanName && cardName == other.cardName && description == other.description && cardType == other.cardType && rarity == other.rarity && cost == other.cost && Equals(frontImage, other.frontImage) && Equals(backImage, other.backImage) && Equals(cardData, other.cardData);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((CardMetaData)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(cardId);
        hashCode.Add(cardKoreanName);
        hashCode.Add(cardName);
        hashCode.Add(description);
        hashCode.Add((int)cardType);
        hashCode.Add((int)rarity);
        hashCode.Add(cost);
        hashCode.Add(frontImage);
        hashCode.Add(backImage);
        hashCode.Add(cardData);
        return hashCode.ToHashCode();
    }

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

    public bool isSmithed = false;

    public Sprite frontImage;
    public Sprite backImage;
    [Header("CardData")]
    public CardData cardData;
    
    
    /// <summary>
    /// 널체크 오버로딩. id가 같으면 같은 카드로 취급한다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
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