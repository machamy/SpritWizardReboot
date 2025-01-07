
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;


public abstract class BaseCardAction : ScriptableObject
{
    /// <summary>
    /// 카드 시전 루틴을 받아옴
    /// </summary>
    /// <param name="caster">시전자</param>
    /// <param name="cardData">카드정보</param>
    /// <param name="targetPosition">목표점</param>
    /// <param name="routine">카드 시전 루틴.</param>
    /// <returns></returns>
    public abstract bool Execute([CanBeNull] object caster, CardData cardData, Vector2Int targetPosition,
        out IEnumerator routine, RuneEffectHolder runeEffectHolder = null);
    
}
