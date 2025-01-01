using System;
using Game.World;
using UnityEngine;


[RequireComponent(typeof(Grid))]
[ExecuteInEditMode]
public class Board : MonoBehaviour
{
     [SerializeField] private Vector2Int gridSize;
     private Tile[][] _tilemap;
     private Grid _grid;
     private void Awake()
     {
          
     }
     
     public void Initialize(Tile[][] tilemap)
     {
          _tilemap = tilemap;
          gridSize = new Vector2Int(tilemap.Length, tilemap[0].Length);
     }
     
     #if UNITY_EDITOR
     public void Clear()
     {
          foreach (Transform child in transform)
          {
               DestroyImmediate(child.gameObject);
          }
     }
     #endif
}
