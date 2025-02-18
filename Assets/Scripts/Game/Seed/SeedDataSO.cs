using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "SeedData", menuName = "SeedDataSO")]
public class SeedDataSO : ScriptableObject
{
    public List<List<SeedType>> weekSeedData;

    public int weekIdx;
    public int dayIdx;

    public void ResetData()
    {
        weekSeedData = new List<List<SeedType>>();
        weekIdx = 0;
        dayIdx = 0;

        Debug.Log("시드 데이터 제거 완료");
    }

    /// <summary>
    /// 시드 데이터 저장시 직렬화 작업
    /// </summary>
    public SerializableSeedData MakeSeedDataSerializable()
    {
        return new SerializableSeedData
        {
            weekSeedData = new List<List<SeedType>>(weekSeedData),
            weekIdx = weekIdx,
            dayIdx = dayIdx
        };

    }

    /// <summary>
    /// 저장된 시드 데이터 불러오기
    /// </summary>
    public void LoadSeedDataFromList(SerializableSeedData seedListData)
    {
        weekSeedData = seedListData.weekSeedData;
        weekIdx = seedListData.weekIdx;
        dayIdx = seedListData.dayIdx;
    }
}


[System.Serializable]
public class SerializableSeedData
{
    public List<List<SeedType>> weekSeedData;
    public int weekIdx;
    public int dayIdx;
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