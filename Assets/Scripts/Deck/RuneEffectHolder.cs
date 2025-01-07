using System.Collections.Generic;
using UnityEngine;

public class RuneEffectHolder : MonoBehaviour
{
    public enum RuneEffectType
    {
        damage,
        attackCnt,
        moveCnt,
        MAX
    }
    
    [SerializeField] private int[] runeEffect = new int[(int)RuneEffectType.MAX];
    
    [SerializeField] private int damage = 0;
    [SerializeField] private int attackCnt = 0;
    public void StackRuneEffect(RuneEffectType runeEffectType, CalculateType calculateType, int amount)
    {
        if (calculateType == CalculateType.add) runeEffect[(int)runeEffectType] += amount;
        else if (calculateType == CalculateType.sub) runeEffect[(int)runeEffectType] -= amount;
        else if (calculateType == CalculateType.mul) runeEffect[(int)runeEffectType] *= amount;
        else Debug.Log("연산타입오류!");
    }
    public void AddRuneEffect(CalculateType damageCalculateType, int damage, CalculateType attackCntCalculateType, int attackCnt)
    {
        StackRuneEffect(RuneEffectType.damage, damageCalculateType, damage);
        StackRuneEffect(RuneEffectType.attackCnt, attackCntCalculateType, attackCnt);
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
