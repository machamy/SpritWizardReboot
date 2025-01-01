using System;
using Game.World;
using UnityEngine;


/// <summary>
/// 싱글턴
/// </summary>
[RequireComponent(typeof(Grid))]
[ExecuteInEditMode]
public class Board : MonoBehaviour
{
     public static Board Instance { get; private set; }
     
     [SerializeField] private Vector2Int gridSize;
     public Vector2Int GridSize => gridSize;
     private Tile[][] _tilemap;
     private Grid _grid;
     
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
     
     public void Initialize(Tile[][] tilemap)
     {
          _tilemap = tilemap;
          gridSize = new Vector2Int(tilemap.Length, tilemap[0].Length);
     }

     /// <summary>
     /// world position을 바탕으로, 타일을 받아온다. 범위 밖이면 null
     /// </summary>
     /// <param name="worldPos"></param>
     /// <returns></returns>
     public Tile WorldToTile(Vector3 worldPos)
     {
          Vector3Int cell = _grid.WorldToCell(worldPos);
          if (cell.x < 0 || cell.x >= gridSize.x || cell.y < 0 || cell.y >= gridSize.y)
          {
               Debug.Log($"Cell out of bounds {cell} (input : {worldPos})");
               return null;
          }
          return _tilemap[cell.x][cell.y];
     }

     public Tile GetTile(Vector2Int coordinates) => CellToTile(coordinates);
     
     /// <summary>
     /// 셀 좌표를 바탕으로 타일을 받아온다. 범위 밖이면 null
     /// </summary>
     /// <param name="worldPos"></param>
     /// <returns></returns>
     public Tile CellToTile(Vector2Int cell)
     {
          if (cell.x < 0 || cell.x >= gridSize.x || cell.y < 0 || cell.y >= gridSize.y)
          {
               Debug.Log($"Cell out of bounds {cell}");
               return null;
          }
          return _tilemap[cell.x][cell.y];
     }
     
     /// <summary>
     /// 월드 좌표 -> 셀 좌표
     /// </summary>
     /// <param name="worldPos"></param>
     /// <returns></returns>
     public Vector2Int WorldToCell(Vector3 worldPos)
     {
          Vector3Int cell = _grid.WorldToCell(worldPos);
          return new Vector2Int(cell.x, cell.y);
     }
     
     /// <summary>
     /// 셀 좌표 -> 월드 좌표
     /// </summary>
     /// <param name="cell"></param>
     /// <returns></returns>
     public Vector3 CellToWorld(Vector2Int cell)
     {
          return _grid.CellToWorld(new Vector3Int(cell.x, cell.y, 0));
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
