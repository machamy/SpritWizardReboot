using UnityEngine;

public static class Define
{
    public enum EnemyPhase
    {
        FirstPhase,
        SecondPhase
    }
    public enum EnemyAttackMode
    {
        Range,
        Melee
    }
    public enum RuneEffectType
    {
        damage,
        attackCnt,
        moveCnt,
        MAX
    }
    public enum CalculateType
    {
        add,
        sub,
        mul
    }
}
