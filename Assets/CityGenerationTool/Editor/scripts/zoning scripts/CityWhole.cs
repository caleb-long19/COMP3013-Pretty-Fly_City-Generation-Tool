using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityWhole : MonoBehaviour
{
    //this script will carry out methods that apply to every district

    // a list of every road object will be stored here
    // this will stop roads from different districts overlapping
    // road placer method should be in here

    //method to connect district roads should also be here


    [SerializeField] private Roads roadPlacer;
    [SerializeField] private BuildingPlacer buildingPlacer;
    [SerializeField] private TerrainGeneration terrainHelper;

    public Roads RoadPlacer { get => roadPlacer; set => roadPlacer = value; }
    public BuildingPlacer BuildingPlacer { get => buildingPlacer; set => buildingPlacer = value; }
    public TerrainGeneration TerrainHelper { get => terrainHelper; set => terrainHelper = value; }

    private int xSize;
    private int zSize;

    public void Generate()
    {
        int i =0;
        Res();
        var districts = GameObject.FindGameObjectsWithTag("District");

        

        foreach (var dist in districts)
        {
            if (i != districts.Length - 1)
            {
                dist.GetComponent<CityZone>().Generate(dist, roadPlacer, districts[i+1]);
            }
            else
            {
                dist.GetComponent<CityZone>().Generate(dist, roadPlacer, districts[0]);
            }
            i++;
        }

        roadPlacer.FixRoad();
        buildingPlacer.Reset();
        foreach(var dist in districts)
        {
            buildingPlacer.PlaceStructuresAroundRoad(dist.GetComponent<CityZone>().localRoadCoordinates, roadPlacer.allRoads, dist.GetComponent<CityZone>().buildingCollection, dist.GetComponent<CityZone>().buildings);
        }


        ReSizeTerrain();
        terrainHelper.generate((xSize + 32) / 2, (zSize + 32) / 2, roadPlacer.allRoads) ;
        
    }

    private void ReSizeTerrain()
    {
        List<Vector3Int> roads = roadPlacer.allRoads;

        roads.Sort((a, b) => a.x.CompareTo(b.x));
        xSize = roads[roads.Count - 1].x - roads[0].x;
        int x = roads[0].x;

        roads.Sort((a, b) => a.z.CompareTo(b.z));
        zSize = roads[roads.Count - 1].z - roads[0].z;
        int z = roads[0].z;

        terrainHelper.transform.position = new Vector3Int(x - 16, 0, z - 16);
    }
    

    private void Res()
    {
        roadPlacer.Reset();

        
    }



}
