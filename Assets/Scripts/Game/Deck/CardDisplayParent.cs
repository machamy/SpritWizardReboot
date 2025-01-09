using System;
using UnityEngine;


public class CardDisplayParent : MonoBehaviour
{
    public static CardDisplayParent Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
