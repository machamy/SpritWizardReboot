
using UnityEngine;

[CreateAssetMenu(fileName = "RuneCardDataFactory", menuName = "CardFactory/RuneCardDataFactory")]
public class RuneCardDataFactorySO : ScriptableObject
{
    public Sprite backImage;

    
    
   public CardMetaData Create(RawRuneCard rawCardData)
{
    var runeCardData = new CardMetaData
    {
        cardId = rawCardData.id,
        cardKoreanName = rawCardData.name,
        cardName = rawCardData.name,
        description = rawCardData.describe,
        cardType = CardType.Rune,
        rarity = Rarity.common,
        cost = rawCardData.cost,
        backImage = backImage,
    };
    runeCardData.cardData = new CardData
    {
        move = 0,
        calculateType = rawCardData.calcType.ToLowerInvariant() == "add" ? Define.CalculateType.add : Define.CalculateType.mul,
        runeEffectAmounts = new[]
        {
            rawCardData.attack,
            rawCardData.projectile,
            rawCardData.movement
        }
    };
    return runeCardData;
}
   
    // public RuneCardMetaData CreateRuneCard(RawRuneCard rawCardData)
    // {
    //     var runeCardData = new RuneCardMetaData
    //     {
    //         cardId = rawCardData.id,
    //         cardKoreanName = rawCardData.name,
    //         cardName = rawCardData.name,
    //         description = rawCardData.describe,
    //         cardType = CardType.Rune,
    //         rarity = Rarity.common,
    //         cost = rawCardData.cost,
    //         backImage = backImage,
    //     };
    //     runeCardData.calculateType = rawCardData.calcType.ToLowerInvariant() == "add" ? Define.CalculateType.add : Define.CalculateType.mul;
    //    
    //     // damage,
    //     // attackCnt,
    //     // moveCnt,
    //     runeCardData.runeEffectAmounts = new[]
    //     {
    //         rawCardData.attack,
    //         rawCardData.projectile,
    //         rawCardData.movement
    //     };
    //     return runeCardData;
    // }
}
