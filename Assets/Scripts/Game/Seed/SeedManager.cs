using EventChannel;
using UnityEngine;
using UnityEngine.UI;

public class SeedManager : MonoBehaviour
{
    [SerializeField] private NodePlanner nodePlanner;
    [SerializeField] private Seed[] seeds = new Seed[2];
    [SerializeField] private RewardManager rewardManager; // 테스트용

    [Header("Channel")]
    [SerializeField] private PhaseEventChannelSO startPhaseEvent;

    [Header("SeedData")]
    [SerializeField] private SeedDataSO seedData;

    void Start()
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            int index = i;
            seeds[index].GetComponent<Button>().onClick.AddListener(() => SelectSeed(seeds[index]));
        }
    }

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
        if (seedData.weekSeedData == null || seedData.weekSeedData.Count == seedData.weekIdx) return SeedType.None;

        if (seedData.weekSeedData[seedData.weekIdx].Count == seedData.dayIdx)
        {
            GameManager.Instance.GateHP += 15; // 하루 종료

            seedData.weekIdx++;
            seedData.dayIdx = 0;

            if (seedData.weekSeedData.Count == seedData.weekIdx) return SeedType.None; // 일주일 종료
        }

        return seedData.weekSeedData[seedData.weekIdx][seedData.dayIdx++];
    }

    public void ShowTwoSeeds()
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            seeds[i].DisplaySeed(nodePlanner.GetWeekSeed());
        }
    }

    public void SelectSeed(Seed seed)
    {
        seedData.ResetData();
        seedData.weekSeedData = seed.SeedList;
    }
}
