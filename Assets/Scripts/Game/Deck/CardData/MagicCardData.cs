using System;
using UnityEngine;
using UnityEngine.Events;


    [Serializable]
    public class MagicCardData : CardData, ICloneable
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

        /// <summary>
        /// 임시
        /// </summary>
        public string projectilePrefabName;
        
        public BaseCardAction magicCardAction;
        public object Clone()
        {
            return new MagicCardData
            {
                skillCaster = skillCaster,
                attackDamage = attackDamage,
                attackCount = attackCount,
                attackType = attackType,
                attackHeight = attackHeight,
                attackWidth = attackWidth,
                attackSpread = attackSpread,
                spreadRange = spreadRange,
                pierce = pierce,
                move = move,
                cost = cost,
                specialEffectId = specialEffectId,
                backImage = backImage,
                magicCardAction = magicCardAction
            };
        }
    }
