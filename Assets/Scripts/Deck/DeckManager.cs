using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    private List<CardSO> allCards = new List<CardSO>();
    private Queue<CardSO> _deckQueue = new Queue<CardSO>();

    [SerializeField] private CardDisplay[] deck;

    void Start()
    {
        allCards = new List<CardSO>(Resources.LoadAll<CardSO>("Cards"));

        InitDeck();
    }

    void Update()
    {
        
    }

    public void CardDraw()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_deckQueue.Count <= 0)
                InitDeck();
            
            deck[i].DisplayCard(_deckQueue.Dequeue());
        }
    }

    private void InitDeck()
    {
        Shuffle(allCards);
        _deckQueue = new Queue<CardSO>(allCards);
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
