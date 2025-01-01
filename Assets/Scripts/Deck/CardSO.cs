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
}
