using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class CardDisplay : MonoBehaviour
{
    private Card cardData;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI damageText;

    private void Start()
    {
        cardData = GetComponent<Card>();
        if (cardData == null) cardData = gameObject.AddComponent<Card>();
    }

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
