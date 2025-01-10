using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Deck.CardHolder
{
    public class HorizontalHandCardHolder : BaseCardHolder
    {
        
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


        protected override void OnCardDiscard(CardObject cardObject)
        {
            if (!cardObject.CardSelect.IsUsed)
            {
                var gameObjectTransform = cardObject.gameObject.transform;
                cardObject.transform.position = new Vector3(transform.position.x, - gameObjectTransform.position.y * 2, transform.position.z);
                RemoveCardFromHolder(cardObject);
                
                StartCoroutine(WaitDiscardCard(cardObject));
                IEnumerator WaitDiscardCard(CardObject cardObject)
                {
                    yield return new WaitForSeconds(0.25f);
                    CardDisplay cardDisplay = cardObject.CardDisplay;
                    yield return new WaitWhile(() => DOTween.IsTweening(cardObject.transform) || cardDisplay.IsAnimating);
                    Destroy(cardDisplay.gameObject);
                    Destroy(cardObject.gameObject);
                }
            }
            else{
                base.OnCardDiscard(cardObject);
            }
        }
    }
}