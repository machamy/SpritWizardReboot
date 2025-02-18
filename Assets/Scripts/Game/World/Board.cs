using System;
using System.Collections.Generic;
using Game.World;
using UnityEngine;


/// <summary>
/// 싱글턴
/// </summary>
[RequireComponent(typeof(Grid))]
[ExecuteInEditMode]
public class Board : MonoBehaviour
{
     [Serializable]
     public class TileLine
     {
          public Tile[] tiles;
     }
     
     
     [SerializeField] private Vector2Int gridSize;
     public Vector2Int GridSize => gridSize;
     private Tile[][] _tilemap;
     [SerializeField] private Grid _grid;
     [SerializeField] private TileLine[] tileLines;
     public void Initialize(Tile[][] tilemap)
     {
          tileLines = new TileLine[tilemap.Length];
          for (int i = 0; i < tilemap.Length; i++)
          {
               tileLines[i] = new TileLine();
               tileLines[i].tiles = tilemap[i];
          }
          gridSize = new Vector2Int(tilemap[0].Length, tilemap.Length);
     }

     private void Start()
     {
          _tilemap = new Tile[gridSize.y][];
          for (int i = 0; i < gridSize.y; i++)
          {
               _tilemap[i] = new Tile[gridSize.x];
               for (int j = 0; j < gridSize.x; j++)
               {
                    _tilemap[i][j] = tileLines[i].tiles[j];
                    _tilemap[i][j].Initialize(new Vector2Int(j,i));
               }
          }
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
          return _tilemap[cell.y][cell.x];
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
               //Debug.Log($"Cell out of bounds {cell}");
               return null;
          }
          return  _tilemap[cell.y][cell.x];
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
     
     /// <summary>
     ///  셀 좌표의 중심을 월드 좌표로 변환
     /// </summary>
     /// <param name="cell"></param>
     /// <returns></returns>
     public Vector3 CellCenterToWorld(Vector2Int cell)
     {
          return _grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
     }
     
     /// <summary>
     /// 해당 좌표 중심을 기준으로, 사각형 크기만큼 타일을 받아온다.
     /// raidus가 1이면 3x3, 2면 5x5
     /// </summary>
     /// <param name="center"></param>
     /// <param name="radius">0~N의 값.</param>
     /// <returns></returns>
     public List<Tile> GetTilesSquare(Vector2Int center, int radius)
     {
          List<Tile> tiles = new List<Tile>();
          for (int x = center.x - radius; x <= center.x + radius; x++)
          {
               for (int y = center.y - radius; y <= center.y + radius; y++)
               {
                    if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y)
                    {
                         continue;
                    }
                    tiles.Add(_tilemap[y][x]);
               }
          }
          return tiles;
     }

     /// <summary>
     /// 해당 좌표를 중심으로 사각형 크기만큼 타일을 받아온다.
     /// </summary>
     /// <param name="center"></param>
     /// <param name="width">가로</param>
     /// <param name="height">세로</param>
     /// <returns></returns>
     public List<Tile> GetTilesSquareAbs(Vector2Int center, int width, int height)
     {
          List<Tile> tiles = new List<Tile>();
          for (int x = center.x - width; x <= center.x + width; x++)
          {
               for (int y = center.y - height; y <= center.y + height; y++)
               {
                    if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y)
                    {
                         continue;
                    }
                    tiles.Add(_tilemap[y][x]);
               }
          }
          return tiles;
     }

     public List<Tile> GetTilesLine(Vector2Int start, Direction direction,int distance,bool cotainStart = true)
     {
          List<Tile> tiles = new List<Tile>();
          Vector2Int dir = direction.ToVectorInt();
          if (cotainStart)
          {
               tiles.Add(_tilemap[start.y][start.x]);
          }
          for (int i = 1; i < distance; i++)
          {
               Vector2Int pos = start + dir * i;
               if (pos.x < 0 || pos.x >= gridSize.x || pos.y < 0 || pos.y >= gridSize.y)
               {
                    break;
               }
               tiles.Add(_tilemap[pos.y][pos.x]);
          }
          return tiles;
     }

     public List<Tile> GetTilesBeam(Vector2Int start, Direction direction, int distance, int width)
     {
          List<Tile> tiles;
          tiles = GetTilesLine(start, direction, distance);

          Direction deltaDirection = direction.BeamWidthDirection();
          Vector2Int deltaVector = deltaDirection.ToVectorInt();
          Vector2Int oppositeVector;
          if (direction.IsDiagonal())
          {
               oppositeVector = deltaDirection.Turn90ClockWise().ToVectorInt();
          }
          else
          {
               oppositeVector = deltaDirection.Opposite().ToVectorInt();
          }
          for(int i = 2; i <= width; i++)
          {
               Vector2Int startPos = start + (i % 2 == 0 ? deltaVector : oppositeVector) * (i/2);
               tiles.AddRange(GetTilesLine(startPos, direction, distance));
          }
          return tiles;
     }
     
     public void UnFocusAllTiles(Tile.FocusState focusType)
     {
          foreach (Tile[] line in _tilemap)
          {
               foreach (Tile tile in line)
               {
                    if (tile.FocusType == focusType)
                    {
                         tile.Unfocus();
                    }
               }
          }
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
