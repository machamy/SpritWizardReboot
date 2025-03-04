using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seed : MonoBehaviour
{
    [Header("Seed Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hardSprite;
    [SerializeField] private Sprite eliteSprite;
    [SerializeField] private Sprite restSprite;
    [SerializeField] private Sprite storeSprite;
    [SerializeField] private Sprite bossSprite;

    private List<List<SeedType>> seedList;
    public List<List<SeedType>> SeedList => seedList;

    private Image[][] seedImages;

    private void InitImage()
    {
        seedImages = new Image[7][];

        for (int i = 0; i < transform.childCount; i++)
        {
            seedImages[i] = new Image[2];
            for (int j = 0; j < 2; j++)
            {
                seedImages[i][j] = transform.GetChild(i).GetChild(j*3).GetComponent<Image>();
            }
        }
    }

    public void DisplaySeed(int dayIdx, int seedIdx)
    {
        int idx = 0;
        foreach (List<SeedType> seedTypes in seedList)
        {
            Transform daySeed = transform.GetChild(idx);
            if (seedTypes.Count == 1)
            {
                for (int i = 1; i < 4; i++)
                {
                    daySeed.GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                for (int i = 1; i < 4; i++)
                {
                    daySeed.GetChild(i).gameObject.SetActive(true);
                }
            }

            for (int i = 0; i < seedTypes.Count; i++)
            {
                seedImages[idx][i].sprite = seedTypes[i] switch
                {
                    SeedType.normal => normalSprite,
                    SeedType.hard => hardSprite,
                    SeedType.elite => eliteSprite,
                    SeedType.rest => restSprite,
                    SeedType.store => storeSprite,
                    SeedType.boss => bossSprite,
                    _ => normalSprite
                };

                if (idx < dayIdx)
                {
                    seedImages[idx][i].transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (idx == dayIdx)
                {
                    if (i < seedIdx)
                    {
                        seedImages[idx][i].transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }

            idx++;
        }
    }

    public void DisplaySeed(List<List<SeedType>> seeds, int dayIdx = 0, int seedIdx = 0)
    {
        if (seedImages == null) InitImage();

        seedList = seeds;
        DisplaySeed(dayIdx, seedIdx);
    }
}
