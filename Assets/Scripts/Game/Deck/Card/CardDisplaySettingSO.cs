
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardDisplaySetting", menuName = "Card/Display Setting")]
public class CardDisplaySettingSO : ScriptableObject
{
    [Header("Focus Animation")]
    public float focusedScale = 1.2f;
    public float focusDuration = 0.2f;
    public bool focusCardVisible = true;
    public float unfocusedScale = 1f;
    public float unfocusDuration = 0.2f;

    [Header("Drag Animation")]
    [Tooltip("드래그시 크기")]public float dragScale = 1.5f;
    [Tooltip("크기 변화 시간")]public float dragScaleDuration = 0.2f;
    [Tooltip("카드 따라오는 속도")]public float dragFollowSpeed = 10f;
    // [Tooltip("카드 따라온 기울임 정도")]public float followTiltAmount = 10f;
    // public float followTiltSpeed = 20f;
    [Tooltip("카드 돌아가는 속도")]public float dragReturnDuration = 0.2f;
    [Tooltip("최대 드래그 가능한 상대 위치")]public float dragMaxHeightCoefficient = 0.5f;

    [FormerlySerializedAs("drageDcayHeightStartCoefficient")] [Header("Decay Animation")]
    [Tooltip("해당 상대 위치부터 사라지기 시작")]public float drageDecayHeightStartCoefficient = 0.5f;
    [Tooltip("해당 상대 위치에 dragDecayScale 만큼 사라짐")]public float dragDecayHeightMaxCoeefcient = 0.75f;
    public float dragDecayScale = 0.5f;
    [Tooltip("사용시, 사라지는 시간")]public float decayDuration = 0.5f;

    [Header("Tile Highlight")]
    public Color tileFocusOkColor = new Color(0.4f, 1f, 0f, 0.6f);
    public Color tileFocusNoColor = new Color(1f, 0f, 0f, 0.6f);
}
