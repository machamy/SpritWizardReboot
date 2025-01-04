using System;
using Game.World;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Card cardData;
    [SerializeField] private Board board;
    private RectTransform rectTransform;
    
    
    private bool isUsed = false;
    public bool IsUsed => isUsed;
    private bool isDragging = false;
    public bool IsDragging => isDragging;
    //[Header("Events")]
    public event Action OnDragStart;
    public event Action OnDragging;
    public event Action OnDragEnd;
    public event Action OnFocus;
    public event Action OnUnfocus;

    void Awake()
    {
        cardData = GetComponent<Card>();
        rectTransform = GetComponent<RectTransform>();
        if(board == null)
            board = Board.Instance;
    }
    
    private void OnEnable()
    {
        cardData.OnCardDrawn += OnCardDrawn;
    }
    
    private void OnDisable()
    { 
        cardData.OnCardDrawn -= OnCardDrawn;
    }
    
    private void OnCardDrawn(CardSO cardSO)
    {
        isUsed = false;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        OnDragStart?.Invoke();
    }
    
    private Tile previousFocusedTile;
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        OnDragging?.Invoke();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Tile tile = board.GetTile(board.WorldToCell(mousePos));
        if(tile != null && previousFocusedTile != tile)
        {
            tile.Focus();
            previousFocusedTile?.Unfocus();  
            previousFocusedTile = tile;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        previousFocusedTile?.Unfocus();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int boardPos = board.WorldToCell(mousePos);
        bool isSuccessful = CardManager.Instance.UseCard(cardData.card, boardPos);
        if(isSuccessful)
        {
            cardData.CardDisplay.ShowDecayDelayed(1);
            isUsed = true;
        }
        isDragging = false;
        OnDragEnd?.Invoke();
    }


    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnFocus?.Invoke();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        OnUnfocus?.Invoke();
    }
}
