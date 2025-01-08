using System;
using DefaultNamespace;
using DG.Tweening;
using Game.World;
using UnityEngine;
using UnityEngine.Serialization;
using Math = Unity.Mathematics.Geometry.Math;

namespace Game.Entity
{
    public class Entity : MonoBehaviour
    {
        private bool _isInitialized;
        private Board _board;
        [FormerlySerializedAs("_position")] [SerializeField] private Vector2Int _coordinate;
        private Tile currentTile;
        public Tile CurrentTile => _board.GetTile(_coordinate);
        public Vector2Int Coordinate => _coordinate;

        private bool isDeath = true;
        public bool IsDeath => isDeath;

        private void Start()
        {
            if (!_isInitialized)
            {
                _board = BattleManager.Instance.Board;
                Initialize(_board, _board.WorldToCell(transform.position));
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
            Tile currentTile = _board.GetTile(_coordinate);
            currentTile.RemoveEntity(this);
            Vector2Int clamped = Utilities.Vector2IntClamp(position, Vector2Int.zero, _board.GridSize - Vector2Int.one);
            Tile nextTile = _board.GetTile(clamped);
            nextTile.AddEntity(this);
            _coordinate = clamped;
        }

        public void MoveToAnimated(Vector2Int position, float time = 1f)
        {
            MoveTo(position);
            transform.DOMove(_board.CellCenterToWorld(_coordinate), time);
        }
        
        public void MoveToImmediate(Vector2Int position)
        {
            MoveTo(position);
            transform.position = _board.CellCenterToWorld(_coordinate);
        }
        
        public Vector2Int MoveDirection(Direction direction, int distance)
        {
            Vector2Int newPosition = _coordinate + direction.ToVectorInt() * distance;
            MoveTo(newPosition);
            return newPosition;
        }
        
        public Vector2Int  MoveDirectionAnimated(Direction direction, int distance, float time = 1)
        {
            Vector2Int newPosition = _coordinate + direction.ToVectorInt() * distance;
            MoveToAnimated(newPosition, time);
            return newPosition;
        }
        
        public Vector2Int MoveDirectionImmediate(Direction direction, int distance)
        {
            Vector2Int newPosition = _coordinate + direction.ToVectorInt() * distance;
            MoveToImmediate(newPosition);
            return newPosition;
        }
        
        public void MoveLeft(int distance)
        {
            MoveTo(_coordinate + Vector2Int.left * distance);
        }
        public void MoveRight(int distance)
        {
            MoveTo(_coordinate + Vector2Int.left * distance);
        }
        
        /// <summary>
        /// Destroy 대신 이걸 사용해야함
        /// </summary>
        public void Delete()
        {
            isDeath = true;
            if(TryGetComponent(out HitHandler hitHandler))
                hitHandler.isDeath = true;
            if(CurrentTile != null)
                CurrentTile.RemoveEntity(this);
        }
        
    }
}