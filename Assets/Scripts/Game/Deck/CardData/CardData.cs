using System;
using System.Collections.Generic;
using Game;
using Game.Player;
using Game.World;
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
    /// move가 0이면 true
    /// Rune여부는 확인하지 않는다
    /// </summary>
    /// <returns></returns>
    public bool CanCastWithoutTarget()
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
    public bool CanCastTo(Vector2Int start, Vector2Int target, RuneEffectHolder runeEffectHolder)
    {
        int moveCnt = move + runeEffectHolder.GetRuneEffect(Define.RuneEffectType.MoveCnt);
        int deltaX = Mathf.Abs(start.x - target.x);
        int deltaY = Mathf.Abs(start.y - target.y);
        // Debug.Log($"deltaX : {deltaX}, deltaY : {deltaY}");
        if ((deltaX == deltaY || deltaX * deltaY == 0) &&
            Mathf.Abs(deltaX) <= moveCnt && Mathf.Abs(deltaY) <= moveCnt)
            return true;
        return false;
    }
    

    public List<Tile> GetTargetTiles(Vector2Int castPosition, RuneEffectHolder runeEffectHolder)
    {
        Board board = BattleManager.Instance.Board;
        int atackCnt = attackCount + runeEffectHolder.GetRuneEffect(Define.RuneEffectType.AttackCnt);
        int attackWidth = this.attackWidth;
        int attackHeight = this.attackHeight;
        List<Tile> targetTiles;
        if (attackType == AttackType.explosion)
        {
            targetTiles = board.GetTilesSquareAbs(castPosition, attackWidth, attackHeight);
            return targetTiles;
        }
        if(attackType == AttackType.projectile)
        {
            attackWidth = 9999;
            attackHeight = 1;
        }
        Direction dir = Direction.R;
        targetTiles = board.GetTilesBeam(castPosition, dir, attackWidth, attackHeight);
        if (attackSpread == AttackSpread.radial)
        {
            while (--atackCnt > 0)
            {
                //TODO List가 아닌 반복자를 받아오는게 나은듯?
                dir = (Direction) ((int)(dir + 1) % spreadRange);
                targetTiles.AddRange(board.GetTilesBeam(castPosition, dir, attackWidth, attackHeight));
            }
        }
        
        
        
        return targetTiles;
    }
    
    public Slime GetSkillCaster()
    {
        return BattleManager.Instance.GetSlime(skillCaster);
    }
    
    public object Clone()
    {
        return MemberwiseClone();
    }

}
