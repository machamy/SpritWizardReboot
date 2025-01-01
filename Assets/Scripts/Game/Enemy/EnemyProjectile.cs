using UnityEngine;
using Game.Entity;
using EventChannel;
public class EnemyProjectile : MonoBehaviour
{
    private Entity _entity;
    public TurnEventChannelSO enemyTurnEvent;
    public TurnEventChannelSO playerTurnExitEvent;
    public int _dmg { get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _entity = GetComponent<Entity>();
    }

    private void OnEnable()
    {
        enemyTurnEvent.OnTurnEventRaised += Execute;
    }
    private void OnDisable()
    {
        enemyTurnEvent.OnTurnEventRaised -= Execute;
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

    public void OnHit(int dmg)
    {
        playerTurnExitEvent.OnTurnEventRaised += OnPlayerturnEnd;
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
