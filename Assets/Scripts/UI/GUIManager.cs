using System;
using DefaultNamespace;
using UnityEngine;


public class GUIManager : MonoBehaviour
{
    [Header("UI Elements (Battle)")]
    [SerializeField] private TMPro.TextMeshProUGUI hpText;
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private TMPro.TextMeshProUGUI turnText;
    [Header("UI Elements (Battle/Rune)")]
    [SerializeField] private TMPro.TextMeshProUGUI damageRunesText;
    [SerializeField] private TMPro.TextMeshProUGUI attackCountRunesText;
    [SerializeField] private TMPro.TextMeshProUGUI moveRunesText;
    [Header("UI Elements (Battle/Deck)")]
    [SerializeField] private TMPro.TextMeshProUGUI drawSizeText;
    [SerializeField] private TMPro.TextMeshProUGUI discardSizeText;
    [Header("Event Channels (Turn)")]
    [SerializeField] private EventChannel.TurnEventChannelSO playerTurnEnterEvent;
    [SerializeField] private EventChannel.TurnEventChannelSO enemyTurnEnterEvent;
    [Header("Variables")]
    [SerializeField] private FloatVariableSO gateHP;
    [Header("Variables(Read Only)")]
    [SerializeField] private IntVariableSO damageRune;
    [SerializeField] private IntVariableSO attackCountRune;
    [SerializeField] private IntVariableSO moveRune;
    [SerializeField] private IntVariableSO drawSize;
    [SerializeField] private IntVariableSO discardSize;

    private void OnEnable()
    {
        gateHP.OnValueChanged += OnGateHPChanged;
        playerTurnEnterEvent.OnTurnEventRaised += OnPlayerTurnEnter;
        enemyTurnEnterEvent.OnTurnEventRaised += OnEnemyTurnEnter;
        damageRune.OnValueChanged += OnDamageRunesChanged;
        attackCountRune.OnValueChanged += OnAttackCountRunesChanged;
        moveRune.OnValueChanged += OnMoveRunesChanged;
        drawSize.OnValueChanged += OnDrawSizeChanged;
        discardSize.OnValueChanged += OnDiscardSizeChanged;
    }
    
    private void OnDisable()
    {
        gateHP.OnValueChanged -= OnGateHPChanged;
        playerTurnEnterEvent.OnTurnEventRaised -= OnPlayerTurnEnter;
        enemyTurnEnterEvent.OnTurnEventRaised -= OnEnemyTurnEnter;
        damageRune.OnValueChanged -= OnDamageRunesChanged;
        attackCountRune.OnValueChanged -= OnAttackCountRunesChanged;
        moveRune.OnValueChanged -= OnMoveRunesChanged;
        drawSize.OnValueChanged -= OnDrawSizeChanged;
        discardSize.OnValueChanged -= OnDiscardSizeChanged;
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
    
    private void OnDrawSizeChanged(int value)
    {
        drawSizeText.text = $"Draw deck : {value}";
    }
    
    private void OnDiscardSizeChanged(int value)
    {
        discardSizeText.text = $"Discard deck : {value}";
    }
}
