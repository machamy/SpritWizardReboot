using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<CardSO> allCards { get; private set; } = new List<CardSO>();
    

    void Start()
    {
        allCards = new List<CardSO>(Resources.LoadAll<CardSO>("Cards"));
    }

    public Queue<CardSO> GetDeckQueue()
    {
        Shuffle(allCards);
        return new Queue<CardSO>(allCards);
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
