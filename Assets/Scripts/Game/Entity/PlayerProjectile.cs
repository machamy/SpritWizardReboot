using System;
using Game.World;
using UnityEngine;

namespace Game.Entity
{

    public class PlayerProjectile : MonoBehaviour
    {
        public float dmg = 1f;
        public float speed = 10f;
        public int penestration = 1;
        private Vector3 _direction;

        
        public void Initialize(Direction direction, float dmg, int penestration = 1)
        {
            _direction = (Vector2)direction.ToVectorInt();
            _direction.Normalize();
            transform.up = _direction;
            this.dmg = dmg;
            this.penestration = penestration;
        }
        
        private void FixedUpdate()
        {
            transform.position += _direction * (speed * Time.fixedDeltaTime);
        }
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Tile"))
            {
                OnTileEntered(other.GetComponent<Tile>());
            }
        }

        public void OnTileEntered(Tile tile)
        {
            tile.ShowDebugColor(Color.red, 0.5f);
            if (tile.IsClear())
                return;
            foreach (var e in tile.GetEntities())
            {
                if (e.CompareTag("Enemy"))
                {
                   //TODO 적 피격 처리 e.GetComponent<Enemy>()
                   Debug.Log($"player projectile hit enemy ${e.name} with damage {dmg}");
                   this.penestration--;
                }

                if (penestration <= 0)
                {
                    break;
                }
            }
            
            if (penestration <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}