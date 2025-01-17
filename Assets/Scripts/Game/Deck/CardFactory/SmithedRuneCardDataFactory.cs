using UnityEngine;

[CreateAssetMenu(fileName = "SmithedRuneCardDataFactory", menuName = "CardFactory/SmithedRuneCardDataFactory")]
public class SmithedRuneCardDataFactory : RuneCardDataFactorySO
{
    public CardMetaData Create(RawSmithedRuneCard rawSmithedCardData)
    {
        var runeCardData = new CardMetaData
        {
            cardId = rawSmithedCardData.id,
            cardKoreanName = rawSmithedCardData.name,
            cardName = rawSmithedCardData.name,
            description = rawSmithedCardData.describe,
            cardType = CardType.Rune,
            rarity = Rarity.common,
            cost = rawSmithedCardData.cost,
            isSmithed = true,
            backImage = backImage,
        };
        runeCardData.cardData = new CardData
        {
            move = 0,
            calculateType = rawSmithedCardData.calcType.ToLowerInvariant() == "add" ? Define.CalculateType.add : Define.CalculateType.mul,
            runeEffectAmounts = new[]
            {
            rawSmithedCardData.attack,
            rawSmithedCardData.projectile,
            rawSmithedCardData.movement
            }
        };
        return runeCardData;
    }
}
