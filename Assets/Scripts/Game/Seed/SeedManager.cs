using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeedManager : MonoBehaviour
{
    [SerializeField] private NodePlanner nodePlanner;
    [SerializeField] private Seed[] seeds;

    private Queue<Queue<SeedType>> weekSeed;
    private Queue<SeedType> daySeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            int index = i;
            seeds[index].GetComponent<Button>().onClick.AddListener(() => SelectSeed(seeds[index]));
        }
    }

    public void PrintSeed()
    {
        Debug.Log(GetSeed());
    }

    public SeedType GetSeed()
    {
        if (daySeed == null || daySeed.Count == 0)
        {
            if (weekSeed == null || weekSeed.Count == 0) return SeedType.None; // 일주일 종료

            daySeed = weekSeed.Dequeue();
        }

        return daySeed.Dequeue();
    }

    public void ShowTwoSeeds()
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            seeds[i].SeedQueue = nodePlanner.GetWeekSeed();
            seeds[i].DisplaySeed();
        }
    }

    public void SelectSeed(Seed seed)
    {
        weekSeed = seed.SeedQueue;
    }
}
