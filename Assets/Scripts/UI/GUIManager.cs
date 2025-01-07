using System;
using DefaultNamespace;
using UnityEngine;


public class GUIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI hpText;
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private TMPro.TextMeshProUGUI turnText;
    [SerializeField] private TMPro.TextMeshProUGUI damageRunesText;
    [SerializeField] private TMPro.TextMeshProUGUI attackCountRunesText;
    [SerializeField] private TMPro.TextMeshProUGUI moveRunesText;
    [Header("Event Channels")]
    [SerializeField] private EventChannel.TurnEventChannelSO playerTurnEnterEvent;
    [SerializeField] private EventChannel.TurnEventChannelSO enemyTurnEnterEvent;
    [SerializeField] private FloatVariableSO gateHP;
    [SerializeField] private IntVariableSO damageRune;
    [SerializeField] private IntVariableSO attackCountRune;
    [SerializeField] private IntVariableSO moveRune;

    private void OnEnable()
    {
        gateHP.OnValueChanged += OnGateHPChanged;
        playerTurnEnterEvent.OnTurnEventRaised += OnPlayerTurnEnter;
        enemyTurnEnterEvent.OnTurnEventRaised += OnEnemyTurnEnter;
        damageRune.OnValueChanged += OnDamageRunesChanged;
        attackCountRune.OnValueChanged += OnAttackCountRunesChanged;
        moveRune.OnValueChanged += OnMoveRunesChanged;
    }
    
    private void OnDisable()
    {
        gateHP.OnValueChanged -= OnGateHPChanged;
        playerTurnEnterEvent.OnTurnEventRaised -= OnPlayerTurnEnter;
        enemyTurnEnterEvent.OnTurnEventRaised -= OnEnemyTurnEnter;
        damageRune.OnValueChanged -= OnDamageRunesChanged;
        attackCountRune.OnValueChanged -= OnAttackCountRunesChanged;
        moveRune.OnValueChanged -= OnMoveRunesChanged;
    }
    
    
    private void OnDamageRunesChanged(int value)
    {
      damageRunesText.text = $"Damage Rune : {value}";
        
    }
    private void OnAttackCountRunesChanged(int value)
    {
        attackCountRunesText.text = $"Attack Count Rune : {value}";
    }
    private void OnMoveRunesChanged(int value)
    {
        moveRunesText.text = $"Move Rune : {value}";
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
