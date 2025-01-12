using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private Transform storeProducts;

    private TextMeshProUGUI[] priceTexts;

    [Header("Costs")]
    private List<StorePriceData> cardPrices; // 0 -> commonMagic, 1 -> rareMagic, 2 -> commonRune, 3 -> rareRune
    private List<StorePriceData> editCardPrices; // 0 -> destroy, 1 -> upgrade1, 2 -> upgrade2

    private void Awake()
    {
        priceTexts = new TextMeshProUGUI[storeProducts.childCount];
        for (int i = 0; i < storeProducts.childCount; i++)
        {
            priceTexts[i] = storeProducts.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        cardPrices = Database.AllCardPrice;
        editCardPrices = Database.AllEditCardPrice;

        SetPrices();
    }

    public void SetPrices()
    {
        int rareIdx = Random.Range(0, 4); // 마법2 + 룬2 중 하나가 rare

        for (int i = 0; i < cardPrices.Count; i++)
        {
            priceTexts[i].text = cardPrices[i/2*2].storePrice.GetRandomInRange().ToString();
        }
        priceTexts[rareIdx].text = cardPrices[rareIdx/2*2 + 1].storePrice.GetRandomInRange().ToString();
        for (int i = 0; i < editCardPrices.Count; i++)
        {
            priceTexts[i + cardPrices.Count].text = editCardPrices[i].storePrice.GetRandomInRange().ToString();
        }
    }
}
