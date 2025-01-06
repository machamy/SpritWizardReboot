using System.Collections;
using System.Collections.Generic;
using DataBase.DataClasses;
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

        private void Awake()
        {
            entity = GetComponent<Entity.Entity>();
        }
        
        
        // public void CastCard(AttackCardSO card, Vector2Int targetPosition)
        // {
        //     Board board = Board.Instance;
        //     StartCoroutine(TestCastCardRoutine(board, card, targetPosition));
        // }
        //
        /// <summary>
        /// 테스트용 카드 시전.
        /// 실제 코드는 카드 자체가 구현하도록 해야함.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="card"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        // private IEnumerator TestCastCardRoutine(Board board, MagicCardData card, Vector2Int targetPosition)
        // {
        //     entity.MoveToImmediate(targetPosition);
        //     yield return new WaitForSeconds(0.2f);
        //     Direction direction = Direction.R;
        //     int cnt = card.attackCount;
        //     int spreadMax = Math.Min(card.spreadRange,(int)Direction.MAX);
        //     while (cnt-- > 0)
        //     {
        //         if (card.attackType == AttackType.projectile)
        //         {
        //             
        //             PlayerProjectile projectile = Instantiate(card.projectilePrefab);
        //             projectile.Initialize(direction, card.attackDamage, card.pierce);
        //             projectile.transform.position = entity.transform.position;
        //         }
        //         else
        //         {
        //             List<Tile> tiles;
        //             if (card.attackType == AttackType.beam)
        //             {
        //                 tiles = board.GetTilesBeam(targetPosition, direction, card.attackWidth,
        //                     card.attackHeight);
        //             }
        //             else
        //             {
        //                 tiles = board.GetTilesSquare(targetPosition, card.spreadRange / 2);
        //             }
        //             foreach (var t in tiles)
        //                 {
        //                     t.ShowDebugColor(new Color(1f, 0, 0, 0.5f));
        //                     if (t.IsClear())
        //                         continue;
        //                     foreach (var e in t.GetEntities())
        //                     {
        //                         if (e.TryGetComponent(out HitHandler hitHandler))
        //                         {
        //                             hitHandler.Raise(this,new HitHandler.HitEventArgs(){dmg = 1});
        //                         }
        //                     }
        //                 }
        //         }
        //         
        //         if (card.attackSpread == AttackSpread.radial)
        //         {
        //             direction = (Direction) ((int) ++direction % spreadMax);
        //             if (direction == Direction.R)
        //             {
        //                 yield return new WaitForSeconds(0.75f);
        //             }
        //         }
        //     }
        //     yield break;
        // }
    }
}