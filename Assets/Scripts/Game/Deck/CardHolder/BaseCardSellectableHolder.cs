
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseCardSellectableHolder : BaseCardHolder
{
    
    [Tooltip("최대 선택 수에 도달하면 자동 종료")] public bool autoExit = true;
    public bool isCardSelectable = false;
    public int maxSellactableCardCount = 1;
    public List<CardObject> sellectedCardObjects = new List<CardObject>();
    
    /// <summary>
    /// 정상적으로 종료(확인 버튼 등) 이벤트.
    /// </summary>
    public event Action<BaseCardSellectableHolder> OnExitSuccessfully;
    /// <summary>
    /// 취소 이벤트.
    /// </summary>
    public event Action<BaseCardSellectableHolder> OnExitCanceled;



    protected override void OnCardPointerUp(CardSelect cardSelect, bool isClick)
    {
        if (!isCardSelectable)
        {
            return;
        }
        if (isClick)
        {
            if (cardSelect.IsSelected)
            {
                SelectCard(cardSelect._cardObject);
            }
            else
            {
                UnselectCard(cardSelect._cardObject);
            }
        }
    }

    public void SelectCard(CardObject cardObject)
    {
        if (sellectedCardObjects.Count >= maxSellactableCardCount)
        {
            return;
        }
        sellectedCardObjects.Add(cardObject);
        if (autoExit && sellectedCardObjects.Count >= maxSellactableCardCount)
        {
            ExitSuccessfully();
        }
    }

    public void UnselectCard(CardObject cardObject)
    {
        if (!sellectedCardObjects.Contains(cardObject))
        {
            Debug.LogWarning("UnselectCard : 카드가 선택되지 않은 상태에서 선택 해제 시도");
            return;
        }
        sellectedCardObjects.Remove(cardObject);
    }
    
    public void ExitSuccessfully()
    {
        Disable();
        OnExitSuccessfully?.Invoke(this);
    }
    
    public void ExitCanceled()
    {
        Disable();
        OnExitCanceled?.Invoke(this);
    }
}
