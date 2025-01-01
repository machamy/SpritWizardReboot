using UnityEngine;

[CreateAssetMenu(fileName = "AttackCard", menuName = "Card/Attack Card")]
public class AttackCardSO : CardSO
{
    public SkillCaster skillCaster;
    public AttackType attackType;
    public Vector2Int attackRange = new Vector2Int(1, 1);
    public AttackSpread attackSpread;
    public int spreadRange = 1;
    public int pierce = 0;
    public int move = 0;
    // TODO -> specialeffectid 추가

    private void OnEnable()
    {
        cardName = "New Attack Card";
        cardType = CardType.Attack;
    }
}
