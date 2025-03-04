using EventChannel;
using UnityEngine;
using UnityEngine.UI;

public class SeedAdder : MonoBehaviour
{
    [SerializeField] private NodePlanner nodePlanner;
    [SerializeField] private Seed[] seeds = new Seed[2];

    [Header("Objects")]
    [SerializeField] private GameObject selectSeedPanel;
    [SerializeField] private Button selectButton;

    //[SerializeField] private RewardManager rewardManager; // 테스트용

    [Header("Channel")]
    [SerializeField] private PhaseEventChannelSO startPhaseEvent;

    [Header("SeedData")]
    [SerializeField] private SeedDataSO seedData;

    [Header("Calendar")]
    [SerializeField] private CalendarUI calendarUI;

    private int selectedIdx;

    void Start()
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            int index = i;
            seeds[index].GetComponent<Button>().onClick.AddListener(() => SelectSeed(index));
        }

        selectButton.onClick.AddListener(AddSeed);
    }

    public void OpenSelectSeedPanel()
    {
        ShowTwoSeeds();
        selectSeedPanel.SetActive(true);
    }

    private void ShowTwoSeeds()
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            seeds[i].DisplaySeed(nodePlanner.GetWeekSeed());
        }
    }

    public void SelectSeed(int index)
    {
        selectButton.gameObject.SetActive(true);
        selectButton.transform.position = seeds[index].transform.position + Vector3.right * 600;
        selectedIdx = index;
    }

    public void AddSeed()
    {
        if (seedData.SeedData == null) seedData.ResetData();

        seedData.SeedData.Add(seeds[selectedIdx].SeedList);
        selectButton.gameObject.SetActive(false);
        selectSeedPanel.SetActive(false);
        calendarUI.InitSeed();
    }

    // TODO => 아래 부분들은 SeedManager로 빼야할듯

    public void StartPhase()
    {
        SeedType seed = GetSeed();
        if (seed == SeedType.None)
        {
            Debug.Log("일주일 시드 완료");
            return; // 보상 후 다음 시드를 선택할 수 있도록 설정
        }
        Debug.Log(seed);
        // startPhaseEvent.RaisePhaseEvent(seed); TODO => 씬 어떻게 쓸지 확실히 정한 후 Invoke타이밍 결정
    }

    public void PrintSeed() // 나중에 전투 종료후 InitReward을 실행시켜야 하므로 제거 해야함
    {
        //rewardManager.InitReward(GetSeed());
        StartPhase();
    }

    public SeedType GetSeed()
    {
        if (seedData.SeedData == null || seedData.SeedData.Count == 0) return SeedType.None;

        if (seedData.WeekSeed[seedData.dayIdx].Count == seedData.seedIdx)
        {
            GameManager.Instance.GateHP += 15; // 하루 종료

            if (seedData.WeekSeed.Count == seedData.dayIdx) return SeedType.None; // 일주일 종료
        }

        return seedData.WeekSeed[seedData.dayIdx][seedData.seedIdx];
    }

    public void AddIdxCount()
    {
        seedData.seedIdx++;

        if (seedData.WeekSeed[seedData.dayIdx].Count == seedData.seedIdx)
        {
            seedData.dayIdx++;
            seedData.seedIdx = 0;
        }

        calendarUI.InitSeed();
    }
}
