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

    public Roads RoadPlacer { get => roadPlacer; set => roadPlacer = value; }
    public BuildingPlacer BuildingPlacer { get => buildingPlacer; set => buildingPlacer = value; }

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
        
    }
    

    private void Res()
    {
        roadPlacer.Reset();

        
    }



}
