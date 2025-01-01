using System;
using DefaultNamespace;
using UnityEngine;


public class GUIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI hpText;
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private TMPro.TextMeshProUGUI turnText;
    [Header("Event Channels")]
    [SerializeField] private EventChannel.TurnEventChannelSO playerTurnEnterEvent;
    [SerializeField] private EventChannel.TurnEventChannelSO enemyTurnEnterEvent;
    [SerializeField] private FloatVariableSO gateHP;


    private void OnEnable()
    {
        gateHP.OnValueChanged += OnGateHPChanged;
        playerTurnEnterEvent.OnTurnEventRaised += OnPlayerTurnEnter;
        enemyTurnEnterEvent.OnTurnEventRaised += OnEnemyTurnEnter;
    }
    private void OnGateHPChanged(float value)
    {
        hpText.text = $"Gate HP : {value}";
    }
    
    private void OnPlayerTurnEnter(int turn)
    {
        turnText.text = $"Player Turn {turn}";
    }
    private void OnEnemyTurnEnter(int turn)
    {
        timerText.text = $"Enemy Turn {turn}";
    }
    
    public void OnEnemyTurnTicking(float time)
    {
        timerText.text = $"Enemy Turn Remaining: {time}";
    }
}
