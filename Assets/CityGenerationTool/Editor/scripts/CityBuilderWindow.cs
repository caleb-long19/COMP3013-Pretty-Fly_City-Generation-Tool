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
    Color[] districtColors = {new Color32(255, 128, 0, 200),
                              new Color32(0, 128, 255, 200),
                              new Color32(255, 0, 255,200),
                              new Color32(255, 0, 0, 200),
                              new Color32(128, 128, 128, 200),
                              new Color32(0, 255, 0, 200)};

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
        newObject.GetComponent<Renderer>().material.color = districtColors[gridInt];

        //district spawns at (0,0) so needs to be moved to a space where it is not overlapping with other districts
        moveDistrict(newObject);

        objectBaseName = "";
    }

    //this method will move a district in the x direction until it is no longer overlapping
    // the plan is to eventually make it move in the direction which requires the least distance to find space 
    private bool moveDistrict(GameObject newDistrict)
    {
        var districts = GameObject.FindGameObjectsWithTag("District");
        var districtCount = districts.Length;

        if( districtCount > 1)
        {
            foreach (var dist in districts)
            {
                if (Overlaps(newDistrict.GetComponent<RectTransform>().rect, dist.GetComponent<RectTransform>().rect))
                {
                    Vector3 p = newDistrict.transform.position;
                    p.x = dist.transform.position.x + dist.transform.localScale.x / 2;
                    newDistrict.transform.position = p;
                }
            }
        }
        
        return false;
    }

    // a small method to check if districts overlap
    bool Overlaps(Rect rA, Rect rB)
    {
        return (rA.x < rB.x + rB.width && rA.x + rA.width > rB.x && rA.y < rB.y + rB.height && rA.y + rA.height > rB.y);
    }

    
}
