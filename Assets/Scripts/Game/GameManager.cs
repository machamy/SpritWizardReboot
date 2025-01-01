
using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;   
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private float MaxHP = 100;
    [SerializeField] private FloatVariableSO gateHP;
    
    [SerializeField] private GUIManager guiManager;
    public GUIManager GUIManager => guiManager;
    public float GateHP
    {
        get => gateHP.Value;
        set => gateHP.Value = value;
    }
    
    private void Start()
    {
        GateHP = MaxHP;
    }
}
