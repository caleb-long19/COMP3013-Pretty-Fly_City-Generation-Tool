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

    // Used for the random prefab selection
    public GameObject[] prefabPool;
    public GameObject[] prefabRandom;

    // Method for randomly generating selected buildings on top of plane
    void Clear(Transform plane)
    {
        int childs = plane.transform.childCount;
        
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(plane.transform.GetChild(i).gameObject);
        }
    }

    // Method used to store 
    public void RandomPrefab()
    {
        //Loads objects from specified folder located in the resource folder
        prefabPool = Resources.LoadAll<GameObject>("Blender Models/Residential");

        // Stores total number of prefabs in folder.
        prefabRandom = new GameObject[prefabPool.Length];

        for (int i = 0; i < prefabRandom.Length; i++)
        {
            // We will now add three prefabs from prefabPool to prefabRandom array
            prefabRandom[i] = prefabPool[Random.Range(0, prefabPool.Length)];
        }
    }

    // Building will be randomly selected
    public void Generate()
    {
        Transform plane = this.transform.GetChild(0);
        Clear(plane);
        planeCorner = transform.TransformPoint(plane.GetComponent<MeshFilter>().sharedMesh.vertices[10]);
        Vector3 Position = planeCorner;
        RandomPrefab();

        for (int i = 0; i < prefabRandom.Length; i++)
        {
            Instantiate(prefabRandom[i], plane, true);
            //setting building properties
            plane.GetChild(i).name = prefabRandom[i].name;
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
