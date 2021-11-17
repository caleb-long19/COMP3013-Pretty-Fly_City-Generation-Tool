using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CityBuilderWindow : EditorWindow
{
    string objectBaseName = "";
    int objectID = 1;
    GameObject districtToSpawn;
    //float districtSize;
    

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

        if(districtToSpawn == null)
        {
            Debug.LogError("Error: Please choose a district to spawn");
            return;
        }
        if(objectBaseName == string.Empty)
        {
            objectBaseName = districtNames[gridIntDistrict];
        }

        GameObject newObject = Instantiate(districtToSpawn, Vector3.zero, districtToSpawn.transform.rotation);
        Undo.RegisterCreatedObjectUndo(newObject, "Spawn District");
        newObject.name = objectBaseName + objectID;
        newObject.transform.GetChild(0).localScale = newObject.transform.GetChild(0).localScale * (gridIntSize + 1);
        newObject.transform.GetChild(0).GetComponent<Renderer>().material = Resources.Load("plane materials/" + objectBaseName, typeof(Material)) as Material;
        objectID++;

        if(gridIntSize == 0)
        {
            newObject.GetComponent<CityZone>().DistrictSize = 4;
            newObject.GetComponent<CityZone>().IterLimit = 2;
        }
        if (gridIntSize == 1)
        {
            newObject.GetComponent<CityZone>().DistrictSize = 8;
            newObject.GetComponent<CityZone>().IterLimit = 2;
        }
        if (gridIntSize == 2)
        {
            newObject.GetComponent<CityZone>().DistrictSize = 10;
            newObject.GetComponent<CityZone>().IterLimit = 4;
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

<<<<<<< HEAD
    
=======
    // a small method to check if districts overlap
    private bool Overlaps(Collider collider1,Collider collider2)
    {
        Debug.Log(collider1.transform.parent);
        Debug.Log(collider1.bounds);
        Debug.Log(collider2.transform.parent);
        Debug.Log(collider2.bounds);
>>>>>>> 0a9f1f48113acc97bfd2b22c4284b4448e41f2fa

    

    
    
<<<<<<< HEAD
   
    
=======
    private void Generate()
    {
        var districts = GameObject.FindGameObjectsWithTag("District");
        var districtCount = districts.Length;
        
        foreach(var dist in districts)
        {
            
            dist.GetComponent<CityZone>().Generate();
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

<<<<<<< Updated upstream
=======
>>>>>>> 0a9f1f48113acc97bfd2b22c4284b4448e41f2fa
>>>>>>> Stashed changes
}
