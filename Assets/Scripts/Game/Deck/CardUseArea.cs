
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUseArea : MonoBehaviour
{
    public delegate bool CardUseDelegate(CardMetaData cardMetaData, Vector2 targetPosition, bool isWorldPosition);
    private event CardUseDelegate OnCardUse;
    
    public void SetListener(CardUseDelegate listener)
    {
        OnCardUse += listener;
    }
    
    public void ClearListener()
    {
        OnCardUse = null;
    }
    
    public bool UseCard(CardMetaData cardMetaData, Vector2 targetPosition, bool isWorldPosition = false)
    {
        return OnCardUse?.Invoke(cardMetaData,targetPosition,isWorldPosition) ?? false;
    }
    
    public static CardUseArea GetCardUseArea(Vector2 uiPosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = uiPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        CardUseArea Check()
        {
            foreach (var result in results)
            {
                if(result.gameObject.TryGetComponent(out CardUseArea cardUseArea))
                {
                    return cardUseArea;
                }
            }
            return null;
        }
        return Check();
    }
}
