using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class RuneEffectHolder : MonoBehaviour
{
    [SerializeField] private IntVariableSO[] runeEffectChannel;
    [SerializeField] private int[] runeEffectData = new int[(int)Define.RuneEffectType.MAX];

    
    /// <summary>
    /// 해당 룬의 값을 가져온다
    /// </summary>
    /// <param name="runeEffectType"></param>
    /// <returns></returns>
    public int GetRuneEffect(Define.RuneEffectType runeEffectType)
    {
        return runeEffectData[(int)runeEffectType];
    }
    /// <summary>
    /// 해당 룬을 그 값으로 설정
    /// </summary>
    /// <param name="runeEffectType"></param>
    /// <param name="amount"></param>
    public void SetRuneEffect(Define.RuneEffectType runeEffectType, int amount)
    {
        runeEffectData[(int)runeEffectType] = amount;
        runeEffectChannel[(int)runeEffectType].Value = amount;
    }
    /// <summary>
    /// 해당 룬을 0으로 초기화
    /// </summary>
    /// <param name="runeEffectType"></param>
    public void ResetRuneEffect(Define.RuneEffectType runeEffectType)
    {
        runeEffectData[(int)runeEffectType] = 0;
        runeEffectChannel[(int)runeEffectType].Value = 0;
    }
    /// <summary>
    /// 해당 룬을 빼내고, 0으로 초기화
    /// </summary>
    /// <param name="runeEffectType"></param>
    /// <returns></returns>
    public int PopRuneEffect(Define.RuneEffectType runeEffectType)
    {
        int amount = runeEffectData[(int)runeEffectType];
        runeEffectData[(int)runeEffectType] = 0;
        runeEffectChannel[(int)runeEffectType].Value = 0;
        return amount;
    }
    
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
}
