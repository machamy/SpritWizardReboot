using UnityEngine;
using System.Collections.Generic;
using Game.Entity;
using DefaultNamespace;
using System.Linq;

[RequireComponent (typeof(Entity))]
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
    private GameObject _behaviorPrefab;
    [SerializeField]
    private EnemyDataSO _data;
    [SerializeField]
    private GameObject _queueObj;

    private Entity _entity;

    private Define.EnemyPhase phase = Define.EnemyPhase.FirstPhase;
    private Define.EnemyAttackMode mode = Define.EnemyAttackMode.Range;

    // 테스트 위한 임시 변수
    public int x = 15;
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
        Debug.Log("1페 원거리");


        CreateBehaviorList();
        Debug.Log("현재 행동 큐");
        foreach (EnemyBehaviourSO behaviour in _behaviorQueue)
        {
            Debug.Log(behaviour.name);
        }

    }
    [ContextMenu("Execute")]
    private void Execute()
    {
        // 해당하는 행동 수행
        //_behaviorQueue.Peek().Execute(_entity);
        Debug.Log(_behaviorQueue.Peek().name + " 실행");

        // 큐에서 삭제
        _behaviorQueue.Dequeue();

        // 큐 조정
        NextBehavior();
    }
    public void NextBehavior()
    {
        // 현재 상태 체크
        CheckState();


        Debug.Log("현재 행동 큐");
        foreach (EnemyBehaviourSO behaviour in _behaviorQueue)
        {
            Debug.Log(behaviour.name);
        }
    }

    // 몬스터 태세 변환
    private void CheckState()
    {
        // 1페 원거리 일때
        if (phase == Define.EnemyPhase.FirstPhase && mode == Define.EnemyAttackMode.Range)
        {
            // 1페 근거리
            if (x == 0)
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
                Debug.Log("1페 근거리");
                _behaviorQueue.Clear();
            }
            // 2페 원거리
            else if (_hp <= _fullHp * _data.phaseSwitchHpRatio / 100)
            {
                _curPatternList.Clear();
                _curPatternList = _data.patternList.Where(e => (e.phase == 2 && e.range != 0)).ToList();
                phase = Define.EnemyPhase.SecondPhase;
                Debug.Log("2페 원거리");
                _behaviorQueue.Clear();
            }
        }
        // 2페 근거리
        else if ((phase == Define.EnemyPhase.FirstPhase && mode == Define.EnemyAttackMode.Melee) ||
                    (phase == Define.EnemyPhase.SecondPhase && mode == Define.EnemyAttackMode.Range))
        {
            if ((_hp <= _fullHp * _data.phaseSwitchHpRatio / 100) && (x == 0))
            {
                _curPatternList.Clear();
                _curPatternList = _data.patternList.Where(e => (e.phase == 2 && e.range == 0)).ToList();
                phase = Define.EnemyPhase.SecondPhase;
                mode = Define.EnemyAttackMode.Melee;
                Debug.Log("2페 근거리");
                _behaviorQueue.Clear();
            }
        }
        // 상태에 맞는 행동 생성
        CreateBehaviorList();

    }

    private void CreateBehaviorList()
    {
        

        //큐에 3개 이상으로 들어있을때까지
        while (_behaviorQueue.Count < 3)
        {
            // 현재 State의 패턴중에 사정거리가 맞는 패턴들 선택
            var availiablePattern = _curPatternList.Where(e => e.range >= x);
            // 가중치 리스트
            int[] weightarray = new int[availiablePattern.ToArray().Length];
            // 가중치 선택
            int index = Utilities.WeightedRandom(weightarray);
            Debug.Log(_curPatternList[index] + " 패턴 선택");
            foreach (EnemyBehaviourSO behaviourElement in _curPatternList[index].actionSequence)
            {
                _behaviorQueue.Enqueue(behaviourElement);
            }
        }
    }
    public void OnHit(int dmg)
    {

        //TODO
        //몬스터 데미지 구현
    }


    // 테스트 위한 임시 코드들
    [ContextMenu ("Damage")]
    public void Damage()
    {
        _hp -= 4;
        Debug.Log(_hp);

        // 데미지 입었으면 상태 체크
        CheckState();
    }
    [ContextMenu("Move")]
    public void Move()
    {
        x -= 2;
        if (x <= 0)
        {
            x = 0;
        }
        CheckState();
    }
    [ContextMenu("MoveZero")]
    public void MoveZero()
    {
        x = 0;
        CheckState();
    }
    [ContextMenu("Reset")]
    public void Reset()
    {
        _hp = 20;
        x = 15;
        _behaviorQueue.Clear();
        // 1페 원거리로 설정
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
