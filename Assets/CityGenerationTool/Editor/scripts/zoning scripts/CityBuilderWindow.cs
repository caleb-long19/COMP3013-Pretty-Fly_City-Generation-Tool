using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CityBuilderWindow : EditorWindow
{
    private string objectBaseName = "";
    private int objectID = 1;
    private GameObject districtToSpawn;
    private GameObject cityParent;
    

    int gridIntDistrict = -1;
    int gridIntSize = -1;
    string[] districtNames = { "Down Town", "CBD", "Residential", "Industrial", "Ghetto", "Green Zone" };
    string[] districtSize = { "small", "medium", "large" };
    
    [MenuItem("Tools/City Builder Window")]   
    public static void ShowWindow()
    {
        GetWindow(typeof(CityBuilderWindow)); // showing the editor window
    }

    private void OnGUI() 
    {
        GUILayout.Label("Select District", EditorStyles.boldLabel);
        gridIntDistrict = GUILayout.SelectionGrid(gridIntDistrict, districtNames, 2);

        EditorGUILayout.Space();
        GUILayout.Label("Select Size", EditorStyles.boldLabel);
        gridIntSize = GUILayout.SelectionGrid(gridIntSize, districtSize, 3);

        EditorGUILayout.Space();
        objectBaseName = EditorGUILayout.TextField("District Name", objectBaseName);


        

        if(GUILayout.Button("Spawn District"))
        {
            Spawn();
        }

        if (GUILayout.Button("Generate"))
        {
            foreach (var district in GameObject.FindGameObjectsWithTag("District"))
            {
                district.GetComponent<CityZone>().Generate(district);
            }
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Undo"))
        {
            CityBuilderUndo();
        }

        if (GUILayout.Button("Redo"))
        {
            CityBuilderRedo();
        }

    }

    private void Spawn()
    {
        districtToSpawn = Resources.Load("prefabs/District Prefab") as GameObject;

        if(gridIntDistrict == -1)
        {
            Debug.LogError("Error: Please choose a district to spawn");
            return;
        }
        if(gridIntSize == -1)
        {
            Debug.LogError("Error: Please choose a size for the district");
            return;
        }
        if(objectBaseName == string.Empty)
        {
            objectBaseName = districtNames[gridIntDistrict];
        }
        if (!cityParent)
        {
            cityParent = new GameObject("City");
            
        }

        GameObject newObject = Instantiate(districtToSpawn, Vector3.zero, districtToSpawn.transform.rotation, cityParent.transform);
        Undo.RegisterCreatedObjectUndo(newObject, "Spawn District");
        newObject.name = objectBaseName + objectID;
        newObject.transform.GetChild(0).localScale = newObject.transform.GetChild(0).localScale * (gridIntSize + 1);
        newObject.transform.GetChild(0).GetComponent<Renderer>().material = Resources.Load("plane materials/" + objectBaseName, typeof(Material)) as Material;

        newObject.transform.GetChild(2).GetComponent<BuildingPlacer>().buildingCollection = (BuildingCollection)Resources.Load("Building Collections/" + objectBaseName + " Buildings");

        objectID++;

        if(gridIntSize == 0)
        {
            newObject.GetComponent<CityZone>().DistrictSize = 5;
            newObject.GetComponent<CityZone>().IterLimit = 2;
        }
        if (gridIntSize == 1)
        {
            newObject.GetComponent<CityZone>().DistrictSize = 8;
            newObject.GetComponent<CityZone>().IterLimit = 2;
        }
        if (gridIntSize == 2)
        {
            newObject.GetComponent<CityZone>().DistrictSize = 12;
            newObject.GetComponent<CityZone>().IterLimit = 3;
        }

        //district spawns at (0,0) so needs to be moved to a space where it is not overlapping with other districts
        moveDistrict(newObject);

        objectBaseName = "";
    }

    //this method will move a district in the x direction until it is no longer overlapping
    // the plan is to eventually make it move in the direction which requires the least distance to find space 
    private void moveDistrict(GameObject newDistrict)
    {
        
        
    }


    private void Generate()
    {
        var districts = GameObject.FindGameObjectsWithTag("District");

        foreach (var dist in districts)
        {
            dist.GetComponent<CityZone>().Generate(dist);
        }

    }

    private void CityBuilderUndo() 
    {
        Undo.PerformUndo();
    }
    private void CityBuilderRedo()
    {
        Undo.PerformRedo();
    }


}
