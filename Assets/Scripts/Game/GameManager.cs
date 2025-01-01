
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
              if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                    }
                }
                return _instance;   
        }
    }
    
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private FloatVariableSO gateHP;
    public float GateHP
    {
        get => gateHP.Value;
        set => gateHP.Value = value;
    }
}
