using UnityEngine;
using System.Collections.Generic;
using Game.Entity;
using DefaultNamespace;
using System.Linq;
using TMPro;
using EventChannel;

[RequireComponent (typeof(Entity), typeof(HitHandler))]
public class Enemy : MonoBehaviour
{
    private string _name;
    [SerializeField]
    private int _hp;
    private int _fullHp;

    private Queue<EnemyBehaviourSO> _behaviorQueue = new Queue<EnemyBehaviourSO>();
    [SerializeField]
    private List<EnemyPatternSO> _curPatternList = new List<EnemyPatternSO>();

    [SerializeField]
    private EnemyDataSO _data;
    [SerializeField]
    private GameObject _queueObj;

    private Entity _entity;

    private Define.EnemyPhase phase = Define.EnemyPhase.FirstPhase;
    private Define.EnemyAttackMode mode = Define.EnemyAttackMode.Range;

    [SerializeField]
    private GameObject _behaviourPrefab;

    [SerializeField]
    private Sprite _rangeAttackSprite;
    [SerializeField]
    private Sprite _meleeAttackSprite;
    [SerializeField]
    private Sprite _restSprite;
    [SerializeField]
    private Sprite _moveSprite;


    public TurnEventChannelSO enemyTurnChannelSO;
    public TurnEventChannelSO playerTurnExitEvent;

    private void Start()
    {
        // 이름 체력 설정
        _name = _data.name;
        double maxhp = _data.hpMiddle + (_data.hpVariation / 2) + 0.5;
        double minhp = _data.hpMiddle - (_data.hpVariation / 2) + 0.5;
        _hp = Random.Range((int)maxhp, (int)minhp + 1);
        _fullHp = _hp;
        _entity = GetComponent<Entity>();


        // 시작 패턴 설정할것
        foreach (EnemyBehaviourSO element in _data.initPattern.actionSequence)
        {
            _behaviorQueue.Enqueue(element);
        }

        // 1페 원거리로 설정
        _curPatternList = _data.patternList.Where(e => (e.phase == 1 && e.range != 0)).ToList();
        CreateBehaviorList();

        AllBehaviourQueueUI();

    }
    [ContextMenu("Execute")]
    private void Execute(int turn)
    {
        // 해당하는 행동 수행
        _behaviorQueue.Peek().Execute(_entity);
        Debug.Log(_behaviorQueue.Peek().name + " 실행");

        // 큐에서 삭제
        _behaviorQueue.Dequeue();

        // 큐 조정
        NextBehavior();
    }

    private void OnEnable()
    {
        enemyTurnChannelSO.OnTurnEventRaised += Execute;
        GetComponent<HitHandler>().Handler += OnHit;
    }
    private void OnDisable()
    {
        enemyTurnChannelSO.OnTurnEventRaised -= Execute;
    }
    private void AllBehaviourQueueUI()
    {
        foreach (Transform child in _queueObj.transform)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (EnemyBehaviourSO e in _behaviorQueue)
        {
            GameObject obj;
            obj = Instantiate(_behaviourPrefab, _queueObj.transform);
            BehaviourPrefabSetting(e, obj);
            i++;
            if (i >= 3)
            {
                break;
            }
        }
    }
    private void BehaviourPrefabSetting(EnemyBehaviourSO behaviour, GameObject obj)
    {
        switch (behaviour.action)
        {
            case EnemyBehaviour.RangeAttack:
                obj.GetComponent<SpriteRenderer>().sprite = _rangeAttackSprite;
                obj.transform.GetComponentInChildren<TextMeshPro>().text = behaviour.value.ToString();
                break;
            case EnemyBehaviour.MeleeAttack:
                obj.GetComponent<SpriteRenderer>().sprite = _meleeAttackSprite;
                obj.transform.GetComponentInChildren<TextMeshPro>().text = behaviour.value.ToString();
                break;
            case EnemyBehaviour.Move:
                obj.GetComponent<SpriteRenderer>().sprite = _moveSprite;
                obj.transform.GetComponentInChildren<TextMeshPro>().text = behaviour.value.ToString();
                break;
            case EnemyBehaviour.Rest:
                obj.GetComponent<SpriteRenderer>().sprite = _restSprite;
                obj.transform.GetComponentInChildren<TextMeshPro>().text = "";
                break;
        }
    }
    public void NextBehavior()
    {
        // 현재 상태 체크
        CheckState();


        //Debug.Log("현재 행동 큐");
        //foreach (EnemyBehaviourSO behaviour in _behaviorQueue)
        //{
        //    Debug.Log(behaviour.name);
        //}
    }

