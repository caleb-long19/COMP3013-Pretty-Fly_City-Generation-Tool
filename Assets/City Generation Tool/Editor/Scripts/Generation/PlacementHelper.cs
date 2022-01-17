using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{

    public static int findRoadsForTerrain(Vector3Int position, List<Vector3Int> collection)
    {

        int count = 0;

        for (int x = -6; x <= 6; x = x += 2)
        {
            for (int z = -6; z <= 6; z += 2)
            {
                if (collection.Contains(position + new Vector3Int(x, 0, z)))
                {
                    count++;
                }
            }
        }

        return count;
        
        
    }

    public static List<Direction> findNeighbour(Vector3Int position, List<Vector3Int> collection)
    {
        
        List<Direction> neighbourDirections = new List<Direction>();
        if (collection.Contains(position + new Vector3Int(2,0,0)))
        {
            neighbourDirections.Add(Direction.Right);
        }
        if (collection.Contains(position - new Vector3Int(2,0,0)))
        {
            neighbourDirections.Add(Direction.Left);
        }
        if (collection.Contains(position + new Vector3Int(0, 0, 2)))
        {
            neighbourDirections.Add(Direction.Up);
        }
        if (collection.Contains(position - new Vector3Int(0, 0, 2)))
        {
            neighbourDirections.Add(Direction.Down);
        }


        
        return neighbourDirections;
    }

    internal static Vector3Int GetOffsetFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector3Int(0, 0, 2);
            case Direction.Down:
                return new Vector3Int(0, 0, -2);
            case Direction.Left:
                return Vector3Int.left * 2;
            case Direction.Right:
                return Vector3Int.right * 2;
            default:
                break;
        }
        throw new System.Exception("No Direction such as " + direction);
    }

    public static Direction GetReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                break;
        }
        throw new System.Exception("No direction such as " + direction);
    }
}
