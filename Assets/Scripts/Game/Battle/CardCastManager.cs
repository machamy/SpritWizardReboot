

using System.Collections;
using System.Collections.Generic;
using EventChannel;
using Game;
using Game.Entity;
using Game.Player;
using UnityEngine;


/// <summary>
/// 카드 시전 관리 매니저
/// </summary>
/// <remarks>
/// TODO : 카드 시전을 룬/마법 분리할 필요 없음. 개선 가능
/// </remarks>
public class CardCastManager : Singleton<CardCastManager>
{
    [SerializeField] private CardObject[] cards;
    
    private RuneEffectHolder _runeEffectHolder;
    
    private void Start()
    {
        _runeEffectHolder = GetComponent<RuneEffectHolder>();
    }
    
    public bool UseCard(CardMetaData cardMetaData, Vector2Int targetPosition)
    {
        var bm = BattleManager.Instance;
        if(bm.IsOnBattle == false)
        {
            Debug.Log("Not in battle");
            return false;
        }
        if (!bm.TurnManager.IsPlayerTurnStrict)
        {
            Debug.Log("Not player turn");
            return false;
        }
        if (cardMetaData == null)
        {
            Debug.Log("No card selected");
            return false;
        }
        if (bm.Board.GetTile(targetPosition) == null)
        {
            Debug.Log("No target position");
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

    public int GetMoveCntRuneEffect(CardData cardData)
    {
        return _runeEffectHolder.GetRuneEffect(Define.RuneEffectType.moveCnt);
    }
    
    private bool UseAttackCard(CardMetaData cardMetaData, Vector2Int targetPosition)
    {
        CardData cardData = cardMetaData.cardData;
        Slime silme = BattleManager.Instance.GetSlime(cardData.skillCaster);

        if(!cardData.CanCastTo(silme.GetComponent<Entity>().Coordinate, targetPosition, GetMoveCntRuneEffect(cardData)))
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
            StartCoroutine(CastAndEndTurnRoutine(routine, cardMetaData.cost));
        }
        return true;
    }
    
    
    private bool UseRuneCard(CardMetaData cardMetaData)
    {
        // TODO : 룬 적용도 카드 안에서 하도록 개선 가능
        var effectHolder = GetComponent<RuneEffectHolder>();
        CardData cardData = cardMetaData.cardData;
        if(!cardData.CanCast())
        {
            return false;
        }
        effectHolder.StackRuneEffectRange(cardData.runeEffectAmounts);
        StartCoroutine(CastAndEndTurnRoutine(1.0f,cardMetaData.cost));
        return true;
    }
    /// <summary>
    /// 해당 시간 후에 턴을 넘긴다.
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    private IEnumerator CastAndEndTurnRoutine(float time, int usedCost)
    {
        yield return new WaitForSeconds(time);
        BattleManager.Instance.TurnManager.ReadyToEndPlayerTurn(usedCost);
    }
    /// <summary>
    /// 해당 시전 루틴이 끝난 후에 턴을 넘긴다.
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    private IEnumerator CastAndEndTurnRoutine(IEnumerator routine, int usedCost)
    {
        yield return routine;
        BattleManager.Instance.TurnManager.ReadyToEndPlayerTurn(usedCost);
    }
    
}
