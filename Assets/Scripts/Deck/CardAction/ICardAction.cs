
using System.Collections;
using UnityEngine;

public interface ICardAction
{
    bool Execute(object caster, CardData card, Vector2Int targetPosition, out IEnumerator routine);
}
