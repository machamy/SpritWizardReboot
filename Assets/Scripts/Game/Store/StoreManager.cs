using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private Transform storeProducts;
    [SerializeField] private BaseCardSellectableHolder cardHolder;
    [SerializeField] private PlayerDataSO playerDataSO;

    private TextMeshProUGUI[] priceTexts;

    private List<CardMetaData> commonRuneCards;
    private List<CardMetaData> commonMagicCards;
    private List<CardMetaData> rareRuneCards;
    private List<CardMetaData> rareMagicCards;

    [Header("Costs")]
    private List<StorePriceData> cardPrices; // 0 -> commonMagic, 1 -> rareMagic, 2 -> commonRune, 3 -> rareRune
    private List<StorePriceData> editCardPrices; // 0 -> destroy, 1 -> upgrade1, 2 -> upgrade2

    private void OnEnable()
    {
        cardHolder.OnExitSuccessfully += BuyCard;
    }

    private void OnDisable()
    {
        cardHolder.OnExitSuccessfully -= BuyCard;
    }

    private void Awake()
    {
        priceTexts = new TextMeshProUGUI[storeProducts.childCount];
        for (int i = 0; i < storeProducts.childCount; i++)
        {
            priceTexts[i] = storeProducts.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        IEnumerable<CardMetaData> runeCards = Database.AllCardMetas.Where(e => e.cardType == CardType.Rune);
        commonRuneCards = runeCards.Where(e => e.rarity == Rarity.common).ToList();
        rareRuneCards = runeCards.Where(e => e.rarity == Rarity.rare).ToList();

        IEnumerable<CardMetaData> magicCards = Database.AllCardMetas.Where(e => e.cardType == CardType.Attack).ToList();
        commonMagicCards = magicCards.Where(e => e.rarity == Rarity.common).ToList();
        rareMagicCards = magicCards.Where(e => e.rarity == Rarity.rare).ToList();

        cardPrices = Database.AllCardPrice;
        editCardPrices = Database.AllEditCardPrice;

        InitStore();
    }

    public void InitStore()
    {
        int rareIdx = Random.Range(0, 4); // 마법2 + 룬2 중 하나가 rare
        List<CardMetaData> cards = new List<CardMetaData>(); // 보여줄 카드 리스트

        for (int i = 0; i < cardPrices.Count; i++)
        {
            priceTexts[i].text = cardPrices[i/2*2].storePrice.GetRandomInRange().ToString();
            if (i / 2 == 0) cards.Add(commonRuneCards[Random.Range(0, commonRuneCards.Count)]);
            else cards.Add(commonMagicCards[Random.Range(0, commonMagicCards.Count)]);
        }

        priceTexts[rareIdx].text = cardPrices[rareIdx/2*2 + 1].storePrice.GetRandomInRange().ToString(); // 0, 1 -> 1   // 2, 3 -> 3    각 카드타입의 레어일때 값으로 설정
        CardMetaData rareCard;
        if (rareIdx / 2 == 0) rareCard = rareRuneCards[Random.Range(0, rareRuneCards.Count)];
        else rareCard = rareMagicCards[Random.Range(0, rareMagicCards.Count)];
        cards[rareIdx] = rareCard;

        cardHolder.Enable();
        cardHolder.Initialize(cards);

        for (int i = 0; i < editCardPrices.Count; i++)
        {
            priceTexts[i + cardPrices.Count].text = editCardPrices[i].storePrice.GetRandomInRange().ToString();
        }
    }

    public void BuyCard(BaseCardSellectableHolder cardholder)
    {
        foreach (CardObject co in cardholder.sellectedCardObjects)
        {
            playerDataSO.Deck.AddCard(co.CardMetaData);
        }
    }
}
