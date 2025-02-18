
using System;
using System.Collections.Generic;
using DataStructure;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicCardFactory", menuName = "CardFactory/MagicCardFactory")]
public class MagicCardDataFactorySO : ScriptableObject, CardDataFactory<CardMetaData>
{
    public Sprite grassCardBack;
    public Sprite iceCardBack;
    public Sprite fireCardBack;
    public BaseCardAction defaultMagicCardAction;
    public SerializableDict<string, BaseCardAction> magicCardActionDict;

    public CardMetaData Create(RawMagicCard rawMagicCard)
    {
        var meta = new CardMetaData
        {
            cardId = rawMagicCard.id,
            cardKoreanName = rawMagicCard.name,
            cardName = rawMagicCard.name,
            description = rawMagicCard.describe,
            cardType = CardType.Attack,
            rarity = rawMagicCard.rarity.ToLowerInvariant() switch
            {
                "common" => Rarity.common,
                "rare" => Rarity.rare,
                _ => Rarity.common
            },
            cost = rawMagicCard.cost,
            frontImage = null,
            backImage = rawMagicCard.skillCaster.ToLowerInvariant() switch
            {
                "grassslime" => grassCardBack,
                "iceslime" => iceCardBack,
                "fireslime" => fireCardBack,
                _ => grassCardBack
            },
        };
        
        var magicCard = new CardData
        {
            skillCaster = rawMagicCard.skillCaster.ToLowerInvariant() switch
            {
                "grassslime" => SkillCaster.Grass,
                "iceslime" => SkillCaster.Ice,
                "fireslime" => SkillCaster.Fire,
                _ => SkillCaster.Ice
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
            specialEffectId = rawMagicCard.specialEffectId,
            
            cardAction = defaultMagicCardAction
        };
        if(magicCard.attackType == AttackType.projectile)
            magicCard.runeEffectTypes = new[] {Define.RuneEffectType.Damage, Define.RuneEffectType.AttackCnt, Define.RuneEffectType.MoveCnt};
        else
            magicCard.runeEffectTypes = new[] {Define.RuneEffectType.Damage, Define.RuneEffectType.MoveCnt};
        
        meta.cardData = magicCard;
        return meta;
    }

    // /// <summary>
    // /// 이미지 추가 +
    // /// 행동이 없으면, 기본행동 추가.
    // /// </summary>
    // /// <param name="cardDataData"></param>
    // /// <returns></returns>
    // public CardData UpdateCardData(CardData cardData)
    // {
    //     cardData = cardData.Clone() as CardData;
    //     if (cardData != null)
    //     {
    //         cardData.backImage = cardData.skillCaster switch
    //         {
    //             SkillCaster.Grass => grassCardBack,
    //             SkillCaster.Ice => iceCardBack,
    //             SkillCaster.Fire => fireCardBack,
    //             _ => iceCardBack
    //         };
    //         if (cardData.cardAction == null)
    //         {
    //             cardData.cardAction = defaultMagicCardAction;
    //         }
    //
    //         return cardData;
    //     }
    //     Debug.LogError("CardData is null");
    //     return null;
    // }
}
