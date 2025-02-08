using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Game.Entity;
using DefaultNamespace;
using System.Linq;
using TMPro;
using EventChannel;
using UnityEngine.Serialization;
using UnityEngine.Animations;

[RequireComponent (typeof(Entity), typeof(HitHandler))]
public class Enemy : MonoBehaviour
{
    [Header("SO 선택")]
    [SerializeField]
    private EnemyDataSO _data;

    
    private string _name;
    [Header("적 정보")]
    public int _hp;
    public int _fullHp;

    private Queue<EnemyBehaviourSO> _behaviorQueue = new Queue<EnemyBehaviourSO>();
    [SerializeField]
    private List<EnemyPatternSO> _curPatternList = new List<EnemyPatternSO>();


    [SerializeField]
    private GameObject _queueObj;

    private Entity _entity;

    private Define.EnemyPhase phase = Define.EnemyPhase.FirstPhase;
    private Define.EnemyAttackMode mode = Define.EnemyAttackMode.Range;

    [SerializeField]
    private GameObject _behaviourPrefab;

    [Header("행동시 스프라이트 변경")]
    [SerializeField]
    private Sprite _rangeAttackSprite;
    [SerializeField]
    private Sprite _meleeAttackSprite;
    [SerializeField]
    private Sprite _restSprite;
    [SerializeField]
    private Sprite _moveSprite;

    private EnemyHpbarController _hpController;

    public Animator enemyAni;

    private SpriteRenderer sprite;
    private string spritePath = "Assets/Sprites/Enemy/";

    [FormerlySerializedAs("enemyTurnChannelSO")] public TurnEventChannelSO enemyTurnEnterChannelSO;
    [FormerlySerializedAs("playerTurnExitEvent")] [FormerlySerializedAs("enemyTurnEnterEvent")] public TurnEventChannelSO playerTurnExitEventSO;

    private void Start()
    {
        // 이름 체력 설정
        _name = _data.name;
        double maxhp = _data.hpMiddle + (_data.hpVariation / 2) + 0.5;
        double minhp = _data.hpMiddle - (_data.hpVariation / 2) + 0.5;
        _hp = Random.Range((int)maxhp, (int)minhp + 1);
        _fullHp = _hp;
        _entity = GetComponent<Entity>();
        _hpController = GetComponent<EnemyHpbarController>();

        // 스프라이트 변경
        sprite = GetComponent<SpriteRenderer>();
        string spriteFullPath = $"{spritePath + _data.spriteName}.png";
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spriteFullPath);
        if (texture != null)
        {
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            sprite.sprite = newSprite;
        }
        else
        {
            Debug.Log("이미지 불러오기 실패");
        }

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
        enemyTurnEnterChannelSO.OnTurnEventRaised += Execute;
        GetComponent<HitHandler>().Handler += OnHit;
    }
    private void OnDisable()
    {
        enemyTurnEnterChannelSO.OnTurnEventRaised -= Execute;
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
        enemyAni.SetTrigger("OnHit");
        _hp -= e.dmg;
        _hpController.UpdateHealthBar(_hp, _fullHp);
        if (_hp <= 0 && !_entity.IsDeath)
        {
            _entity.Delete();
            enemyTurnEnterChannelSO.OnTurnEventRaised += OnEnemyTurnEnter;
        }
        CheckState();
    }

    // private void Ondeath()
    // {
    //     
    //     //애니메이션 출력
    // }
    private void OnEnemyTurnEnter(int turn)
    {
        enemyTurnEnterChannelSO.OnTurnEventRaised -= OnEnemyTurnEnter;
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
    [ContextMenu("PlayAni")]
    public void PlayAni()
    {
        enemyAni.SetTrigger("OnHit");
    }

}
