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
        if (_entity.Position.x != 0)
        {
            _entity.MoveDirectionImmediate(Direction.L, 1);
        }
        else
        {
            GameManager.Instance.GateHP -= _dmg;
            Destroy(gameObject);
        }
    }

    public void OnHit(object caller, HitHandler.HitEventArgs e)
    {
        HP -= e.dmg;
        if (_hp <= 0)
        {
            Ondeath();
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
    }
}
