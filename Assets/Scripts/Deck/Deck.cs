using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public List<CardData> allCards { get; private set; } = new List<CardData>();
    

    public Queue<CardData> GetDeckQueue()
    {
        Shuffle(allCards);
        return new Queue<CardData>(allCards);
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
