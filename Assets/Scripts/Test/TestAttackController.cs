using System;
using Game;
using Game.Entity;
using UnityEngine;

namespace Test
{
    [RequireComponent(typeof(Entity))]
    public class TestAttackController : MonoBehaviour
    {
        public Entity e;
        
        public Direction direction = Direction.R;
        public bool changeDirection = false;
        [Header("Projectile")]
        public PlayerProjectile projectilePrefab;
        public int projectileCount = 1;
        public KeyCode spawnKey = KeyCode.Space;
        [Header("Square")] 
        public int squareSize = 1;
        public KeyCode squareKey = KeyCode.Q;
        [Header("Beam")] 
        public int beamLength = 1;
        public int beamWidth = 1;
        public KeyCode beamKey = KeyCode.E;
        
        private void OnEnable()
        {
            e = GetComponent<Entity>();
        }

        public void SpawnProjectiles()
        {
            for (int i = 0; i < projectileCount; i++)
            {
                SpawnProjectile();
            }
        }
        
        public void SpawnProjectile()
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.Initialize(direction , 1);
            projectile.transform.position = transform.position;
            if (changeDirection)
                direction = (Direction)((int)++direction % (int)Direction.MAX);
        }
        
        public void AttackSquare()
        {
            Board board = BattleManager.Instance.Board;
            var tiles = board.GetTilesSquare(e.Coordinate, squareSize/2);
            if (TryGetComponent(out HitHandler hitHandler))
            {
                hitHandler.Raise(this, new HitHandler.HitEventArgs(){dmg = 1});
            }
        }
        
        public void AttackBeam()
        {
            Board board = BattleManager.Instance.Board;
            var tiles = board.GetTilesBeam(e.Coordinate, direction, beamLength, beamWidth);
            foreach (var tile in tiles)
            {
                //TODO 딜넣기
                tile.ShowDebugColor(Color.red, 1f);
                foreach (var e in tile.GetEntities())
                {
                    if (TryGetComponent(out HitHandler hitHandler))
                    {
                        hitHandler.Raise(this, new HitHandler.HitEventArgs(){dmg = 1});
                    }
                }
            }
            if (changeDirection)
                direction = (Direction)((int)++direction % (int)Direction.MAX);
        }
        
        public void Update()
        {
            
            if(Input.GetKeyDown(KeyCode.W))
            {
                Move(Direction.U);
            }
            if(Input.GetKeyDown(KeyCode.A))
            {
                Move(Direction.L);
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                Move(Direction.D);
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                Move(Direction.R);
            }
            
            if (Input.GetKeyDown(spawnKey))
            {
                SpawnProjectiles();
            }
            if (Input.GetKeyDown(squareKey))
            {
                AttackSquare();
            }
            if (Input.GetKeyDown(beamKey))
            {
                AttackBeam();
            }
        }
        
        public void Move(Direction direction)
        {
            
            e.MoveDirectionImmediate(direction, 1);
        }
    }
}