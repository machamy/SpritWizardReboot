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
    
    
    /// <summary>
    /// 뽑을 카드 더미에서 카드를 하나 뽑는다.
    /// 만약 더미가 비어있으면 버린 카드 더미를 섞어서 더미로 옮긴다.
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// 전체 덱 리스트에 카드를 추가한다.
    /// </summary>
    /// <param name="cardMetaData"></param>
    public void AddCard(CardMetaData cardMetaData)
    {
        cardDataList.Add(cardMetaData);
    }
    /// <summary>
    /// 뽑을 카드 더미에 전체 덱 리스트를 넣는다.
    /// </summary>
    public void SetupForBattle()
    {
        drawCardQueue = new Queue<CardMetaData>(cardDataList);
        ShuffleDrawPool();
    }
    /// <summary>
    /// 뽑을 카드 더미에 카드를 추가한다.
    /// </summary>
    /// <param name="cardMetaData"></param>
    public void AddCardToDrawPool(CardMetaData cardMetaData)
    {
        drawCardQueue.Enqueue(cardMetaData);
        drawSize.Value = drawCardQueue.Count;
    }
    
    /// <summary>
    /// 버릴 카드 더미에 카드를 추가한다
    /// </summary>
    /// <param name="cardMetaData"></param>
    public void AddCardToDiscardPool(CardMetaData cardMetaData)
    {
        discardCardQueue.Enqueue(cardMetaData);
        discardSize.Value = discardCardQueue.Count;
    }
    
    /// <summary>
    /// 뽑을 카드 더미를 섞는다
    /// </summary>
    public void ShuffleDrawPool()
    {
        List<CardMetaData> drawList = new List<CardMetaData>(drawCardQueue);
        drawList.Shuffle();
        drawCardQueue = new Queue<CardMetaData>(drawList);
        drawSize.Value = drawCardQueue.Count;
        discardSize.Value = discardCardQueue.Count;
    }
    
    /// <summary>
    /// 버릴 카드 더미를 섞는다
    /// </summary>
    public void ShuffleDiscardPool()
    {
        List<CardMetaData> discardList = new List<CardMetaData>(discardCardQueue);
        discardList.Shuffle();
        discardCardQueue = new Queue<CardMetaData>(discardList);
        drawSize.Value = drawCardQueue.Count;
        discardSize.Value = discardCardQueue.Count;
    }

    /// <summary>
    /// 버릴카드 더미를 섞은 후, 뽑을 카드 더미에 추가한다.
    /// </summary>
    public void ShuffleDiscardToDraw()
    {
        List<CardMetaData> discardList = new List<CardMetaData>(discardCardQueue);
        discardList.Shuffle();
        drawCardQueue = new Queue<CardMetaData>(discardList);
        discardCardQueue.Clear();
        discardSize.Value = discardCardQueue.Count;
        drawSize.Value = drawCardQueue.Count;
    }

    /// <summary>
    /// 버린카드더미와 뽑을 카드 더미를 모두 섞고, 뽑을카드 더미에 넣는다
    /// </summary>
    public void ShuffleAll()
    {
        List<CardMetaData> resultList = new List<CardMetaData>(drawCardQueue);
        resultList.AddRange(discardCardQueue);
        resultList.Shuffle();
        drawCardQueue = new Queue<CardMetaData>(resultList);
    }
    
}
