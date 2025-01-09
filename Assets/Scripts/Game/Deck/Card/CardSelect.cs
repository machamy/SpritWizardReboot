using System;
using System.Collections;
using Game;
using Game.World;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public CardObject _cardObject;
    [SerializeField] private Board board;
    private RectTransform rectTransform;
    private Canvas canvas;
    
    public bool isDraggable = true;
    private bool isUsed = false;
    public bool IsUsed => isUsed;
    private bool isDragging = false;
    public bool IsDragging => isDragging;
    private bool isFocused = false;
    public bool IsFocused => isFocused;
    //[Header("Events")]
    public event Action<CardSelect> OnDragStart;
    public event Action<CardSelect> OnDragging;
    public event Action<CardSelect> OnDragEnd;
    public event Action<CardSelect> OnFocus;
    public event Action<CardSelect> OnUnfocus;
    
    public event Action<Tile> OnPointerTileEnter;
    public event Action<Tile> OnPointerTileExit;
    

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        if(board == null)
            board = BattleManager.Instance.Board;
        _cardObject = GetComponent<CardObject>();
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
        if(!isDraggable)
            return;
        isDragging = true;
        OnDragStart?.Invoke(this);
    }
    
    private Tile previousEnteredTile;
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if(!isDraggable)
            return;
        OnDragging?.Invoke(this);
        Vector3 screenToWorldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        rectTransform.position = eventData.position;
        Tile tile = board.GetTile(board.WorldToCell(screenToWorldPoint));
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
        if(!isDraggable)
            return;
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
            // StartCoroutine(DelayedDestroy());
            // IEnumerator DelayedDestroy()
            // {
            //     yield return new WaitForSeconds(1);
            //     _cardObject.Discard();
            // }
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
        isDragging = false;
        OnDragEnd?.Invoke(this);
    }


    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isFocused = true;
        OnFocus?.Invoke(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isFocused = false;
        OnUnfocus?.Invoke(this);
    }
}
