using UnityEngine;

public class CardSO : ScriptableObject
{
    public string cardName;
    public string description;
    public Rarity rarity;
    [HideInInspector] public CardType cardType;
    public int damage = 1;
    /// <summary>
    /// 시전 횟수
    /// </summary>
    public int attackCnt = 1;
    public int cost = 0;
    public Sprite image;
    
    
    public bool CheckCanSlimeMove(Vector2Int startPosition, Vector2Int targetPosition, int maxDistance)
    {
        int deltaX = targetPosition.x - startPosition.x;
        int deltaY = targetPosition.y - startPosition.y;
        if ((deltaX == deltaY || deltaX == -deltaY || deltaX * deltaY == 0) && Mathf.Abs(deltaX) <= maxDistance && Mathf.Abs(deltaY) <= maxDistance) return true;
            return true;
        return false;
    }
}
