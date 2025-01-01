using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : Card
{
    [SerializeField] private Card cardData;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI damageText;

    public void DisplayCard(CardSO card)
    {
        cardData.card = card;
        nameText.text = card.cardName;
        descriptionText.text = card.description;
        image.sprite = card.image;
        costText.text = card.cost.ToString();
        damageText.text = card.damage.ToString();
    }
}
