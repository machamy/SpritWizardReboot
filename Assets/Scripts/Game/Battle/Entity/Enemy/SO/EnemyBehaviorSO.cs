using UnityEngine;
using Game.Entity;

public abstract class EnemyBehaviourSO : ScriptableObject
{
    public string id;
    public EnemyBehaviour action;
    public int value;

    public abstract void Execute(Entity entity);
    
}