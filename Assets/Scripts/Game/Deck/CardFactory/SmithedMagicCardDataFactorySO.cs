using UnityEngine;

[CreateAssetMenu(fileName = "SmithedMagicCardFactory", menuName = "CardFactory/SmithedMagicCardFactory")]
public class SmithedMagicCardDataFactorySO : MagicCardDataFactorySO
{
    public CardMetaData Create(RawSmithedMagicCard rawSmithedMagicCard) // TODO => 임시로 기존 카드팩토리 그대로 복사해 왔는데 좀더 나은 방법 생각해보기
    {
        var meta = new CardMetaData
        {
            cardId = rawSmithedMagicCard.id,
            cardKoreanName = rawSmithedMagicCard.name,
            cardName = rawSmithedMagicCard.name,
            description = rawSmithedMagicCard.describe,
            cardType = CardType.Attack,
            rarity = rawSmithedMagicCard.rarity.ToLowerInvariant() switch
            {
                "common" => Rarity.common,
                "rare" => Rarity.rare,
                _ => Rarity.common
            },
            cost = rawSmithedMagicCard.cost,
            isSmithed = true,
            frontImage = null,
            backImage = rawSmithedMagicCard.skillCaster.ToLowerInvariant() switch
            {
                "grassslime" => grassCardBack,
                "iceslime" => iceCardBack,
                "fireslime" => fireCardBack,
                _ => grassCardBack
            },
        };

        var magicCard = new CardData
        {
            skillCaster = rawSmithedMagicCard.skillCaster.ToLowerInvariant() switch
            {
                "grassslime" => SkillCaster.Grass,
                "iceslime" => SkillCaster.Ice,
                "fireslime" => SkillCaster.Fire,
                _ => SkillCaster.Ice
            },
            attackDamage = rawSmithedMagicCard.attackDamage,
            attackCount = rawSmithedMagicCard.attackCount,
            attackType = rawSmithedMagicCard.attackType.ToLowerInvariant() switch
            {
                "beam" => AttackType.beam,
                "projectile" => AttackType.projectile,
                "explosion" => AttackType.explosion,
                _ => AttackType.projectile
            },
            attackHeight = rawSmithedMagicCard.attackHeight,
            attackWidth = rawSmithedMagicCard.attackWidth,
            attackSpread = rawSmithedMagicCard.attackSpread.ToLowerInvariant() switch
            {
                "radial" => AttackSpread.radial,
                "focused" => AttackSpread.focused,
                _ => AttackSpread.radial
            },
            spreadRange = rawSmithedMagicCard.spreadRange,
            pierce = rawSmithedMagicCard.pierce,
            move = rawSmithedMagicCard.move,
            specialEffectId = rawSmithedMagicCard.specialEffectId,

            cardAction = defaultMagicCardAction
        };
        if (magicCard.attackType == AttackType.projectile)
            magicCard.runeEffectTypes = new[] { Define.RuneEffectType.Damage, Define.RuneEffectType.AttackCnt, Define.RuneEffectType.MoveCnt };
        else
            magicCard.runeEffectTypes = new[] { Define.RuneEffectType.Damage, Define.RuneEffectType.MoveCnt };

        meta.cardData = magicCard;
        return meta;
    }
}
