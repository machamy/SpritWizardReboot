using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json.Linq;
/// <summary>
/// 구글 시트를 기반으로 데이터 클래스 생성해주는 매니저
/// 수정사항 : 
/// type을 따로 받아오고
/// 배열도 받아올 수 있음
/// 출처 : https://goranitv.tistory.com/29
/// </summary>
public class GoogleSheetManager : MonoBehaviour
{
    [Tooltip("true: google sheet, false: local json")]
    [SerializeField] bool isAccessGoogleSheet = true;
    [Tooltip("Google sheet appsscript webapp url")]
    [SerializeField] string googleSheetUrl;
    // [Tooltip("Google sheet avail sheet tabs. seperate `/`. For example `Sheet1/Sheet2`")]
    // [SerializeField] string availSheets = "Sheet1/Sheet2";
    [Tooltip("For example `/GenerateGoogleSheet`")]
    [SerializeField] string generateFolderPath = "/GeneratedGoogleSheet";
    [Tooltip("You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`")]
    public ScriptableObject googleSheetSO;
    
    [Header("Types")]
    public List<string> allowedTypes = new List<string> { "string", "int", "float", "bool" };

    string JsonPath => $"{Application.dataPath}{generateFolderPath}/GoogleSheetJson.json";
    string ClassPath => $"{Application.dataPath}{generateFolderPath}/GoogleSheetClass.cs";
    string SOPath => $"Assets{generateFolderPath}/GoogleSheetSO.asset";

    string[] availSheetArray;
    string json;
    bool refeshTrigger;
    static GoogleSheetManager instance;



    public static T SO<T>() where T : ScriptableObject
    {
        if (GetInstance().googleSheetSO == null)
        {
            Debug.Log($"googleSheetSO is null");
            return null;
        }

        return GetInstance().googleSheetSO as T;
    }



#if UNITY_EDITOR
    [ContextMenu("FetchGoogleSheet")]
    async void FetchGoogleSheet()
    {
        //Init
        // availSheetArray = availSheets.Split('/');

        if (isAccessGoogleSheet)
        {
            Debug.Log($"Loading from google sheet..");
            json = await LoadDataGoogleSheet(googleSheetUrl);
        }
        else
        {
            Debug.Log($"Loading from local json..");
            json = LoadDataLocalJson();
        }
        if (json == null) return;
        // Debug.Log(json);
        bool isJsonSaved = SaveFileOrSkip(JsonPath, json);
        string allClassCode = GenerateCSharpClass(json);
        bool isClassSaved = SaveFileOrSkip(ClassPath, allClassCode);

        if (isJsonSaved || isClassSaved)
        {
            refeshTrigger = true;
            UnityEditor.AssetDatabase.Refresh();
        }
        else
        {
            CreateGoogleSheetSO();
            Debug.Log($"Fetch done.");
        }
    }

