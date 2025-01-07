using System;
using Game;
using Game.World;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CardObject _cardObject;
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
        _cardObject = GetComponent<CardObject>();
        rectTransform = GetComponent<RectTransform>();
        if(board == null)
            board = BattleManager.Instance.Board;
    }
    
    private void OnEnable()
    {
        _cardObject.OnCardDrawn += OnCardObjectDrawn;
    }
    
    private void OnDisable()
    { 
        _cardObject.OnCardDrawn -= OnCardObjectDrawn;
    }
    
    private void OnCardObjectDrawn(CardMetaData cardMetaSo)
    {
        isUsed = false;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        OnDragStart?.Invoke();
    }
    
    private Tile previousEnteredTile;
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        OnDragging?.Invoke();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Tile tile = board.GetTile(board.WorldToCell(mousePos));
        if(tile != null && previousEnteredTile != tile)
        {
            if(previousEnteredTile != null)
                OnPointerTileExit?.Invoke(previousEnteredTile);
            previousEnteredTile = tile;
            OnPointerTileEnter?.Invoke(tile);
        }
        else if(tile == null && previousEnteredTile != null)
        {
            OnPointerTileExit?.Invoke(previousEnteredTile);
            previousEnteredTile = null;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if(previousEnteredTile != null)
            OnPointerTileExit?.Invoke(previousEnteredTile);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int boardPos = board.WorldToCell(mousePos);
        bool isSuccessful = CardCastManager.Instance.UseCard(_cardObject.CardMetaData, boardPos);
        if(isSuccessful)
        {
            _cardObject.CardDisplay.ShowDecayDelayed(1);
            isUsed = true;
            _cardObject.Discard();
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
