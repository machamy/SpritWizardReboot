
using UnityEngine;

[CreateAssetMenu(fileName = "CardDisplaySetting", menuName = "Card/Display Setting")]
public class CardDisplaySettingSO : ScriptableObject
{
    [Header("Focus Animation")]
    public float focusedScale = 1.2f;
    public float focusDuration = 0.2f;
    public float unfocusedScale = 1f;
    public float unfocusDuration = 0.2f;

    [Header("Drag Animation")]
    public float dragScale = 1.5f;
    public float dragScaleDuration = 0.2f;
    public float dragFollowSpeed = 10f;
    public float dragReturnDuration = 0.2f;
    public float dragMaxHeightCoefficient = 0.5f;

    [Header("Decay Animation")]
    public float drageDcayHeightStartCoefficient = 0.5f;
    public float dragDecayHeightMaxCoeefcient = 0.75f;
    public float dragDecayScale = 0.5f;
    public float decayDuration = 0.5f;

    [Header("Tile Highlight")]
    public Color tileFocusOkColor = new Color(0.4f, 1f, 0f, 0.6f);
    public Color tileFocusNoColor = new Color(1f, 0f, 0f, 0.6f);
}
