using System;
using System.Collections.Generic;
using DefaultNamespace;
using Random = UnityEngine.Random;

[Serializable]
public class Deck
{
    List<CardMetaData> cardDataList = new List<CardMetaData>();
    
    Queue<CardMetaData> drawCardQueue = new Queue<CardMetaData>();
    Queue<CardMetaData> discardCardQueue = new Queue<CardMetaData>();
    
    
    public CardMetaData DrawCard()
    {
        if (drawCardQueue.Count <= 0)
        {
            if (discardCardQueue.Count <= 0)
                return null;
            ShuffleDiscardPool();
            drawCardQueue = new Queue<CardMetaData>(discardCardQueue);
            discardCardQueue.Clear();
        }
        return drawCardQueue.Dequeue();
    }
    
    public void AddCard(CardMetaData cardMetaData)
    {
        cardDataList.Add(cardMetaData);
    }
    public void SetupForBattle()
    {
        drawCardQueue = new Queue<CardMetaData>(cardDataList);
        ShuffleDrawPool();
    }
    public void AddCardToDrawPool(CardMetaData cardMetaData)
    {
        drawCardQueue.Enqueue(cardMetaData);
    }
    public void AddCardToDiscardPool(CardMetaData cardMetaData)
    {
        discardCardQueue.Enqueue(cardMetaData);
    }
    
    public void ShuffleDrawPool()
    {
        List<CardMetaData> drawList = new List<CardMetaData>(drawCardQueue);
        drawList.Shuffle();
        drawCardQueue = new Queue<CardMetaData>(drawList);
    }
    
    public void ShuffleDiscardPool()
    {
        List<CardMetaData> discardList = new List<CardMetaData>(discardCardQueue);
        discardList.Shuffle();
        discardCardQueue = new Queue<CardMetaData>(discardList);
    }
    
}
