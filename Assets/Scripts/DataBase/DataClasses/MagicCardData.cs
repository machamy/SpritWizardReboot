using System;

namespace DataBase.DataClasses
{
    [Serializable]
    public class MagicCardData
    {
        /// <summary>스킬 이름</summary>
        public string skillKoreanName;
        /// <summary></summary>
        public int id;
        /// <summary></summary>
        public string name;
        /// <summary></summary>
        public string describe;
        /// <summary></summary>
        public SkillCaster skillCaster;
        /// <summary></summary>
        public Rarity rarity;
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
        public string attackSpread;
        /// <summary></summary>
        public int spreadRange;
        /// <summary></summary>
        public int pierce;
        /// <summary></summary>
        public int move;
        /// <summary></summary>
        public int cost;
        /// <summary>특수효과</summary>
        public string specialEffectId;
        
        public void ParseRawData(RawMagicCard rawMagicCard)
        {
            skillKoreanName = rawMagicCard.skillKoreanName;
            id = rawMagicCard.id;
            name = rawMagicCard.name;
            describe = rawMagicCard.describe;
            skillCaster = rawMagicCard.skillCaster.ToLowerInvariant() switch
            {
                "grassslime" => SkillCaster.Grass,
                "iceslime" => SkillCaster.Ice,
                "fireslime" => SkillCaster.Fire,
                _ => SkillCaster.Ice
            };
            rarity = rawMagicCard.rarity.ToLowerInvariant() switch
            {
                "common" => Rarity.normal,
                "rare" => Rarity.rare,
                _ => Rarity.normal
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
            attackSpread = rawMagicCard.attackSpread;
            spreadRange = rawMagicCard.spreadRange;
            pierce = rawMagicCard.pierce;
            move = rawMagicCard.move;
            cost = rawMagicCard.cost;
            specialEffectId = rawMagicCard.specialEffectId;
        }
    }
}