    async Task<string> LoadDataGoogleSheet(string url)
    {
        using (HttpClient client = new HttpClient())
        { 
            try
            {
                byte[] dataBytes = await client.GetByteArrayAsync(url);
                return Encoding.UTF8.GetString(dataBytes);
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"Request error: {e.Message}");
                return null;
            }
        }
    }

    string LoadDataLocalJson()
    {
        if (File.Exists(JsonPath))
        {
            return File.ReadAllText(JsonPath);
        }

        Debug.Log($"File not exist.\n{JsonPath}");
        return null;
    }

    bool SaveFileOrSkip(string path, string contents)
    {
        string directoryPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(path) && File.ReadAllText(path).Equals(contents))
            return false;

        File.WriteAllText(path, contents);
        return true;
    }

    // bool IsExistAvailSheets(string sheetName)
    // {
    //     return Array.Exists(availSheetArray, x => x == sheetName);
    // }
    private String ToTitleCase(String str)
    {
        return "Raw" + char.ToUpper(str[0]) + str.Substring(1);
    }

    string GenerateCSharpClass(string jsonInput)
    {
        JObject jsonObject = JObject.Parse(jsonInput);
        StringBuilder classCode = new();

        // Scriptable Object
        classCode.AppendLine("using System;\nusing System.Collections.Generic;\nusing UnityEngine;\n");
        classCode.AppendLine("/// <summary>You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`</summary>");
        classCode.AppendLine("public class GoogleSheetSO : ScriptableObject\n{");

        foreach (var sheet in jsonObject)
        {
            string className = sheet.Key;
            // if (!IsExistAvailSheets(className))
            //     continue;
            className = ToTitleCase(className);
            classCode.AppendLine($"\tpublic List<{className}> {className}List;");
        }
        classCode.AppendLine("}\n");

        // Class
        foreach (var jObject in jsonObject)
        {
            string className = jObject.Key;
            className = ToTitleCase(className);

            // if (!IsExistAvailSheets(className))
            //     continue;

            var items = (JArray)jObject.Value;
            var comments = (JObject)items[0];
            var types = (JObject)items[1];
            classCode.AppendLine($"[Serializable]\npublic class {className}\n{{");
            Dictionary<string, string> commentDict = new();
            foreach (var comment in comments.Properties())
            {
                commentDict.Add(comment.Name, comment.Value.ToString());
            }
            foreach (var property in types.Properties())
            {
                string propertyName = property.Name;
                string propertyType = property.Value.ToString();
                if (!allowedTypes.Contains(propertyType))
                {
                    propertyType = "string";
                }
                if (commentDict.ContainsKey(propertyName))
                    classCode.AppendLine($"\t/// <summary>{commentDict[propertyName]}</summary>");
                classCode.AppendLine($"\tpublic {propertyType} {propertyName};");
            }

            classCode.AppendLine("}\n");
        }
        return classCode.ToString();
    }

    // string GetCSharpType(JTokenType jsonType)
    // {
    //     switch (jsonType)
    //     {
    //         case JTokenType.Integer:
    //             return "int";
    //         case JTokenType.Float:
    //             return "float";
    //         case JTokenType.Boolean:
    //             return "bool";
    //         default:
    //             return "string";
    //     }
    // }

    bool CreateGoogleSheetSO()
    {
        if (Type.GetType("GoogleSheetSO") == null)
            return false;

        googleSheetSO = ScriptableObject.CreateInstance("GoogleSheetSO");
        JObject jsonObject = JObject.Parse(json);
        
        try
        {
            foreach (var jObject in jsonObject)
            {
                string className = jObject.Key;
                className = ToTitleCase(className);
                // if (!IsExistAvailSheets(className))
                //     continue;

                Type classType = Type.GetType(className);
                Type listType = typeof(List<>).MakeGenericType(classType);
                IList listInst = (IList)Activator.CreateInstance(listType);
                var items = (JArray)jObject.Value;

                for(int idx = 2; idx < items.Count; idx++)
                {
                    object classInst = Activator.CreateInstance(classType);
                    var item = items[idx];

                    foreach (var property in ((JObject)item).Properties())
                    {
                        FieldInfo fieldInfo = classType.GetField(property.Name);
                        // Debug.Log($"{fieldInfo.FieldType} {fieldInfo.Name} {property.Value}");
                        if(fieldInfo.FieldType.IsArray)
                        {
                            JArray arr = (JArray)property.Value;
                            Array array = Array.CreateInstance(fieldInfo.FieldType.GetElementType()!, arr.Count);
                            for (int i = 0; i < arr.Count; i++)
                            {
                                object value = Convert.ChangeType(arr[i].ToString(), fieldInfo.FieldType.GetElementType()!);
                                array.SetValue(value, i);
                            }
                            fieldInfo.SetValue(classInst, array);
                        }else
                        {
                            // Debug.Log($"{fieldInfo.FieldType} {fieldInfo.Name} {property.Value}");
                            object value = Convert.ChangeType(property.Value.ToString(), fieldInfo.FieldType);
                            fieldInfo.SetValue(classInst, value);
                        }
                    }

                    listInst.Add(classInst);
                }

                googleSheetSO.GetType().GetField($"{className}List").SetValue(googleSheetSO, listInst);
            }
        }
        catch (Exception e)
        { 
            Debug.LogError(e);
        }
        print("CreateGoogleSheetSO");
        UnityEditor.AssetDatabase.CreateAsset(googleSheetSO, SOPath);
        UnityEditor.AssetDatabase.SaveAssets();
        return true;
    }

    void OnValidate()
    {
        if (refeshTrigger)
        {
            bool isCompleted = CreateGoogleSheetSO();
            if (isCompleted)
            {
                refeshTrigger = false;
                Debug.Log($"Fetch done.");
            }
        }
    }
#endif

    static GoogleSheetManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindFirstObjectByType<GoogleSheetManager>();
        }
        return instance;
    }
}