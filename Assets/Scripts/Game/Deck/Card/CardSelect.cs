using System;
using Game;
using Game.World;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Card card;
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
    
    public event Action<Tile> OnPointerTileEnter;
    public event Action<Tile> OnPointerTileExit;
    

    void Awake()
    {
        card = GetComponent<Card>();
        rectTransform = GetComponent<RectTransform>();
        if(board == null)
            board = BattleManager.Instance.Board;
    }
    
    private void OnEnable()
    {
        card.OnCardDrawn += OnCardDrawn;
    }
    
    private void OnDisable()
    { 
        card.OnCardDrawn -= OnCardDrawn;
    }
    
    private void OnCardDrawn(CardData cardSO)
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
        bool isSuccessful = CardCastManager.Instance.UseCard(card.CardData, boardPos);
        if(isSuccessful)
        {
            card.CardDisplay.ShowDecayDelayed(1);
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
