using UnityEngine;

namespace Game.World
{
    public class Tile : MonoBehaviour
    {
        private Vector2Int coordinates;
        
        public Vector2Int Coordinates
        {
            get => coordinates;
        }
        
        public void Initialize(Vector2Int coordinates)
        {
            this.coordinates = coordinates;
            
        }
    }
}