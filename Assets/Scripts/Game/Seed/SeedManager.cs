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

    public void PrintSeed()
    {
        rewardManager.InitReward(GetSeed());
    }

    public SeedType GetSeed()
    {
        if (seedData.daySeedData == null || seedData.daySeedData.Count == 0)
        {
            GameManager.Instance.GateHP += 15; // 하루 종료
            if (seedData.weekSeedData == null || seedData.weekSeedData.Count == 0) return SeedType.None; // 일주일 종료

            seedData.daySeedData = seedData.weekSeedData.Dequeue();
        }

        return seedData.daySeedData.Dequeue();
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
        seedData.weekSeedData = seed.SeedQueue;
        seedData.daySeedData = seedData.weekSeedData.Dequeue(); // 미리 하루치 빼두기
    }
}
