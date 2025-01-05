using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private Queue<Queue<SeedType>> seedQueue;
    public Queue<Queue<SeedType>> SeedQueue { get => seedQueue; set => seedQueue = value; }

    private TextMeshProUGUI[] seedTexts;

    private void Awake()
    {
        seedTexts = new TextMeshProUGUI[7];
        for (int i = 0; i < seedTexts.Length; i++)
        {
            seedTexts[i] = transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
        }
    }

    public void DisplaySeed()
    {
        int idx = 0;
        foreach (Queue<SeedType> seedTypes in seedQueue)
        {
            seedTexts[idx].text = "";
            foreach (SeedType seed in seedTypes)
            {
                seedTexts[idx].text += seed.ToString() + "\n";
            }
            idx++;
        }
    }
}
