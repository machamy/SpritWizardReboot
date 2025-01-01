using UnityEngine;

namespace Game.World
{
#if UNITY_EDITOR
    public class BoardInitializer : MonoBehaviour
    {
        [SerializeField] private Board board;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2 tileSize;
        [SerializeField] private GameObject whiteTilePrefab;
        [SerializeField] private GameObject greyTilePrefab;
        

        public void InitBoard()
        {
            Grid grid = board.GetComponent<Grid>();
            board.Clear();
            Tile[][] tilemap = new Tile[gridSize.y][];
            for (int y = 0; y < gridSize.y; y++)
            {
                tilemap[y] = new Tile[gridSize.x];
                for (int x = 0; x < gridSize.x; x++)
                {
                    Vector3 pos = grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    int cnt = x + y;
                    GameObject tilePrefab = cnt % 2 == 0 ? whiteTilePrefab : greyTilePrefab;
                    GameObject go = Instantiate(tilePrefab,pos , Quaternion.identity);
                    go.transform.SetParent(board.transform);
                    go.name = $"Tile {x},{y}";
                    Tile tile = go.GetComponent<Tile>();
                    tile.Initialize(new Vector2Int(x, y));
                    tilemap[y][x] = tile;
                }
            }
            board.Initialize(tilemap);
        }
    }
#endif
}