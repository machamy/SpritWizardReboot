using System;
using UnityEngine;
using Game.Entity;
using EventChannel;



[RequireComponent(typeof(HitHandler))]
public class EnemyProjectile : MonoBehaviour
{
    private Entity _entity;
    public TurnEventChannelSO enemyTurnEvent;
    public TurnEventChannelSO playerTurnExitEvent;
    private int _hp = 1;
    public int HP
    {
        get => _hp;
        set => _hp = value;
    }
    public int _dmg { get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _entity = GetComponent<Entity>();
    }

    private void OnEnable()
    {
        enemyTurnEvent.OnTurnEventRaised += Execute;
        GetComponent<HitHandler>().Handler += OnHit;
    }
    private void OnDisable()
    {
        enemyTurnEvent.OnTurnEventRaised -= Execute;
        GetComponent<HitHandler>().Handler -= OnHit;
    }

    private void Execute(int turn)
    {
        if (_entity.Coordinate.x != 0)
        {
            _entity.MoveDirectionAnimated(Direction.L, 1,1f);
        }
        else
        {
            GameManager.Instance.GateHP -= _dmg;
            playerTurnExitEvent.OnTurnEventRaised += OnPlayerturnEnd;
            _entity.Delete();
        }
    }

    public void OnHit(object caller, HitHandler.HitEventArgs e)
    {
        HP -= e.dmg;
        if (_hp <= 0 && !_entity.IsDeath)
        {
            Ondeath();
            _entity.Delete();
            playerTurnExitEvent.OnTurnEventRaised += OnPlayerturnEnd;
        }
    }
    private void OnPlayerturnEnd(int turn)
    {
        playerTurnExitEvent.OnTurnEventRaised -= OnPlayerturnEnd;
        Destroy(gameObject);
    }
    private void Ondeath()
    {
        //애니메이션 출력
        //
        // Destroy(gameObject);
        
        
        //TODO Destory 가능하도록 설정, Destory를 여기서 하면 안됨!!!
    }

    private void OnDestroy()
    {
        playerTurnExitEvent.OnTurnEventRaised -= OnPlayerturnEnd;
    }
}
