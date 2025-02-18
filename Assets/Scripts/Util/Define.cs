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
        Damage,
        AttackCnt,
        MoveCnt,
        MAX
    }
    public enum CalculateType
    {
        add,
        sub,
        mul
    }
}
