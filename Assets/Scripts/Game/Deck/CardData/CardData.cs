using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[Serializable]
    public class CardData
    {
        [Header("MagicCardData")]
        /// <summary></summary>
        public SkillCaster skillCaster;
        
        /// <summary></summary>
        public int attackDamage;
        /// <summary></summary>
        public int attackCount;
        /// <summary></summary>
        public AttackType attackType;
        public Define.RuneEffectType[] runeEffectTypes;
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
        [Header("RuneCardData")]
        public int[] runeEffectAmounts = new int[(int)Define.RuneEffectType.MAX];
        public Define.CalculateType calculateType;
        
        [FormerlySerializedAs("magicCardAction")] public BaseCardAction cardAction;

        public bool CanCast()
        {
            if(move == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanCastTo(Vector2Int start, Vector2Int target)
        {
            int deltaX = Mathf.Abs(start.x - target.x);
            int deltaY = Mathf.Abs(start.y - target.y);
            Debug.Log($"deltaX : {deltaX}, deltaY : {deltaY}");
            if ((deltaX == deltaY || deltaX * deltaY == 0) &&
                Mathf.Abs(deltaX) <= move && Mathf.Abs(deltaY) <= move)
                return true;
            return false;
        }

        // public object Clone()
        // {
        //     Debug.Log("MagicCardData Clone");
        //     return new MagicCardMetaData
        //     {
        //         skillCaster = skillCaster,
        //         attackDamage = attackDamage,
        //         attackCount = attackCount,
        //         attackType = attackType,
        //         runeEffectTypes = runeEffectTypes,
        //         attackHeight = attackHeight,
        //         attackWidth = attackWidth,
        //         attackSpread = attackSpread,
        //         spreadRange = spreadRange,
        //         pierce = pierce,
        //         move = move,
        //         cost = cost,
        //         specialEffectId = specialEffectId,
        //         backImage = backImage,
        //         cardAction = cardAction
        //     };
        // }
    }
