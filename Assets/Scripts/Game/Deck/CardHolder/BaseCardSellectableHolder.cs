
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

    protected override void OnFocus(CardSelect cardSelect)
    {
        base.OnFocus(cardSelect);
        CardDisplay cardDisplay = cardSelect._cardObject.CardDisplay;
        if(!cardDisplay.focusCardVisible)
            return;
        float focusedY = cardDisplay.GetClampedVisiblePos(cardSelect.transform.position.y);
        cardSelect.transform.position = new Vector3(cardSelect.transform.position.x, focusedY, cardSelect.transform.position.z);
        cardSelect.transform.localScale = Vector3.one * cardDisplay.focusedScale;
        cardDisplay.transform.SetAsLastSibling();
    }
        
    protected override void OnUnfocus(CardSelect cardSelect)
    {
        cardSelect.transform.localPosition = Vector3.zero;
        cardSelect.transform.localScale = cardSelect._cardObject.CardDisplay.unfocusedScale * Vector3.one;
        UpdateAllCardIndex();
        base.OnUnfocus(cardSelect);
    }

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
        print($"[BaseCardSellactableHolder::ExitSuccessfully] sellectedCardObjects.Count : {sellectedCardObjects.Count} first : {sellectedCardObjects[0].CardMetaData.cardName}");
        OnExitSuccessfully?.Invoke(this);
        Disable();
    }
    
    public void ExitCanceled()
    {
        gameObject.SetActive(false);
        OnExitCanceled?.Invoke(this);
    }
}
