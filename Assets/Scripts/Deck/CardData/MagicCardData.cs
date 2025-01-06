using System;
using UnityEngine;
using UnityEngine.Events;

namespace DataBase.DataClasses
{
    [Serializable]
    public class MagicCardData : CardData
    {
        /// <summary></summary>
        public SkillCaster skillCaster;

        /// <summary></summary>
        public int attackDamage;
        /// <summary></summary>
        public int attackCount;
        /// <summary></summary>
        public AttackType attackType;
        /// <summary></summary>
        public int attackHeight;
        /// <summary></summary>
        public int attackWidth;
        /// <summary></summary>
        public AttackSpread attackSpread;
        /// <summary></summary>
        public int spreadRange;
        /// <summary></summary>
        public int pierce;
        /// <summary></summary>
        public int move;
        /// <summary>특수효과</summary>
        public string specialEffectId;
        
        public void ParseRawData(RawMagicCard rawMagicCard)
        {
            cardKoreanName = rawMagicCard.skillKoreanName;
            cardId = rawMagicCard.id;
            cardName = rawMagicCard.name;
            description = rawMagicCard.describe;
            skillCaster = rawMagicCard.skillCaster.ToLowerInvariant() switch
            {
                "grassslime" => SkillCaster.Grass,
                "iceslime" => SkillCaster.Ice,
                "fireslime" => SkillCaster.Fire,
                _ => SkillCaster.Ice
            };
            rarity = rawMagicCard.rarity.ToLowerInvariant() switch
            {
                "common" => Rarity.common,
                "rare" => Rarity.rare,
                _ => Rarity.common
            };
            attackDamage = rawMagicCard.attackDamage;
            attackCount = rawMagicCard.attackCount;
            attackType = rawMagicCard.attackType.ToLowerInvariant() switch
            {
                "beam" => AttackType.beam,
                "projectile" => AttackType.projectile,
                "explosion" => AttackType.explosion,
                _ => AttackType.projectile
            };
            attackHeight = rawMagicCard.attackHeight;
            attackWidth = rawMagicCard.attackWidth;
            attackSpread = rawMagicCard.attackSpread.ToLowerInvariant() switch
            {
                "radial" => AttackSpread.radial,
                "focused" => AttackSpread.focused,
                _ => AttackSpread.radial
            };
            spreadRange = rawMagicCard.spreadRange;
            pierce = rawMagicCard.pierce;
            move = rawMagicCard.move;
            cost = rawMagicCard.cost;
            specialEffectId = rawMagicCard.specialEffectId;
        }
    }
}