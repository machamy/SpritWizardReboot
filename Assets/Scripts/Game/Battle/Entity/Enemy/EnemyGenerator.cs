using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class EnemyGenerator : MonoBehaviour
{
    private string enemySpritePath = "Assets/Sprites/Enemy/";

    private string enemyDataSOPath = "Assets/ScriptableObjects/EnemyData/EnemyDataSO/";
    private string enemyActionSOPath = "Assets/ScriptableObjects/EnemyData/EnemyActionSO/";
    private string enemyPatternSOPath = "Assets/ScriptableObjects/EnemyData/EnemyPatternSO/";

    [ContextMenu("Enemy Generate")]
    public void EnemyDataGenerate()
    {
        //foreach(var action in Database.AllEnemyActionData)
        //{
        //    switch (action.action)
        //    {
        //        case EnemyBehaviour.RangeAttack:
        //            EnemyAttackSO rangeAttackSO = new();
        //            rangeAttackSO.id = action.id;
        //            rangeAttackSO.action = action.action;
        //            rangeAttackSO.value = action.value;
        //            AssetDatabase.CreateAsset(rangeAttackSO, $"{enemyActionSOPath + action.id}.asset");
        //            AssetDatabase.SaveAssets();
        //            AssetDatabase.Refresh();
        //            break;
        //        case EnemyBehaviour.MeleeAttack:
        //            EnemyAttackSO meleeAttackSO = new();
        //            meleeAttackSO.id = action.id;
        //            meleeAttackSO.action = action.action;
        //            meleeAttackSO.value = action.value;
        //            AssetDatabase.CreateAsset(meleeAttackSO, $"{enemyActionSOPath + action.id}.asset");
        //            AssetDatabase.SaveAssets();
        //            AssetDatabase.Refresh();
        //            break;
        //        case EnemyBehaviour.Move:
        //            EnemyMoveSO moveSO = new();
        //            moveSO.id = action.id;
        //            moveSO.action = action.action;
        //            moveSO.value = action.value;
        //            AssetDatabase.CreateAsset(moveSO, $"{enemyActionSOPath + action.id}.asset");
        //            AssetDatabase.SaveAssets();
        //            AssetDatabase.Refresh();
        //            break;
        //        case EnemyBehaviour.Rest:
        //            EnemyRestSO restSO = new();
        //            restSO.id = action.id;
        //            restSO.action = action.action;
        //            restSO.value = action.value;
        //            AssetDatabase.CreateAsset(restSO, $"{enemyActionSOPath + action.id}.asset");
        //            AssetDatabase.SaveAssets();
        //            AssetDatabase.Refresh();
        //            break;
        //    }

        //}
        foreach(var pattern in Database.AllEnemyPatternData)
        {
            EnemyPatternSO patternSO = new();
            patternSO.id = pattern.id;
            patternSO.range = pattern.range;
            patternSO.phase = pattern.phase;
            patternSO.weight = pattern.weight;
            List<EnemyBehaviourSO> actions = new();
            foreach(var action in pattern.actionSequence)
            {
                string actionPath = $"{enemyActionSOPath + action}.asset";
                EnemyBehaviourSO loadSO = AssetDatabase.LoadAssetAtPath<EnemyBehaviourSO>(actionPath);
                if (loadSO == null)
                {
                    Debug.Log($"적 행동 SO 안불러와짐 {actionPath}");
                    continue;
                }
                actions.Add(loadSO);
            }
            patternSO.actionSequence = actions;
            AssetDatabase.CreateAsset(patternSO, $"{enemyPatternSOPath + pattern.id}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

        foreach (var enemyData in Database.AllEnemyData)
        {
            EnemyDataSO enemySO = new();
            enemySO.enemyName = enemyData.name;
            enemySO.enemyId = enemyData.id;
            enemySO.spriteName = enemyData.sprite;
            enemySO.size = enemyData.size;
            enemySO.hpMiddle = enemyData.hpMiddle;
            enemySO.hpVariation = enemyData.hpVariation;
            List<EnemyPatternSO> patterns = new();
            foreach(var pattern in enemyData.pattern)
            {
                string patternPath = $"{enemyPatternSOPath + pattern}.asset";
                EnemyPatternSO loadSO = AssetDatabase.LoadAssetAtPath<EnemyPatternSO>(patternPath);
                if (loadSO == null)
                {
                    Debug.Log($"적 패턴 SO 안불러와짐 {patternPath}");
                    continue;
                }
                patterns.Add(loadSO);
            }
            enemySO.patternList = patterns;

            string initPatternPath = $"{enemyPatternSOPath + enemyData.initPattern}.asset";
            EnemyPatternSO initPattern = AssetDatabase.LoadAssetAtPath<EnemyPatternSO>(initPatternPath);
            if (initPattern == null)
            {
                Debug.Log($"시작패턴 {initPatternPath} 이 없음");
            }
            else
            {
                enemySO.initPattern = initPattern;
            }
            enemySO.phaseSwitchHpRatio = enemyData.phaseSwitchHpRatio;

            AssetDatabase.CreateAsset(enemySO, $"{enemyDataSOPath + enemyData.name}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
    }
}
