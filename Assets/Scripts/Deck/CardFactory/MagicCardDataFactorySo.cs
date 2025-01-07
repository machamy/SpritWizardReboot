
using DataBase.DataClasses;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicCardFactory", menuName = "CardFactory/MagicCardFactory")]
public class MagicCardDataFactorySO : ScriptableObject, CardDataFactory<MagicCardData>
{
    public Sprite grassCardBack;
    public Sprite iceCardBack;
    public Sprite fireCardBack;
    public BaseCardAction defaultMagicCardAction;

    public MagicCardData CreateMagicCard(RawMagicCard rawMagicCard)
    {
        var magicCard = new MagicCardData
        {
            cardKoreanName = rawMagicCard.skillKoreanName,
            cardId = rawMagicCard.id,
            cardName = rawMagicCard.name,
            description = rawMagicCard.describe,
            skillCaster = rawMagicCard.skillCaster.ToLowerInvariant() switch
            {
                "grassslime" => SkillCaster.Grass,
                "iceslime" => SkillCaster.Ice,
                "fireslime" => SkillCaster.Fire,
                _ => SkillCaster.Ice
            },
            rarity = rawMagicCard.rarity.ToLowerInvariant() switch
            {
                "common" => Rarity.common,
                "rare" => Rarity.rare,
                _ => Rarity.common
            },
            attackDamage = rawMagicCard.attackDamage,
            attackCount = rawMagicCard.attackCount,
            attackType = rawMagicCard.attackType.ToLowerInvariant() switch
            {
                "beam" => AttackType.beam,
                "projectile" => AttackType.projectile,
                "explosion" => AttackType.explosion,
                _ => AttackType.projectile
            },
            attackHeight = rawMagicCard.attackHeight,
            attackWidth = rawMagicCard.attackWidth,
            attackSpread = rawMagicCard.attackSpread.ToLowerInvariant() switch
            {
                "radial" => AttackSpread.radial,
                "focused" => AttackSpread.focused,
                _ => AttackSpread.radial
            },
            spreadRange = rawMagicCard.spreadRange,
            pierce = rawMagicCard.pierce,
            move = rawMagicCard.move,
            cost = rawMagicCard.cost,
            specialEffectId = rawMagicCard.specialEffectId,
            backImage = rawMagicCard.skillCaster.ToLowerInvariant() switch
            {
                "grassslime" => Resources.Load<Sprite>("CardBack/GrassCardBack"),
                "iceslime" => Resources.Load<Sprite>("CardBack/IceCardBack"),
                "fireslime" => Resources.Load<Sprite>("CardBack/FireCardBack"),
                _ => Resources.Load<Sprite>("CardBack/IceCardBack")
            },
            magicCardAction = defaultMagicCardAction
        };
        
        return magicCard;
    }

    /// <summary>
    /// 이미지 추가 +
    /// 행동이 없으면, 기본행동 추가.
    /// </summary>
    /// <param name="cardData"></param>
    /// <returns></returns>
    public MagicCardData UpdateCardData(MagicCardData cardData)
    {
        cardData = cardData.Clone() as MagicCardData;
        if (cardData != null)
        {
            cardData.backImage = cardData.skillCaster switch
            {
                SkillCaster.Grass => grassCardBack,
                SkillCaster.Ice => iceCardBack,
                SkillCaster.Fire => fireCardBack,
                _ => iceCardBack
            };
            if (cardData.magicCardAction == null)
            {
                cardData.magicCardAction = defaultMagicCardAction;
            }

            return cardData;
        }
        Debug.LogError("CardData is null");
        return null;
    }
}
