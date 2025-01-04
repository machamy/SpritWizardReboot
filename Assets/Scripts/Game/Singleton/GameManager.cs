
using System;
using DefaultNamespace;
using Game;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    
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
