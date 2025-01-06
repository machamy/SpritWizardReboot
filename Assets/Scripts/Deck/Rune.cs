using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private int damage = 0;
    [SerializeField] private int attackCnt = 0;

    public void AddRuneEffect(CalculateType damageCalculateType, int damage, CalculateType attackCntCalculateType, int attackCnt)
    {
        if (damageCalculateType == CalculateType.add) this.damage += damage;
        else if (damageCalculateType == CalculateType.sub) this.damage -= damage;
        else if (damageCalculateType == CalculateType.mul) this.damage *= damage;
        else Debug.Log("연산타입오류!");
        
        if (attackCntCalculateType == CalculateType.add) this.attackCnt += attackCnt;
        else if (attackCntCalculateType == CalculateType.sub) this.attackCnt -= attackCnt;
        else if (attackCntCalculateType == CalculateType.mul) this.attackCnt *= attackCnt;
        else Debug.Log("연산타입오류!");
    }

    public Dictionary<RuneEffect, int> GetRuneEffect()
    {
        Dictionary<RuneEffect, int> effect = new Dictionary<RuneEffect, int>();
        effect[RuneEffect.damage] = damage;
        effect[RuneEffect.attackCnt] = attackCnt;

        // 룬 초기화
        damage = 0;
        attackCnt = 0;

        return effect;
    }
}
