

using System.Collections;
using System.Collections.Generic;
using EventChannel;
using Game;
using Game.Player;
using UnityEngine;


public class CardCastManager : Singleton<CardCastManager>
{
    [SerializeField] private Card[] cards;
    
    private RuneEffectHolder _runeEffectHolder;
    private Queue<CardData> deckQueue = new Queue<CardData>();




    private void Start()
    {
        _runeEffectHolder = GetComponent<RuneEffectHolder>();
    }


    
    public bool UseCard(CardData cardData, Vector2Int targetPosition)
    {
        if(BattleManager.Instance.IsOnBattle == false)
        {
            Debug.Log("Not in battle");
            return false;
        }
        if (cardData == null)
        {
            Debug.Log("No card selected");
            return false;
        }
        // TODO 현재 마나 확인
        if (cardData.cost > 9999)
        {
            Debug.Log("Not enough mana");
            return false;
        }
        Debug.Log($"Cast Card : {cardData.cardName}");
        switch (cardData.cardType)
        {
            case CardType.Attack:
                return UseAttackCard(cardData, targetPosition);
            case CardType.Rune:
                return UseRuneCard(cardData);
        }
        return false;
    }
    
    private bool UseAttackCard(CardData cardData, Vector2Int targetPosition)
    {
        if (cardData is not MagicCardData)
        {
            Debug.Log("Invalid card");
            return false;
        }
        MagicCardData magicCardData = (MagicCardData) cardData;
        Slime silme = BattleManager.Instance.GetSlime(magicCardData.skillCaster);
        if(magicCardData.magicCardAction.Execute(silme, magicCardData, targetPosition, out var routine))
        {
            StartCoroutine(CastAndEndTurnRoutine(routine));
        }
        return true;
    }
    
    private bool UseRuneCard(CardData cardData)
    {
        var effectHolder = GetComponent<RuneEffectHolder>();
        if (cardData is not RuneCardData)
        {
            Debug.Log("Invalid card");
            return false;
        }
        RuneCardData runeCardData = (RuneCardData) cardData;
        effectHolder.StackRuneEffectRange(runeCardData.runeEffectAmounts);
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
