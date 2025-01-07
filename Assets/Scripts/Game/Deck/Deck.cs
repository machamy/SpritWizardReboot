using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Deck
{
    [SerializeField] private IntVariableSO discardSize;
    [SerializeField] private IntVariableSO drawSize;
    
    List<CardMetaData> cardDataList = new List<CardMetaData>();
    Queue<CardMetaData> drawCardQueue = new Queue<CardMetaData>();
    Queue<CardMetaData> discardCardQueue = new Queue<CardMetaData>();
    
    
    public CardMetaData DrawCard()
    {
        if (drawCardQueue.Count <= 0)
        {
            if (discardCardQueue.Count <= 0)
                return null;
            ShuffleDiscardToDraw();
        }
        CardMetaData cardMetaData = drawCardQueue.Dequeue();
        drawSize.Value = drawCardQueue.Count;
        return cardMetaData;
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
        drawSize.Value = drawCardQueue.Count;
    }
    public void AddCardToDiscardPool(CardMetaData cardMetaData)
    {
        discardCardQueue.Enqueue(cardMetaData);
        discardSize.Value = discardCardQueue.Count;
    }
    
    public void ShuffleDrawPool()
    {
        List<CardMetaData> drawList = new List<CardMetaData>(drawCardQueue);
        drawList.Shuffle();
        drawCardQueue = new Queue<CardMetaData>(drawList);
        drawSize.Value = drawCardQueue.Count;
        discardSize.Value = discardCardQueue.Count;
    }
    
    public void ShuffleDiscardPool()
    {
        List<CardMetaData> discardList = new List<CardMetaData>(discardCardQueue);
        discardList.Shuffle();
        discardCardQueue = new Queue<CardMetaData>(discardList);
        drawSize.Value = drawCardQueue.Count;
        discardSize.Value = discardCardQueue.Count;
    }

    public void ShuffleDiscardToDraw()
    {
        List<CardMetaData> discardList = new List<CardMetaData>(discardCardQueue);
        discardList.Shuffle();
        drawCardQueue = new Queue<CardMetaData>(discardList);
        discardCardQueue.Clear();
        discardSize.Value = discardCardQueue.Count;
        drawSize.Value = drawCardQueue.Count;
    }
    
}
