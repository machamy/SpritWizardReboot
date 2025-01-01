using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Card cardData;
    [SerializeField] private Board board;

    private Vector3 prevPos;
    private RectTransform rectTransform;

    void Start()
    {
        cardData = GetComponent<Card>();
        rectTransform = GetComponent<RectTransform>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        prevPos = transform.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int boardPos = board.WorldToCell(mousePos);
        CardManager.Instance.UseCard(cardData.card, boardPos);
        rectTransform.position = prevPos;
    }


    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.position += Vector3.up * 30;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        rectTransform.position += Vector3.down * 30;
    }
}
