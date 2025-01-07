
using System.Collections;
using UnityEngine;

public interface ICardAction
{
    bool Execute(object caster, CardMetaData cardMeta, Vector2Int targetPosition, out IEnumerator routine);
}
