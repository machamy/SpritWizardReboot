using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class CardSO : ScriptableObject
{
    public string cardName;
    public string description;
    public CardType cardType;
    public AttackType attackType;
    public int cost;
    public int damage;
    public Sprite image;
}
