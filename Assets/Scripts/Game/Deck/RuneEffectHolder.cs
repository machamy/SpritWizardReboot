using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class RuneEffectHolder : MonoBehaviour
{
    [SerializeField] private IntVariableSO[] runeEffectChannel;
    [SerializeField] private int[] runeEffectData = new int[(int)Define.RuneEffectType.MAX];

    public void StackRuneEffectRange(int[] runeEffectAmounts)
    {
        for (int i = 0; i < (int)Define.RuneEffectType.MAX; i++)
        {
            StackRuneEffect((Define.RuneEffectType)i, Define.CalculateType.add, runeEffectAmounts[i]);
        }
    }
    public void StackRuneEffect(Define.RuneEffectType runeEffectType, Define.CalculateType calculateType, int amount)
    {
        switch (calculateType)
        {
            case Define.CalculateType.add:
                runeEffectData[(int)runeEffectType] += amount;
                runeEffectChannel[(int)runeEffectType].Value = runeEffectData[(int)runeEffectType];
                break;
            // case Define.CalculateType.sub:
            //     runeEffectData[(int)runeEffectType] -= amount;
            //     break;
            case Define.CalculateType.mul:
                runeEffectData[(int)runeEffectType] *= amount;
                runeEffectChannel[(int)runeEffectType].Value = runeEffectData[(int)runeEffectType];
                break;
        }
    }
    public void AddRuneEffect(Define.CalculateType damageCalculateType, int damage, Define.CalculateType attackCntCalculateType, int attackCnt)
    {
        StackRuneEffect(Define.RuneEffectType.damage, damageCalculateType, damage);
        StackRuneEffect(Define.RuneEffectType.attackCnt, attackCntCalculateType, attackCnt);
    }
}
