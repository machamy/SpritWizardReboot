using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CalendarUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private PopUpUIHandler popUpUIHandler;

    [SerializeField] private GameObject calendarPopUpUI;
    [SerializeField] private GameObject calendarInfoPopUpUI;
    [SerializeField] private Outline calendarButtonOutline;

    [Header("Week Seed")]
    [SerializeField] private Seed[] seeds;

    [Header("Seed Data")]
    [SerializeField] private SeedDataSO seedData;

    private void Start()
    {
        InitSeed();
    }

    public void InitSeed()
    {
        if (seedData.SeedData == null ||
            seedData.SeedData.Count == 0 ||
            seedData.SeedData.Count > seeds.Length) return;

        for (int i = 0; i < seedData.SeedData.Count; i++)
        {
            int dayIdx = (seedData.weekIdx > i) ? 6 : seedData.dayIdx;
            seeds[i].DisplaySeed(seedData.SeedData[i], dayIdx, seedData.seedIdx);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        calendarInfoPopUpUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        calendarInfoPopUpUI.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (calendarPopUpUI.activeSelf)
        {
            popUpUIHandler.ClosePopUp();
        }
        else
        {
            popUpUIHandler.OpenPopUp(calendarPopUpUI, calendarButtonOutline);
        }
    }
}
