using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelect : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Card cardData;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        CardManager.Instance.UseCard(cardData.card);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<RectTransform>().position += Vector3.up * 30;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        GetComponent<RectTransform>().position += Vector3.down * 30;
    }
}
