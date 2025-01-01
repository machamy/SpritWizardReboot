using System;
using DefaultNamespace;
using Game.World;
using UnityEngine;
using Math = Unity.Mathematics.Geometry.Math;

namespace Game.Entity
{
    public class Entity : MonoBehaviour
    {
        private bool _isInitialized;
        private Board _board;
        [SerializeField] private Vector2Int _position;
        public Vector2Int Position => _position;

        private void Start()
        {
            if (!_isInitialized)
            {
                Initialize(Board.Instance, Board.Instance.WorldToCell(transform.position));
            }
        }

        public void Initialize(Board board, Vector2Int position)
        {
            _board = board;
            _isInitialized = true;
            MoveToImmediate(position);
        }
        
        public void MoveTo(Vector2Int position)
        {
            Tile currentTile = _board.GetTile(_position);
            currentTile.RemoveEntity(this);
            Vector2Int clamped = Utilities.Vector2IntClamp(position, Vector2Int.zero, _board.GridSize - Vector2Int.one);
            Tile nextTile = _board.GetTile(clamped);
            nextTile.AddEntity(this);
            _position = clamped;
        }
        
        public void MoveToImmediate(Vector2Int position)
        {
            Tile currentTile = _board.GetTile(_position);
            currentTile.RemoveEntity(this);
            Vector2Int clamped = Utilities.Vector2IntClamp(position, Vector2Int.zero, _board.GridSize - Vector2Int.one);
            Tile nextTile = _board.GetTile(clamped);
            nextTile.AddEntity(this);
            _position = clamped;
            transform.position = _board.CellCenterToWorld(clamped);
        }
        
        public void MoveDirection(Direction direction, int distance)
        {
            Vector2Int newPosition = _position + direction.ToVectorInt() * distance;
            MoveTo(newPosition);
        }
        
        public void MoveDirectionImmediate(Direction direction, int distance)
        {
            Vector2Int newPosition = _position + direction.ToVectorInt() * distance;
            MoveToImmediate(newPosition);
        }
        
        public void MoveLeft(int distance)
        {
            MoveTo(_position + Vector2Int.left * distance);
        }
        public void MoveRight(int distance)
        {
            MoveTo(_position + Vector2Int.left * distance);
        }
    }
}