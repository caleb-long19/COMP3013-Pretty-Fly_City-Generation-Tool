using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityZone : MonoBehaviour
{

    private string zoneType;
    public List<Transform> buildingList = new List<Transform>();
    
    public float buildingSpace;

    public float roadWidth = 30f;

    private Vector3 planeCorner;

    private float defaultSpacing = 0.2f;

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
        planeCorner = transform.TransformPoint(plane.GetComponent<MeshFilter>().sharedMesh.vertices[10]);
        Vector3 Position = planeCorner;

        for (int i = 0; i < buildingList.Count; i++)
        {
            Instantiate(buildingList[i], plane, true);
            //setting building properties
            plane.GetChild(i).name = buildingList[i].name;
            plane.GetChild(i).localScale = plane.GetChild(i).localScale / 8;

            //positioning of building
            Position.x += plane.GetChild(i).GetComponent<Renderer>().bounds.size.x;
            plane.GetChild(i).position = Position;    
        } 
    }



    static bool BoundsIsEncapsulated(Bounds Encapsulator, Bounds Encapsulating)
    {
        Debug.Log(Encapsulator.Contains(Encapsulating.min) && Encapsulator.Contains(Encapsulating.max));
        return Encapsulator.Contains(Encapsulating.min) && Encapsulator.Contains(Encapsulating.max);

    }


}
