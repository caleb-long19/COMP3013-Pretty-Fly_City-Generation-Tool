using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CityBuilderWindow : EditorWindow
{
    string objectBaseName = "";
    int objectID = 1;
    GameObject districtToSpawn;
    float districtSize;


    int gridInt = -1;
    string[] districtNames = { "Down Town", "CBD", "Residential", "Industrial", "Ghetto", "Green Zone" };
    
    [MenuItem("Tools/City Builder Window")]   
    public static void ShowWindow()
    {
        GetWindow(typeof(CityBuilderWindow)); // showing the editor window
    }

    private void OnGUI() 
    {
        GUILayout.Label("Select District", EditorStyles.boldLabel);
        gridInt = GUILayout.SelectionGrid(gridInt, districtNames, 2);

        objectBaseName = EditorGUILayout.TextField("District Name", objectBaseName);
        districtSize = EditorGUILayout.Slider("District Size", districtSize, 0.5f, 3f);

        if(GUILayout.Button("Spawn District"))
        {
            Spawn();
        }

        if (GUILayout.Button("Generate"))
        {
            Generate();
        }

    }

    private void Spawn()
    {
        districtToSpawn = Resources.Load("District Prefab") as GameObject;

        if(districtToSpawn == null)
        {
            Debug.LogError("Error: Please choose a district to spawn");
            return;
        }
        if(objectBaseName == string.Empty)
        {
            objectBaseName = districtNames[gridInt];
        }

        GameObject newObject = Instantiate(districtToSpawn, Vector3.zero, districtToSpawn.transform.rotation);
        newObject.name = objectBaseName + objectID;
        newObject.transform.localScale = newObject.transform.localScale * districtSize;
        newObject.transform.GetChild(0).GetComponent<Renderer>().material = Resources.Load("plane materials/" + objectBaseName, typeof(Material)) as Material;
        objectID++;
        //district spawns at (0,0) so needs to be moved to a space where it is not overlapping with other districts
        moveDistrict(newObject);

        objectBaseName = "";
    }

    //this method will move a district in the x direction until it is no longer overlapping
    // the plan is to eventually make it move in the direction which requires the least distance to find space 
    private void moveDistrict(GameObject newDistrict)
    {
        var districts = GameObject.FindGameObjectsWithTag("District");
        var districtCount = districts.Length;

        

        if( districtCount > 1)
        { 
            
            foreach (var dist in districts)
            {
                Debug.Log("Position: " + newDistrict.transform.position);
                
                if (newDistrict != dist && Overlaps(newDistrict.transform.GetChild(0).GetComponent<Collider>(), dist.transform.GetChild(0).GetComponent<Collider>()))
                {
                    
                    Vector3 p = newDistrict.transform.position;
                    p.x = dist.transform.position.x + newDistrict.transform.GetChild(0).GetComponent<Renderer>().bounds.size.x / 2 + dist.transform.GetChild(0).GetComponent<Renderer>().bounds.size.x / 2;
                   
                    newDistrict.transform.position = p;


                    
                }
            }
        }
        
        
    }

    // a small method to check if districts overlap
    bool Overlaps(Collider collider1,Collider collider2)
    {
        Debug.Log(collider1.transform.parent);
        Debug.Log(collider1.bounds);
        Debug.Log(collider2.transform.parent);
        Debug.Log(collider2.bounds);

        Debug.Log(collider1.bounds.Intersects(collider2.bounds));
        return (collider1.bounds.Intersects(collider2.bounds));
    }

    
    
    private void Generate()
    {
        var districts = GameObject.FindGameObjectsWithTag("District");
        var districtCount = districts.Length;
        
        foreach(var dist in districts)
        {
            
            dist.GetComponent<CityZone>().Generate();
        }
    }
    
}
