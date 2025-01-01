using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private string _name;
    private int _hp;
    private Queue<EnemyBehavior> _behaviorQueue = new Queue<EnemyBehavior>();

    [SerializeField]
    private GameObject _behaviorPrefab;
    [SerializeField]
    private GameObject _queueObj;
    //몬스터 위치 변수

    private void Start()
    {
        NextBehaviorUI();
    }
    private void NextBehaviorUI()
    {
        GameObject obj = Instantiate(_behaviorPrefab, _queueObj.transform);
    }
}
