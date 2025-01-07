

using System.Collections;
using System.Collections.Generic;
using EventChannel;
using Game;
using Game.Entity;
using Game.Player;
using UnityEngine;


public class CardCastManager : Singleton<CardCastManager>
{
    [SerializeField] private Card[] cards;
    
    private RuneEffectHolder _runeEffectHolder;
    private Queue<CardMetaData> deckQueue = new Queue<CardMetaData>();




    private void Start()
    {
        _runeEffectHolder = GetComponent<RuneEffectHolder>();
    }


    
    public bool UseCard(CardMetaData cardMetaData, Vector2Int targetPosition)
    {
        if(BattleManager.Instance.IsOnBattle == false)
        {
            Debug.Log("Not in battle");
            return false;
        }
        if (cardMetaData == null)
        {
            Debug.Log("No card selected");
            return false;
        }
        // TODO 현재 마나 확인
        if (cardMetaData.cost > 9999)
        {
            Debug.Log("Not enough mana");
            return false;
        }
        Debug.Log($"Cast Card : {cardMetaData.cardName}");
        switch (cardMetaData.cardType)
        {
            case CardType.Attack:
                return UseAttackCard(cardMetaData, targetPosition);
            case CardType.Rune:
                return UseRuneCard(cardMetaData);
        }
        return false;
    }
    
    private bool UseAttackCard(CardMetaData cardMetaData, Vector2Int targetPosition)
    {
        CardData cardData = cardMetaData.cardData;
        Slime silme = BattleManager.Instance.GetSlime(cardData.skillCaster);
        if(!cardData.CanCastTo(silme.GetComponent<Entity>().Coordinate, targetPosition))
        {
            return false;
        }
        
        CardData runeAppliedCardData = cardData.Clone() as CardData;
        foreach (var runeEffectType in cardData.runeEffectTypes)
        {
            int runeEffectAmount = _runeEffectHolder.PopRuneEffect(runeEffectType);
            switch (runeEffectType)
            {
                case Define.RuneEffectType.damage:
                    runeAppliedCardData.attackDamage += runeEffectAmount;
                    break;
                case Define.RuneEffectType.attackCnt:
                    runeAppliedCardData.attackCount += runeEffectAmount;
                    break;
                case Define.RuneEffectType.moveCnt:
                    runeAppliedCardData.move += runeEffectAmount;
                    break;
            }
        }
        
        if(cardData.cardAction.Execute(silme, runeAppliedCardData, targetPosition, out var routine))
        {
            StartCoroutine(CastAndEndTurnRoutine(routine));
        }
        return true;
    }
    
    private bool UseRuneCard(CardMetaData cardMetaData)
    {
        var effectHolder = GetComponent<RuneEffectHolder>();
        CardData cardData = cardMetaData.cardData;
        if(!cardData.CanCast())
        {
            return false;
        }
        effectHolder.StackRuneEffectRange(cardData.runeEffectAmounts);
        StartCoroutine(CastAndEndTurnRoutine(1.0f));
        return true;
    }
    /// <summary>
    /// 해당 시간 후에 턴을 넘긴다.
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    private IEnumerator CastAndEndTurnRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        BattleManager.Instance.TurnManager.ReadyToEndPlayerTurn();
    }
    /// <summary>
    /// 해당 시전 루틴이 끝난 후에 턴을 넘긴다.
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    private IEnumerator CastAndEndTurnRoutine(IEnumerator routine)
    {
        yield return routine;
        BattleManager.Instance.TurnManager.ReadyToEndPlayerTurn();
    }
    
}
