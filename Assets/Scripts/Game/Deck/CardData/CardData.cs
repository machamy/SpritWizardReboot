using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[Serializable]
    public class CardData : ICloneable
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
        
        public BaseCardAction cardAction;

        /// <summary>
        /// 위치 정보가 없어도 시전이 가능한지 확인
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 해당 위치로 시전이 가능한지 여부 확인
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CanCastTo(Vector2Int start, Vector2Int target, int additionalMoveCnt = 0)
        {
        int moveCnt = move + additionalMoveCnt;
            int deltaX = Mathf.Abs(start.x - target.x);
            int deltaY = Mathf.Abs(start.y - target.y);
            // Debug.Log($"deltaX : {deltaX}, deltaY : {deltaY}");
            if ((deltaX == deltaY || deltaX * deltaY == 0) &&
                Mathf.Abs(deltaX) <= moveCnt && Mathf.Abs(deltaY) <= moveCnt)
                return true;
            return false;
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }
  
    }
