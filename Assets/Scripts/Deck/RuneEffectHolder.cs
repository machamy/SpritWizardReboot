using System.Collections.Generic;
using UnityEngine;

public class RuneEffectHolder : MonoBehaviour
{

    [SerializeField] private int[] runeEffect = new int[(int)Define.RuneEffectType.MAX];
    
    [SerializeField] private int damage = 0;
    [SerializeField] private int attackCnt = 0;
    public void StackRuneEffect(Define.RuneEffectType runeEffectType, Define.CalculateType calculateType, int amount)
    {
        switch (calculateType)
        {
            case Define.CalculateType.add:
                runeEffect[(int)runeEffectType] += amount;
                break;
            case Define.CalculateType.sub:
                runeEffect[(int)runeEffectType] -= amount;
                break;
            case Define.CalculateType.mul:
                runeEffect[(int)runeEffectType] *= amount;
                break;
        }
    }
    public void AddRuneEffect(Define.CalculateType damageCalculateType, int damage, Define.CalculateType attackCntCalculateType, int attackCnt)
    {
        StackRuneEffect(Define.RuneEffectType.damage, damageCalculateType, damage);
        StackRuneEffect(Define.RuneEffectType.attackCnt, attackCntCalculateType, attackCnt);
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
