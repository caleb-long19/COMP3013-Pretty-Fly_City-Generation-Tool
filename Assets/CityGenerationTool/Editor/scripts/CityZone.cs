using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityZone : MonoBehaviour
{

    private string zoneType;
    public List<Transform> buildingList = new List<Transform>();
    
    public float buildingSpace;

    private Vector3 planeCorner;

    GameObject newObj;

    void Clear(Transform plane)
    {
        int childs = plane.transform.childCount;
        
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(plane.transform.GetChild(i).gameObject);
        }
    }

    //method for randomly generating selected buildings on top of plane
    // building will be randomly selected
    public void Generate()
    {
        Transform plane = this.transform.GetChild(0);
        Clear(plane);
        planeCorner = transform.TransformPoint(plane.GetComponent<MeshFilter>().sharedMesh.vertices[0]);
        Vector3 Position = planeCorner;

        for (int i = 0; i < 3; i++)
        {
            placeBuilding(Position, buildingList[i], plane, i);
        }

        
    }


    void placeBuilding(Vector3 position, Transform building, Transform plane, int i)
    {
      

        Instantiate(building, plane, true);
        Vector3 P = plane.GetChild(i).position;

        

        plane.GetChild(i).name = buildingList[i].name;
        plane.GetChild(i).localScale = plane.GetChild(i).localScale / 8;
        P.x = position.x + plane.GetChild(i).GetComponent<Renderer>().bounds.size.x / 2;
        P.y = position.y + plane.GetChild(i).GetComponent<Renderer>().bounds.size.y / 2;
        plane.GetChild(i).position = P;

        position.x = position.x + buildingSpace;

        
    }

   
}
