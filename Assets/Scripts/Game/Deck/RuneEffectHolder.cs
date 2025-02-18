using System;
using System.Collections.Generic;
using DefaultNamespace;
using EventChannel;
using UnityEngine;
using UnityEngine.Serialization;

public class RuneEffectHolder : MonoBehaviour
{
    // [SerializeField] private IntVariableSO[] runeEffectChannel;
    [SerializeField] private int[] tmpRuneEffectData = new int[(int)Define.RuneEffectType.MAX];
    [SerializeField] private int[] runeEffectData = new int[(int)Define.RuneEffectType.MAX];
    [SerializeField] GUIEventChannelSO guiEventChannel;
    
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
        guiEventChannel.RaiseUpdateRune(runeEffectType, amount);
        // runeEffectChannel[(int)runeEffectType].Value = amount;
    }
    /// <summary>
    /// 해당 룬을 0으로 초기화
    /// </summary>
    /// <param name="runeEffectType"></param>
    public void ResetRuneEffect(Define.RuneEffectType runeEffectType)
    {
        runeEffectData[(int)runeEffectType] = 0;
        guiEventChannel.RaiseUpdateRune(runeEffectType, 0);
        // runeEffectChannel[(int)runeEffectType].Value = 0;
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
        guiEventChannel.RaiseUpdateRune(runeEffectType, 0);
        // runeEffectChannel[(int)runeEffectType].Value = 0;
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
                // runeEffectChannel[(int)runeEffectType].Value = runeEffectData[(int)runeEffectType];
                guiEventChannel.RaiseUpdateRune(runeEffectType, runeEffectData[(int)runeEffectType]);
                break;
            // case Define.CalculateType.sub:
            //     runeEffectData[(int)runeEffectType] -= amount;
            //     break;
            case Define.CalculateType.mul:
                runeEffectData[(int)runeEffectType] *= amount;
                // runeEffectChannel[(int)runeEffectType].Value = runeEffectData[(int)runeEffectType];
                guiEventChannel.RaiseUpdateRune(runeEffectType, runeEffectData[(int)runeEffectType]);
                break;
        }
    }
    
    public void ResetTempRuneEffect(Define.RuneEffectType runeEffectType)
    {
        tmpRuneEffectData[(int)runeEffectType] = runeEffectData[(int)runeEffectType];
        guiEventChannel.RaiseUpdateTempRune(runeEffectType, tmpRuneEffectData[(int)runeEffectType]);
    }
    
    public void ResetAllTempRuneEffect()
    {
        print("ResetAllTempRuneEffect");
        for (int i = 0; i < (int)Define.RuneEffectType.MAX; i++)
        {
            ResetTempRuneEffect((Define.RuneEffectType)i);
        }
    }
    
    public void StackTempRuneEffectRange(int[] runeEffectAmounts)
    {
        for (int i = 0; i < (int)Define.RuneEffectType.MAX; i++)
        {
            StackTempRuneEffect((Define.RuneEffectType)i, Define.CalculateType.add, runeEffectAmounts[i]);
        }
    }
    
    public void StackTempRuneEffect(Define.RuneEffectType runeEffectType, Define.CalculateType calculateType, int amount)
    {
        switch (calculateType)
        {
            case Define.CalculateType.add:
                tmpRuneEffectData[(int)runeEffectType] += amount;
                guiEventChannel.RaiseUpdateTempRune(runeEffectType, tmpRuneEffectData[(int)runeEffectType]);
                break;
            case Define.CalculateType.mul:
                tmpRuneEffectData[(int)runeEffectType] *= amount;
                guiEventChannel.RaiseUpdateTempRune(runeEffectType, tmpRuneEffectData[(int)runeEffectType]);
                break;
        }
    }
}
