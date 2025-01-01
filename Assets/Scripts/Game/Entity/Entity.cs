using System;
using Game.World;
using UnityEngine;
using Math = Unity.Mathematics.Geometry.Math;

namespace Game.Entity
{
    public class Entity : MonoBehaviour
    {
        private Board _board;
        private Vector2Int _position;
        public Vector2Int Position => _position;
        
        public void Initialize(Board board, Vector2Int position)
        {
            _board = board;
            _position = position;
        }
        
        public void MoveTo(Vector2Int position)
        {
            
            Tile currentTile = _board.GetTile(_position);
            currentTile.AddEntity(this);
        }
        
        public void MoveLeft(int distance)
        {
            MoveTo(_position + Vector2Int.left * distance);
        }
    }
}