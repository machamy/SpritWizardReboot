
using UnityEngine;


public abstract class BaseCardAction : ScriptableObject
{
    public abstract void Execute(CardSO card, Vector2Int targetPosition);
}
