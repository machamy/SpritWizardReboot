using System.Collections.Generic;
using UnityEngine;

public class NodePlanner : MonoBehaviour
{
    enum Week { 월, 화, 수, 목, 금, 토, 일 }
    private void Start()
    {
        /* 테스트용
        int i = 0;
        Queue<Queue<SeedType>> seed = GetWeekSeed();
        while (seed.Count > 0)
        {
            int j = 0;
            Queue<SeedType> s = seed.Dequeue();
            while (s.Count > 0)
            {
                Debug.Log((Week)i + "요일 " + (j + 1) + "번째 페이지 : " +s.Dequeue());
                j++;
            }
            i++;
        }
        */
    }

    /// <summary>
    /// 일주일치 시드 반환
    /// </summary>
    public List<List<SeedType>> GetWeekSeed()
    {
        List<List<SeedType>> weekSeed = new List<List<SeedType>>();
        for (int i = 0; i < 7; i++)
        {
            weekSeed.Add(new List<SeedType>());
        }

        int ran = 2; // 난수 저장용 변수

        // 월요일
        weekSeed[0].Add(SeedType.normal); // 노말 1개

        // 화요일
        for (int i = 0; i < Random.Range(1, 3); i++) // 노말 1개 + 50% 확률로 하드 1개
        {
            weekSeed[1].Add((SeedType)(i+1));
        }

        // 수, 목, 금, 토요일
        List<int> nodeCnt = DistributeNodesPerDay(); // 수, 목, 금요일 시드 개수
        nodeCnt.Add(2); // 토요일 시드 개수 추가
        List<SeedType>[] seeds = new List<SeedType>[4]; // 수, 목, 금, 토요일 시드
        int extraRest = 0; // 추가 휴식 위치 확인용 비트 마스크 -> 000
        
        for (int i = 0; i < nodeCnt.Count; i++)
        {
            seeds[i] = new List<SeedType>(nodeCnt[i]); // 각 노드 개수만큼 할당
            for (int j = 0; j < nodeCnt[i]; j++)
            {
                seeds[i].Add(SeedType.None);
            }
        }

        int remain = 0; // 남은 노드 개수
        foreach (int n in nodeCnt) remain += n;

        seeds[3][1] = SeedType.store; // 토요일 두번째 노드는 상점
        remain--;


        // 30% 확률로 중간 상점 존재
        if (Random.Range(0, 10) < 3)
        {
            ran = Random.Range(0, 2); // 수요일 or 목요일
            seeds[ran][Random.Range(0, nodeCnt[ran])] = SeedType.store; // 해당 요일의 랜덤 노드에 상점
            extraRest |= 1 << ran;
            remain--;
            
            ran = Random.Range(0, 2) < 1 ? 1 - ran : 2; // 상점 O -> 나머지 두 요일중 하나에 휴식 존재
        }
        else
        {
            ran = Random.Range(0, 3); // 상점 X -> 세 요일중 하나에 휴식 존재
        }


        // 상점 없는 요일 휴식 추가
        seeds[ran][Random.Range(0, nodeCnt[ran])] = SeedType.rest;
        extraRest |= 1 << ran;
        remain--;


        // 30% 확률로 휴식 추가 존재
        if (Random.Range(0, 10) < 3)
        {
            List<int> unUsed = new List<int>(); // 상점 or 휴식이 없는 요일

            for (int i = 0; i < 3; i++)
            {
                if ((extraRest & (1 << i)) == 0) unUsed.Add(i);
            }
            
            ran = Random.Range(0, unUsed.Count);
            seeds[unUsed[ran]][Random.Range(0, nodeCnt[unUsed[ran]])] = SeedType.rest;
            remain--;
        }

        // 엘리트 몹 추가 -> 나머지는 일반으로
        int elite = Random.Range(2, 10) / 3 + 1; // 엘리트 수 : 1/8 -> 1, 3/8 -> 2, 3/8 -> 3, 1/8 -> 4
        SeedType[] remainArr = new SeedType[remain];
        
        for (int i = 0; i < remain; i++)
        {
            remainArr[i] = SeedType.elite;
        }

        int idx = 0;
        if (elite < remain)
        {
            idx = 0;
            while (idx < remain - elite)
            {
                ran = Random.Range(0, remain);
                if (remainArr[ran] == SeedType.elite)
                {
                    remainArr[ran] = SeedType.hard;
                    idx++;
                }
            }
        }

        idx = 0;
        for (int i = 0; i < nodeCnt.Count; i++)
        {
            for (int j = 0; j < nodeCnt[i]; j++)
            {
                if (seeds[i][j] == SeedType.None)
                {
                    seeds[i][j] = remainArr[idx];
                    idx++;
                }
            }
        }

        for (int i = 0; i < nodeCnt.Count; i++)
        {
            weekSeed[i + 2] = seeds[i];
        }

        // 일요일
        weekSeed[6].Add(SeedType.boss);

        return weekSeed;
    }
    
    /// <summary>
    /// 수, 목, 금요일의 노드 개수를 짜는 알고리즘
    /// </summary>
    private List<int> DistributeNodesPerDay()
    {
        List<int> nodeCnt = new List<int> { 2, 2, 2 };

        switch (Random.Range(0, 4))
        {
            case 0: // 1개 노드가 0개 -> 모두 2개의 노드
                break;
            case 1: // 1개 노드가 1개 -> 노드 중 하나만 1개
            case 2:
                nodeCnt[Random.Range(0, 3)] = 1;
                break;
            case 3: // 1개 노드가 2개 -> 노드 중 하나만 2개
                nodeCnt = new List<int> { 1, 1, 1 };
                nodeCnt[Random.Range(0, 3)] = 2;
                break;
        }

        return nodeCnt;
    }
}