    // 몬스터 태세 변환
    private void CheckState()
    {
        // 1페 원거리 일때
        if (phase == Define.EnemyPhase.FirstPhase && mode == Define.EnemyAttackMode.Range)
        {
            // 1페 근거리
            if (_entity.Coordinate.x == 0)
            {
                _curPatternList.Clear();
                foreach (EnemyPatternSO element in _data.patternList)
                {
                    if (element.phase == 1 && element.range == 0)
                    {
                        _curPatternList.Add(element);
                    }
                }
                mode = Define.EnemyAttackMode.Melee;
                _behaviorQueue.Clear();
            }
            // 2페 원거리
            else if (_hp <= _fullHp * _data.phaseSwitchHpRatio / 100)
            {
                _curPatternList.Clear();
                _curPatternList = _data.patternList.Where(e => (e.phase == 2 && e.range != 0)).ToList();
                phase = Define.EnemyPhase.SecondPhase;
                _behaviorQueue.Clear();
            }
        }
        // 2페 근거리
        else if ((phase == Define.EnemyPhase.FirstPhase && mode == Define.EnemyAttackMode.Melee) ||
                    (phase == Define.EnemyPhase.SecondPhase && mode == Define.EnemyAttackMode.Range))
        {
            if ((_hp <= _fullHp * _data.phaseSwitchHpRatio / 100) && (_entity.Coordinate.x == 0))
            {
                _curPatternList.Clear();
                _curPatternList = _data.patternList.Where(e => (e.phase == 2 && e.range == 0)).ToList();
                phase = Define.EnemyPhase.SecondPhase;
                mode = Define.EnemyAttackMode.Melee;
                _behaviorQueue.Clear();
            }
        }
        // 상태에 맞는 행동 생성
        CreateBehaviorList();
        // 행동 프리팹 생성
        AllBehaviourQueueUI();

    }

    private void CreateBehaviorList()
    {
        

        //큐에 3개 이상으로 들어있을때까지
        while (_behaviorQueue.Count < 3)
        {
            // 현재 State의 패턴중에 사정거리가 맞는 패턴들 선택
            var availiablePattern = _curPatternList.Where(e => e.range >= _entity.Coordinate.x);
            // 가중치 리스트
            int[] weightarray = new int[availiablePattern.ToArray().Length];
            // 가중치 선택
            int index = Utilities.WeightedRandom(weightarray);
            foreach (EnemyBehaviourSO behaviourElement in _curPatternList[index].actionSequence)
            {
                _behaviorQueue.Enqueue(behaviourElement);
            }
        }
    }
    public void OnHit(object caller, HitHandler.HitEventArgs e)
    {

        //TODO
        //몬스터 데미지 구현
        _hp -= e.dmg;
        if (_hp <= 0)
        {
            Ondeath();
            playerTurnExitEvent.OnTurnEventRaised += OnPlayerturnEnd;
        }
        CheckState();
    }

    private void Ondeath()
    {
        
        //애니메이션 출력
    }
    private void OnPlayerturnEnd(int turn)
    {
        playerTurnExitEvent.OnTurnEventRaised -= OnPlayerturnEnd;
        if (gameObject != null) 
            Destroy(gameObject);
    }
    [ContextMenu("Reset")]
    public void Reset()
    {
        _hp = 20;
        _behaviorQueue.Clear();
        _curPatternList = _data.patternList.Where(e => (e.phase == 1 && e.range != 0)).ToList();
        phase = Define.EnemyPhase.FirstPhase;
        mode = Define.EnemyAttackMode.Range;
        Debug.Log("1페 원거리");
        CreateBehaviorList();
    }
    [ContextMenu ("ShowQueue")]
    public void QueueList()
    {
        if (_behaviorQueue.Count == 0)
        {
            Debug.Log("큐가 비어있음"); 
        }
        else
        {
            foreach (EnemyBehaviourSO behaviour in _behaviorQueue)
            {
                Debug.Log(behaviour.name);
            }
        }
    }
}
