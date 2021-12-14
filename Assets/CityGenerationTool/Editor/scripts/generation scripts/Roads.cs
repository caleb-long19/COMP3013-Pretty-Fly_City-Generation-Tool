using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Roads : MonoBehaviour
{


    public GameObject roadStraight, roadCorner, road3Way, road4Way, roadEnd;
    Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> allRoads = new HashSet<Vector3Int>();

    public List<Vector3Int> GetRoadPositions()
    {
        return roadDictionary.Keys.ToList();
    }

    public void PlaceStreetPositions(Vector3 startPosition, Vector3Int direction, int length, Transform parent)
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
            var road = Instantiate(roadStraight, position, rotation, parent);
            Undo.RegisterCreatedObjectUndo(road, "Generate Builings");
            roadDictionary.Add(position, road);
            
            allRoads.Add(position);
            parent.parent.gameObject.GetComponent<CityZone>().localRoadCoordinates.Add(position);
            
        }
        
    }

    public void FixRoad()
    {
        
        foreach (var position in allRoads)
        {
            List<Direction> neighbourDirections = PlacementHelper.findNeighbour(position, new List<Vector3Int>(roadDictionary.Keys));

            Quaternion rotation = Quaternion.identity;

            Transform parent = roadDictionary[position].transform.parent;
            

            if (neighbourDirections.Count == 1)
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
                roadDictionary[position] = Instantiate(roadEnd, position, rotation, parent);
                Undo.RegisterCreatedObjectUndo(roadDictionary[position], "Generate Road");

            }
            else if( neighbourDirections.Count == 2)
            {
                if(neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Down)
                    || neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Left)
                    )
                {
                    continue;
                }
                DestroyImmediate(roadDictionary[position]);
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
                roadDictionary[position] = Instantiate(roadCorner, position, rotation, parent);
                Undo.RegisterCreatedObjectUndo(roadDictionary[position], "Generate Builings");

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

                roadDictionary[position] = Instantiate(road3Way, position, rotation, parent);
                Undo.RegisterCreatedObjectUndo(roadDictionary[position], "Generate Builings");
            }
            else if (neighbourDirections.Count == 4)
            {
                DestroyImmediate(roadDictionary[position]);
                roadDictionary[position] = Instantiate(road4Way, position, rotation, parent);
                Undo.RegisterCreatedObjectUndo(roadDictionary[position], "Generate Builings");
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
        allRoads = new HashSet<Vector3Int>();
    }
}
