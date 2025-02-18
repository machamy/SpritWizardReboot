using System;
using DefaultNamespace;
using EventChannel;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    #region Nested Event Channel



    #endregion

    #region Inspector Fields

    [Header("Channels")]
    [SerializeField] private GUIEventChannelSO guiEventChannel;

    [Header("UI Elements - Battle")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TurnIndicatingSlider turnSlider;

    [Header("UI Elements - Rune")]
    [SerializeField] private RunesHUD runesHUD;

    [Header("UI Elements - Deck")]
    [SerializeField] private TextMeshProUGUI drawSizeText;
    [SerializeField] private TextMeshProUGUI discardSizeText;

    [Header("Event Channels - Turn")]
    [SerializeField] private EventChannel.TurnEventChannelSO playerTurnEnterEvent;
    [SerializeField] private EventChannel.TurnEventChannelSO enemyTurnEnterEvent;

    [Header("Variables")]
    [SerializeField] private FloatVariableSO gateHP;

    [Header("Read Only Variables")]
    [SerializeField] private IntVariableSO damageRune;
    [SerializeField] private IntVariableSO attackCountRune;
    [SerializeField] private IntVariableSO moveRune;
    [SerializeField] private IntVariableSO drawSize;
    [SerializeField] private IntVariableSO discardSize;

    #endregion

    #region Unity Callbacks

    private void OnEnable()
    {
        // Subscribe to battle UI value changes
        gateHP.OnValueChanged += OnGateHPChanged;
        drawSize.OnValueChanged += OnDrawSizeChanged;
        discardSize.OnValueChanged += OnDiscardSizeChanged;

        // Subscribe to GUI event channel for rune updates
        if(guiEventChannel != null)
        {
            guiEventChannel.OnUpdateRune += HandleUpdateRune;
            guiEventChannel.OnUpdateTempRune += HandleUpdateTempRune;
        }

        // playerTurnEnterEvent.OnTurnEventRaised += OnPlayerTurnEnter;
        // enemyTurnEnterEvent.OnTurnEventRaised += OnEnemyTurnEnter;
        // damageRune.OnValueChanged += OnDamageRuneChanged;
        // attackCountRune.OnValueChanged += OnAttackCountRuneChanged;
        // moveRune.OnValueChanged += OnMoveRuneChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe from battle UI value changes
        gateHP.OnValueChanged -= OnGateHPChanged;
        drawSize.OnValueChanged -= OnDrawSizeChanged;
        discardSize.OnValueChanged -= OnDiscardSizeChanged;

        // Unsubscribe from GUI event channel events
        if(guiEventChannel != null)
        {
            guiEventChannel.OnUpdateRune -= HandleUpdateRune;
            guiEventChannel.OnUpdateTempRune -= HandleUpdateTempRune;
        }
        
        // playerTurnEnterEvent.OnTurnEventRaised -= OnPlayerTurnEnter;
        // enemyTurnEnterEvent.OnTurnEventRaised -= OnEnemyTurnEnter;
        // damageRune.OnValueChanged -= OnDamageRuneChanged;
        // attackCountRune.OnValueChanged -= OnAttackCountRuneChanged;
        // moveRune.OnValueChanged -= OnMoveRuneChanged;
    }

    #endregion

    // 직접 변경하는거... 수정 필요
    #region UI Update Methods 

    private void OnGateHPChanged(float value)
    {
        hpText.text = $"Gate HP : {value}";
    }

    private void OnDrawSizeChanged(int value)
    {
        drawSizeText.text = $"Draw deck : {value}";
    }

    private void OnDiscardSizeChanged(int value)
    {
        discardSizeText.text = $"Discard deck : {value}";
    }

    public void OnEnemyTurnTicking(float time, float maxTime)
    {
        turnSlider.SetValue(time / maxTime);
    }
    
    // private void OnDamageRuneChanged(int value) { runesHUD.UpdateRune(Define.RuneEffectType.Damage, value); }
    // private void OnAttackCountRuneChanged(int value) { runesHUD.UpdateRune(Define.RuneEffectType.AttackCnt, value); }
    // private void OnMoveRuneChanged(int value) { runesHUD.UpdateRune(Define.RuneEffectType.MoveCnt, value); }

    #endregion

    #region Rune Event Handlers

    private void HandleUpdateRune(Define.RuneEffectType runeType, int value)
    {
        runesHUD.UpdateRune(runeType, value);
    }

    private void HandleUpdateTempRune(Define.RuneEffectType runeType, int value)
    {
        runesHUD.UpdateTempRune(runeType, value);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 다른 시스템(예: 카드)이 직접 임시 룬 값을 업데이트할 경우 호출
    /// </summary>
    public void SetTempRune(Define.RuneEffectType runeType, int value)
    {
        runesHUD.UpdateTempRune(runeType, value);
    }

    /// <summary>
    /// RunesHUD 전체를 갱신
    /// </summary>
    public void UpdateRunesHUD()
    {
        runesHUD.UpdateRunesHUD();
    }

    #endregion
}
