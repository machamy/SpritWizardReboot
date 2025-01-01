using UnityEngine;
using Game.Entity;
public abstract class EnemyBehaviourSO : ScriptableObject
{
    public int id;
    public int value;

    public abstract void Execute(Entity entity);
    
}