using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Roads : MonoBehaviour
{
    public GameObject roadStraight, roadCorner, road3Way, road4Way, roadEnd;
    Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

    public List<Vector3Int> GetRoadPositions()
    {
        return roadDictionary.Keys.ToList();
    }

    public void PlaceStreetPositions(Vector3 startPosition, Vector3Int direction, int length)
    {
        var rotation = Quaternion.identity;
        if(direction.x == 0)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        for (int i = 0; i < length; i++)
        {
            var position = Vector3Int.RoundToInt(startPosition + direction * i * 2);
            if (roadDictionary.ContainsKey(position))
            {
                continue;
            }
            var road = Instantiate(roadStraight, position, rotation, transform);
            roadDictionary.Add(position, road);
            if (i==0 || i == length - 1)
            {
                fixRoadCandidates.Add(position);
            }
            
        }
        
    }

    public void FixRoad()
    {
        foreach (var position in fixRoadCandidates)
        {
            List<Direction> neighbourDirections = PlacementHelper.findNeighbour(position, roadDictionary.Keys);

            Quaternion rotation = Quaternion.identity;

            
            if(neighbourDirections.Count == 1)
            {
                DestroyImmediate(roadDictionary[position]);
                if (neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[position] = Instantiate(roadEnd, position, rotation, transform);

            }
            else if( neighbourDirections.Count == 2)
            {
                if(neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Down)
                    || neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Left)
                    )
                {
                    continue;
                }
                Destroy(roadDictionary[position]);
                if (neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down) && neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[position] = Instantiate(roadCorner, position, rotation, transform);

            }
            else if(neighbourDirections.Count == 3)
            {
                DestroyImmediate(roadDictionary[position]);
                if (neighbourDirections.Contains(Direction.Right)
                    && neighbourDirections.Contains(Direction.Down)
                    && neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down)
                    && neighbourDirections.Contains(Direction.Left)
                    && neighbourDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Left)
                    && neighbourDirections.Contains(Direction.Up)
                    && neighbourDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }

                roadDictionary[position] = Instantiate(road3Way, position, rotation, transform);
            }
            else if (neighbourDirections.Count == 4)
            {
                DestroyImmediate(roadDictionary[position]);
                roadDictionary[position] = Instantiate(road4Way, position, rotation, transform);
            }
        }
    }
    public void Reset()
    {
        foreach (var item in roadDictionary.Values)
        {
            DestroyImmediate(item);
        }
        roadDictionary.Clear();
        fixRoadCandidates = new HashSet<Vector3Int>();
    }
}
