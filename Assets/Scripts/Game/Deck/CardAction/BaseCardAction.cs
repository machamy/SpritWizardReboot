
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;


public abstract class BaseCardAction : ScriptableObject
{
    public abstract bool Execute([CanBeNull] object caster, CardData card, Vector2Int targetPosition,
        out IEnumerator routine);
}
