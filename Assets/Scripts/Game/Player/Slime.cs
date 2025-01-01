using System.Collections;
using System.Collections.Generic;
using Game.Entity;
using Game.World;
using Unity.Mathematics.Geometry;
using UnityEngine;
using Math = System.Math;

namespace Game.Player
{
    [RequireComponent(typeof(Entity.Entity))]
    public class Slime : MonoBehaviour
    {
        [SerializeField] SkillCaster skillCaster;
        public SkillCaster SkillCaster => skillCaster;
        
        private Entity.Entity entity;
        [Header("Debug")]
        [SerializeField] private AttackCardSO testCard;
        private void Awake()
        {
            entity = GetComponent<Entity.Entity>();
        }
        
        [ContextMenu("Cast Test Card")]
        public void CastTestCard()
        {
            Vector2Int randomPos = new Vector2Int(Random.Range(0, 14), Random.Range(0, 5));
            CastCard(testCard, randomPos);
        }
        
        public void CastCard(AttackCardSO card, Vector2Int targetPosition)
        {
            Board board = Board.Instance;
            StartCoroutine(CastCardRoutine(board, card, targetPosition));
        }
        
        private IEnumerator CastCardRoutine(Board board, AttackCardSO card, Vector2Int targetPosition)
        {
            entity.MoveToImmediate(targetPosition);
            yield return new WaitForSeconds(0.2f);
            Direction direction = Direction.R;
            int cnt = card.attackCnt;
            int spreadMax = Math.Min(card.spreadRange,(int)Direction.MAX);
            while (cnt-- > 0)
            {
                if (card.attackType == AttackType.projectile)
                {
                    
                    PlayerProjectile projectile = Instantiate(card.projectilePrefab);
                    projectile.Initialize(direction, card.damage, card.pierce);
                    projectile.transform.position = entity.transform.position;
                }
                else
                {
                    List<Tile> tiles;
                    if (card.attackType == AttackType.beam)
                    {
                        tiles = board.GetTilesBeam(targetPosition, direction, card.attackRange.x,
                            card.attackRange.y);
                    }
                    else
                    {
                        tiles = board.GetTilesSquare(targetPosition, card.spreadRange / 2);
                    }
                    foreach (var t in tiles)
                        {
                            t.ShowDebugColor(new Color(1f, 0, 0, 0.5f));
                            if (t.IsClear())
                                continue;
                            foreach (var e in t.GetEntities())
                            {
                                if (e.CompareTag("Enemy"))
                                {
                                    Enemy enemy = e.GetComponent<Enemy>();
                                    enemy.OnHit(card.damage);
                                }
                            }
                        }
                }
                
                if (card.attackSpread == AttackSpread.radial)
                {
                    direction = (Direction) ((int) ++direction % spreadMax);
                    if (direction == Direction.R)
                    {
                        yield return new WaitForSeconds(0.75f);
                    }
                }
            }
            yield break;
        }
    }
}