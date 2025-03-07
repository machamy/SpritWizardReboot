using System;
using Game.World;
using UnityEngine;

namespace Game.Entity
{

    public class PlayerProjectile : MonoBehaviour
    {
        public int dmg = 1;
        public float speed = 10f;
        public int penestration = 1;
        private Vector3 _direction;

        
        public void Initialize(Direction direction, int dmg, int penestration = 1)
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
            
            tile.ShowDebugColor(new Color(1f, 0, 0, 0.5f), 0.5f);
            if (tile.IsClear())
                return;
            foreach (var e in tile.GetEntities())
            {
                if(e == null)
                {
                    continue;
                }
                if (penestration < 0)
                {
                    break;
                }
                if(e.IsDeath)
                    continue;
                if (e.TryGetComponent(out HitHandler hitHandler))
                {
                    bool hitSuccese = hitHandler.Raise(this, new HitHandler.HitEventArgs(){dmg = this.dmg});
                    if(hitSuccese)
                        penestration--;
                }
            }
            
            if (penestration < 0)
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