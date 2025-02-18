using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private List<List<SeedType>> seedList;
    public List<List<SeedType>> SeedList => seedList;

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
        foreach (List<SeedType> seedTypes in seedList)
        {
            seedTexts[idx].text = "";
            foreach (SeedType seed in seedTypes)
            {
                seedTexts[idx].text += seed.ToString() + "\n";
            }
            idx++;
        }
    }

    public void DisplaySeed(List<List<SeedType>> seeds)
    {
        seedList = seeds;
        DisplaySeed();
    }
}
