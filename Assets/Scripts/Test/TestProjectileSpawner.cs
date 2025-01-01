using Game.Entity;
using UnityEngine;

namespace Test
{
    public class TestProjectileSpawner : MonoBehaviour
    {
        public Define.Direction direction = Define.Direction.R;
        public PlayerProjectile projectilePrefab;
        public int projectileCount = 1;
        public bool changeDirection = false;

        public void SpawnProjectiles()
        {
            for (int i = 0; i < projectileCount; i++)
            {
                SpawnProjectile();
            }
        }
        
        public void SpawnProjectile()
        {
            if (changeDirection)
                direction = (Define.Direction)((int)++direction % (int)Define.Direction.MAX);

            var projectile = Instantiate(projectilePrefab);
            projectile.Initialize(direction , 1f);
            projectile.transform.position = transform.position;
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnProjectiles();
            }
        }
    }
}