
    using UnityEngine;

public static class DirectionHelper
{
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
         
    public static Direction BeamWidthDirection(this Direction direction)
    {
        switch (direction)
        {
            case Direction.R:
            case Direction.RU:
                return Direction.U;
            case Direction.RD:
            case Direction.D:
                return Direction.R;
            case Direction.LD:
            case Direction.L:
                return Direction.D;
            case Direction.LU:
            case Direction.U:
                return Direction.L;
            default:
                return Direction.R;
        }
    }

    public static Direction Next(this Direction direction)
    {
        return (Direction)(((int)direction + 1) % (int)Direction.MAX);
    }
    
    public static Direction Turn90ClockWise(this Direction direction)
    {
        switch (direction)
        {
            case Direction.R:
                return Direction.D;
            case Direction.RU:
                return Direction.RD;
            case Direction.RD:
                return Direction.LD;
            case Direction.U:
                return Direction.R;
            case Direction.D:
                return Direction.L;
            case Direction.LU:
                return Direction.RU;
            case Direction.LD:
                return Direction.L;
            case Direction.L:
                return Direction.U;
            default:
                return Direction.R;
        }
    }
    
    public static Direction Turn45ClockWise(this Direction direction)
    {
        switch (direction)
        {
            case Direction.R:
                return Direction.RD;
            case Direction.RD:
                return Direction.D;
            case Direction.D:
                return Direction.LD;
            case Direction.LD:
                return Direction.L;
            case Direction.L:
                return Direction.LU;
            case Direction.LU:
                return Direction.U;
            case Direction.U:
                return Direction.RU;
            case Direction.RU:
                return Direction.R;
            default:
                return Direction.R;
        }
    }
    
    public static Direction Turn90CounterClockWise(this Direction direction)
    {
        switch (direction)
        {
            case Direction.R:
                return Direction.U;
            case Direction.RU:
                return Direction.LU;
            case Direction.LU:
                return Direction.LD;
            case Direction.U:
                return Direction.L;
            case Direction.D:
                return Direction.R;
            case Direction.LD:
                return Direction.RD;
            case Direction.L:
                return Direction.D;
            case Direction.RD:
                return Direction.R;
            default:
                return Direction.R;
        }
    }
    
    public static Direction Turn45CounterClockWise(this Direction direction)
    {
        switch (direction)
        {
            case Direction.R:
                return Direction.RU;
            case Direction.RU:
                return Direction.U;
            case Direction.U:
                return Direction.LU;
            case Direction.LU:
                return Direction.L;
            case Direction.L:
                return Direction.LD;
            case Direction.LD:
                return Direction.D;
            case Direction.D:
                return Direction.RD;
            case Direction.RD:
                return Direction.R;
            default:
                return Direction.R;
        }
    }
    
    public static Direction Opposite(this Direction direction)
    {
        switch (direction)
        {
            case Direction.R:
                return Direction.L;
            case Direction.RU:
                return Direction.LD;
            case Direction.RD:
                return Direction.LU;
            case Direction.U:
                return Direction.D;
            case Direction.D:
                return Direction.U;
            case Direction.LU:
                return Direction.RD;
            case Direction.LD:
                return Direction.RU;
            case Direction.L:
                return Direction.R;
            default:
                return Direction.R;
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
