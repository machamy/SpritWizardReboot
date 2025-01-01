using System.Collections.Generic;
using UnityEngine;

namespace Game.World
{
    public class Tile : MonoBehaviour
    {
        private Vector2Int coordinates;
        private List<Entity.Entity> entities;
        public Vector2Int Coordinates
        {
            get => coordinates;
        }
        
        public void Initialize(Vector2Int coordinates)
        {
            this.coordinates = coordinates;
        }
        
        public bool IsClear()
        {
            return entities.Count == 0;
        }
        
        public List<Entity.Entity> GetEntities()
        {
            return entities;
        }
        
        public void AddEntity(Entity.Entity entity)
        {
            entities.Add(entity);
        }
    }
}