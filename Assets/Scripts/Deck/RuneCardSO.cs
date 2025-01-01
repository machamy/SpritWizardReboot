using UnityEngine;

[CreateAssetMenu(fileName = "RuneCard", menuName = "Card/Rune Card")]
public class RuneCardSO : CardSO
{
    public CalculateType damageCalculateType;
    public CalculateType attackCntCalculateType;

    private void OnEnable()
    {
        cardName = "New Rune Card";
        cardType = CardType.Rune;
    }
}
