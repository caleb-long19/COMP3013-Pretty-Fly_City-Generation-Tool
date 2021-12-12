using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildingPlacer : MonoBehaviour
{
    public BuildingCollection buildingCollection;
    public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject>();

    public void PlaceStructuresAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeSpots = FindFreeSpacesAroundRoad(roadPositions);
        List<Vector3Int> blockedPositions = new List<Vector3Int>();
        foreach(var freeSpot in freeSpots)
        {
            if (blockedPositions.Contains(freeSpot.Key))
            {
                continue;
            }
            var rotation = Quaternion.identity;
            switch (freeSpot.Value)
            {
                case Direction.Up:
                    rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Direction.Down:
                    rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case Direction.Right:
                    rotation = Quaternion.Euler(0, 180, 0);
                    break;
                default:
                    break;
            }
            for (int i = 0; i < buildingCollection.buildings.Length; i++)
            {
                if(buildingCollection.buildings[i].quantity == -1)
                {
                    var building = SpawnPrefab(buildingCollection.buildings[i].GetPrefab(), freeSpot.Key, rotation);
                    structuresDictionary.Add(freeSpot.Key, building);
                    break;
                }
                if (buildingCollection.buildings[i].IsBuildingAvailable())
                {
                    if(buildingCollection.buildings[i].sizeRequired > 1)
                    {
                        var halfSize = Mathf.FloorToInt(buildingCollection.buildings[i].sizeRequired / 2.0f);
                        List<Vector3Int> tempPosLocked = new List<Vector3Int>();
                        if(VerifyIfBuildingFits(halfSize, freeSpots,freeSpot,blockedPositions ,ref tempPosLocked))
                        {
                            blockedPositions.AddRange(tempPosLocked);
                            var building = SpawnPrefab(buildingCollection.buildings[i].GetPrefab(), freeSpot.Key, rotation);
                            structuresDictionary.Add(freeSpot.Key, building);
                            foreach(var pos in tempPosLocked)
                            {
                                structuresDictionary.Add(pos, building);
                            }
                            
                        }
                    }
                    else
                    {
                        var building = SpawnPrefab(buildingCollection.buildings[i].GetPrefab(), freeSpot.Key, rotation);
                        structuresDictionary.Add(freeSpot.Key, building);
                    }
                    break;
                }
            
            }
            
        }
    }

    private bool VerifyIfBuildingFits(int halfSize, Dictionary<Vector3Int, Direction> freeSpots, KeyValuePair<Vector3Int, Direction> freeSpot,List<Vector3Int> blockedPositions, ref List<Vector3Int> tempPosLocked)
    {
        Vector3Int direction = Vector3Int.zero;
        if(freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
        {
            direction = Vector3Int.right;
        }
        else
        {
            direction = new Vector3Int(0, 0, 2);
        }
        for (int i = 1; i <= halfSize; i++)
        {
            var pos1 = freeSpot.Key + direction * i;
            var pos2 = freeSpot.Key - direction * i;
            if(!freeSpots.ContainsKey(pos1) || !freeSpots.ContainsKey(pos2) || 
                blockedPositions.Contains(pos1) || blockedPositions.Contains(pos2))
            { 
                return false;
            }
            tempPosLocked.Add(pos1);
            tempPosLocked.Add(pos2);
        }
        return true;
    }

    private GameObject SpawnPrefab(GameObject prefab, Vector3Int position, Quaternion rotation)
    {
        var newStructure = Instantiate(prefab, position, rotation, transform);
        Undo.RegisterCreatedObjectUndo(newStructure, "Generate Buildings");
        return newStructure;
    }

    private Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        //duplicate code from road code, possibly find more efficient way
        foreach(var position in roadPositions)
        {
            var neighboursDirections = PlacementHelper.findNeighbour(position, roadPositions);
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if(neighboursDirections.Contains(direction) == false)
                {
                    var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);
                    if (freeSpaces.ContainsKey(newPosition))
                    {
                        continue;
                    }
                    freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                }
            }
        }
        return freeSpaces;
    }
    public void Reset()
    {
        foreach (var item in structuresDictionary.Values)
        {
            DestroyImmediate(item);
        }
        structuresDictionary.Clear();
        foreach (var buildingType in buildingCollection.buildings)
        {
            buildingType.Reset();
        }

    }
}
