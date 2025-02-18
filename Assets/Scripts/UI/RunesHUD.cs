using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class RunesHUDItem
{
    public Define.RuneEffectType runeType;
    public TextMeshProUGUI runeText;
}

public class RunesHUD : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color tempColor  = Color.red;
    
    [Header("UI References")]
    [SerializeField] private List<RunesHUDItem> runeItems;


    private Dictionary<Define.RuneEffectType, TextMeshProUGUI> runeTextDict = new Dictionary<Define.RuneEffectType, TextMeshProUGUI>();


    private Dictionary<Define.RuneEffectType, int> baseRuneValues = new Dictionary<Define.RuneEffectType, int>();
    private Dictionary<Define.RuneEffectType, int> tempRuneValues = new Dictionary<Define.RuneEffectType, int>();

    public Color DefaultColor => defaultColor;
    
    public bool isDirty = false;
    private void Awake()
    {
        foreach (var item in runeItems)
        {
            runeTextDict[item.runeType] = item.runeText;
            // 초기값 설정 (예시로 0)
            baseRuneValues[item.runeType] = 0;
            tempRuneValues[item.runeType] = 0;
        }
        UpdateRunesHUD();
    }


    private void Update()
    {
        // 디버그 출력
        // foreach (var runeType in runeTextDict.Keys)
        // {
        //     print($"{runeType} : {tempRuneValues[runeType]}");
        //     print($"{runeType} : {baseRuneValues[runeType]}");
        // }
        if (isDirty)
        {
            UpdateRunesHUD();
            isDirty = false;
        }
    }

    /// <summary>
    /// 룬 UI의 텍스트 색상을 변경
    /// </summary>
    public void SetRuneTextColor(Define.RuneEffectType runeType, Color color, bool animate = false)
    {
        if (runeTextDict.TryGetValue(runeType, out var textMesh))
        {
            if (animate)
            {
                textMesh.DOColor(color, 0.2f);
            }
            else
            {
                textMesh.color = color;
            }
        }
    }

    /// <summary>
    /// 기본 수치를 업데이트
    /// </summary>
    public void UpdateRune(Define.RuneEffectType runeType, int baseValue)
    {
        baseRuneValues[runeType] = baseValue;
        isDirty = true;
    }
    
    /// <summary>
    /// 임시 수치를 업데이트
    /// </summary>
    public void UpdateTempRune(Define.RuneEffectType runeType, int tempValue)
    {
        tempRuneValues[runeType] = tempValue;
        isDirty = true;
    }

    /// <summary>
    /// 임시 수치를 기본 수치로
    /// </summary>
    public void ResetTempRune(Define.RuneEffectType runeType)
    {
        tempRuneValues[runeType] = baseRuneValues[runeType];
        isDirty = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateRuneText(Define.RuneEffectType runeType)
    {
        if (runeTextDict.TryGetValue(runeType, out var textMesh))
        {
            int displayValue = tempRuneValues[runeType];    
            textMesh.text = displayValue.ToString();
        }
    }

    /// <summary>
    /// 전체 룬 UI를 업데이트합니다.
    /// </summary>
    public void UpdateRunesHUD()
    {
        foreach (var runeType in runeTextDict.Keys)
        {
            UpdateRuneText(runeType);
            // 임시 수치가 있으면 임시 색상, 아니면 기본 색상
            var color = (tempRuneValues[runeType] != baseRuneValues[runeType]) ? tempColor : defaultColor;
            // print($"tmp: {tempRuneValues[runeType]}, base: {baseRuneValues[runeType]}");
            SetRuneTextColor(runeType, color);
        }
    }
}
