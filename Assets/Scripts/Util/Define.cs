using UnityEngine;

public static class Define
{
    public enum Direction
    {
        R,
        RU,
        RD,
        U,
        D,
        LU,
        LD,
        L,
        MAX
    }
    
    public static Vector2Int ToVectorInt(this Direction direction)
    {
        switch (direction)
        {
            case Direction.R:
                return new Vector2Int(1, 0);
            case Direction.RU:
                return new Vector2Int(1, 1);
            case Direction.RD:
                return new Vector2Int(1, -1);
            case Direction.U:
                return new Vector2Int(0, 1);
            case Direction.D:
                return new Vector2Int(0, -1);
            case Direction.LU:
                return new Vector2Int(-1, 1);
            case Direction.LD:
                return new Vector2Int(-1, -1);
            case Direction.L:
                return new Vector2Int(-1, 0);
            default:
                return Vector2Int.zero;
        }
    }
    

    public static bool IsDiagonal(this Direction direction)
    {
        switch (direction)
        {
            case Direction.R:
            case Direction.U:
            case Direction.D:
            case Direction.L:
                return false;
            default:
                return true;
        }
    }
    
}
