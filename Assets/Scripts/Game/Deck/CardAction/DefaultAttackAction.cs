
using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Entity;
using Game.Player;
using Game.World;
using JetBrains.Annotations;
using UnityEngine;


[CreateAssetMenu(menuName = "CardAction/DefaultAttackAction")]
public class DefaultAttackAction :BaseCardAction
{
    [SerializeField] private PlayerProjectile projectilePrefab;
    public override bool Execute([CanBeNull] object caster, CardData card, Vector2Int targetPosition, out IEnumerator routine)
    {
        if (card is not MagicCardData || caster is not Slime)
        {
            routine = null;
            return false;
        }
        Slime slime = (Slime) caster;
        routine = DefualtCardCastRoutine(slime.GetComponent<Entity>(),(MagicCardData) card, targetPosition);
        return true;
    }
    private IEnumerator DefualtCardCastRoutine(Entity entity, MagicCardData card, Vector2Int targetPosition)
    {
        Board board = BattleManager.Instance.Board;
        entity.MoveToImmediate(targetPosition);
        yield return new WaitForSeconds(0.2f);
        Direction direction = Direction.R;
        int cnt = card.attackCount;
        int spreadMax = Math.Min(card.spreadRange,(int)Direction.MAX);
        while (cnt-- > 0)
        {
            if (card.attackType == AttackType.projectile)
            {
                
                PlayerProjectile projectile = Instantiate(projectilePrefab);
                projectile.Initialize(direction, card.attackDamage, card.pierce);
                projectile.transform.position = entity.transform.position;
            }
            else
            {
                List<Tile> tiles;
                if (card.attackType == AttackType.beam)
                {
                    tiles = board.GetTilesBeam(targetPosition, direction, card.attackWidth,
                        card.attackHeight);
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
                            if (e.TryGetComponent(out HitHandler hitHandler))
                            {
                                hitHandler.Raise(this,new HitHandler.HitEventArgs(){dmg = 1});
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
