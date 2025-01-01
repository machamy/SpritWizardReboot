using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class CardSO : ScriptableObject
{
    public string cardName;
    public string description;
    public CardType cardType;
    public AttackType attackType;
    public int cost = 1;
    public CalculateType damageCalculateType;
    public int damage = 1;
    public CalculateType attackCntCalculateType;
    public int attackCnt = 1;
    public Sprite image;
}
