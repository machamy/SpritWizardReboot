using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "SeedData", menuName = "SeedDataSO")]
public class SeedDataSO : ScriptableObject
{
    public List<List<List<SeedType>>> SeedData;
    public List<List<SeedType>> WeekSeed => SeedData[weekIdx];

    public int weekIdx;
    public int dayIdx;
    public int seedIdx;

    public void ResetData()
    {
        SeedData = new List<List<List<SeedType>>>();
        weekIdx = 0;
        dayIdx = 0;
        seedIdx = 0;

        Debug.Log("시드 데이터 제거 완료");
    }

    /// <summary>
    /// 시드 데이터 저장시 직렬화 작업
    /// </summary>
    public SerializableSeedData MakeSeedDataSerializable()
    {
        return new SerializableSeedData
        {
            SeedData = new List<List<List<SeedType>>>(SeedData),
            weekIdx = weekIdx,
            dayIdx = dayIdx,
            seedIdx = seedIdx
        };

    }

    /// <summary>
    /// 저장된 시드 데이터 불러오기
    /// </summary>
    public void LoadSeedDataFromList(SerializableSeedData seedListData)
    {
        SeedData = seedListData.SeedData;
        weekIdx = seedListData.weekIdx;
        dayIdx = seedListData.dayIdx;
        seedIdx = seedListData.seedIdx;
    }
}


[System.Serializable]
public class SerializableSeedData
{
    public List<List<List<SeedType>>> SeedData;
    public int weekIdx;
    public int dayIdx;
    public int seedIdx;
}
#if UNITY_EDITOR //TODO : Editor 디렉토리로 이동
[CustomEditor(typeof(SeedDataSO))]
public class SeedDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SeedDataSO seedData = (SeedDataSO)target;

        if (GUILayout.Button("Reset SeedData"))
        {
            seedData.ResetData();
            EditorUtility.SetDirty(seedData);
        }
    }
}
#endif