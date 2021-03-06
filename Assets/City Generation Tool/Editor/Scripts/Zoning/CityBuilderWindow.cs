using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CityBuilderWindow : EditorWindow 
{
    private string cityName = "";
    private string newDistrictName = "";
    private string newCityName = "";

    private int objectID = 1;
    private GameObject districtToSpawn;
    private GameObject cityParent;
    private bool distSelected;
    private bool genTerrain;

    int gridIntDistrict = -1;
    int gridIntSize = -1;
    int gridIntBuildingSize = -1;
    string[] districtNames = { "Downtown District", "Central Business District", "Residential District", "Industrial District", "Slums District", "Green Zone" };
    string[] districtSize = { "Small District", "Medium District", "Large District" };
    string[] buildingHeight = { "Small", "Medium", "Large" };

    private static Texture2D tex;

    [MenuItem("Tools/Pretty Fly's City Generator")]   
    public static void ShowWindow()
    {
        GetWindow(typeof(CityBuilderWindow)); // showing the editor window

        

        tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex.SetPixel(0, 0, new Color32(204, 204, 196, 255));
        tex.Apply();
    }

    private void Awake()
    {
        cityParent = GameObject.FindGameObjectWithTag("City");
    }


    void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI() 
    {
        #region GUI Styles - These change the font colour, size, style, etc.
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.textColor = Color.white;
        myStyle.fontSize = 14;
        myStyle.fontStyle = FontStyle.Bold;

        GUIStyle Header = new GUIStyle();
        Header.normal.textColor = Color.white;
        Header.fontSize = 16;
        Header.fontStyle = FontStyle.Bold;
        #endregion


        #region Changes button colours and background
        // GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), tex, ScaleMode.StretchToFill);
        GUI.backgroundColor = new Color32(65, 121, 158, 255);
        #endregion


        #region Contains code to display majority of UI Elements e.g. distict type buttons, labels, etc.
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Enter Your City Name:", myStyle);
        cityName = EditorGUILayout.TextField(cityName, GUILayout.Width(300), GUILayout.Height(20));

        EditorGUILayout.Space(10);
        GUILayout.Label("Select District Type:", myStyle);
        gridIntDistrict = GUILayout.SelectionGrid(gridIntDistrict, districtNames, 2, GUILayout.Width(300), GUILayout.Height(80));

        EditorGUILayout.Space(10);
        GUILayout.Label("Select The Size Of Your District:", myStyle);
        gridIntSize = GUILayout.SelectionGrid(gridIntSize, districtSize, 3, GUILayout.Width(300), GUILayout.Height(40));

        EditorGUILayout.Space(10);
        GUILayout.Label("Spawn Your Selected District:", myStyle);
        if (GUILayout.Button("Spawn District/s", GUILayout.Width(300), GUILayout.Height(40)))
        {
            Spawn();
        }



        EditorGUILayout.Space(10);
        GUILayout.Label("Generate Your City Inside The District:", myStyle);
        genTerrain = EditorGUILayout.Toggle("Generate With Terrain", genTerrain);
        if (GUILayout.Button("Generate City", GUILayout.Width(300), GUILayout.Height(40)))
        {
            Generate();
        }
        #endregion


        #region Contains the Undo and Redo button
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Clear", GUILayout.Width(100), GUILayout.Height(35)))
        {
            CityBuilderClear();
        }

        if (GUILayout.Button("Undo", GUILayout.Width(100), GUILayout.Height(35)))
        {
            CityBuilderUndo();
        }

        if (GUILayout.Button("Redo", GUILayout.Width(100), GUILayout.Height(35)))
        {
            CityBuilderRedo();
        }
        GUILayout.EndHorizontal();
        #endregion

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (Selection.count != 0)
        {
            if (Selection.activeGameObject != null)
            {
                if (Selection.activeGameObject.tag == "City")
                {
                    GUILayout.Label("Edit Your City Details:", Header);

                    EditorGUILayout.Space(15);
                    EditorGUILayout.LabelField("Please Choose The Name Of Your City Prefab:", myStyle);
                    newCityName = EditorGUILayout.TextField(newCityName, GUILayout.Width(400), GUILayout.Height(25));

                    if (GUILayout.Button("Save Generated City As Prefab", GUILayout.Width(300), GUILayout.Height(30)))
                    {
                        GameObject selectedCity;
                        selectedCity = Selection.activeGameObject;
                        SaveCity(newCityName, selectedCity);
                    }
                }
            }
        }

        if (Selection.count != 0)
        {
            if (Selection.activeGameObject != null)
            {
                if (Selection.activeGameObject.tag == "District")
                {
                    var curDistrict = Selection.activeGameObject;
                    #region Contains code to allow user to change their city name and street length (Unfinished)
                    GUILayout.Label("Edit Your District Details:", Header);

                    EditorGUILayout.Space(15);
                    EditorGUILayout.LabelField("Change Selected District Name:", myStyle);
                    newDistrictName = EditorGUILayout.TextField(newDistrictName, GUILayout.Width(400), GUILayout.Height(25));


                    if (Selection.activeGameObject.GetComponent<CityZone>().dynamicHeight)
                    {
                        EditorGUILayout.LabelField("Change Building Height", myStyle);

                        gridIntBuildingSize = GUILayout.SelectionGrid(gridIntBuildingSize, buildingHeight, 3, GUILayout.Width(300), GUILayout.Height(40));
                    }

                    //street density

                    //building density

                    //


                    EditorGUILayout.Space(5);
                    if (GUILayout.Button("Save Details", GUILayout.Width(100), GUILayout.Height(40)))
                    {
                        if (newDistrictName != "")
                        {
                            curDistrict.name = newDistrictName;

                        }


                        setBuildingHeight(curDistrict, gridIntBuildingSize);

                    }

                    #endregion
                }
            }
        }


    }


    private void Spawn()
    {
        districtToSpawn = Resources.Load("Prefabs/District Prefab") as GameObject;

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
        
        if (!cityParent)
        {
            if(cityName != "")
            {
                cityParent = new GameObject(cityName);
                cityParent.tag = "City";
                
            }
            else
            {
                cityParent = new GameObject("City");
                cityParent.tag = "City";
            }
            cityParent.AddComponent<CityWhole>();
            cityParent.tag = "City";

            GameObject roadPlacer = Instantiate(Resources.Load("Prefabs/Road Placer") as GameObject, Vector3.zero, Quaternion.identity, cityParent.transform);
            roadPlacer.name = "Road Placer";
            cityParent.GetComponent<CityWhole>().RoadPlacer = roadPlacer.GetComponent<Roads>();

            GameObject buildingPlacer = Instantiate(Resources.Load("Prefabs/Building Placer") as GameObject, Vector3.zero, Quaternion.identity, cityParent.transform);
            buildingPlacer.name = "Building Placer";
            cityParent.GetComponent<CityWhole>().BuildingPlacer = buildingPlacer.GetComponent<BuildingPlacer>();

            GameObject gridManager = Instantiate(Resources.Load("Prefabs/Grid Manager") as GameObject, Vector3.zero, Quaternion.identity, cityParent.transform);
            gridManager.name = "Grid Manager";

            GameObject terrainHelper = Instantiate(Resources.Load("Prefabs/Terrain Helper") as GameObject, Vector3.zero, Quaternion.identity, cityParent.transform);
            terrainHelper.name = "Terrain Helper";
            cityParent.GetComponent<CityWhole>().TerrainHelper = terrainHelper.GetComponent<TerrainGeneration>();
        }

        GameObject newObject = Instantiate(districtToSpawn, Vector3.zero, districtToSpawn.transform.rotation, cityParent.transform);
        newObject.transform.GetChild(3).GetComponent<LSystemGenerator>().rules[0] = (Rule)Resources.Load("Rules/Rule " + districtNames[gridIntDistrict]);
        
        if(gridIntDistrict < 2){
            newObject.GetComponent<CityZone>().dynamicHeight = true;
        }

        Undo.RegisterCreatedObjectUndo(newObject, "Spawn District");
        newObject.name = districtNames[gridIntDistrict];

        newObject.GetComponent<CityZone>().distType = districtNames[gridIntDistrict];


        newObject.transform.GetChild(0).localScale = newObject.transform.GetChild(0).localScale * (gridIntSize + 1);
        newObject.transform.GetChild(0).GetComponent<Renderer>().material = Resources.Load("Planes/" + districtNames[gridIntDistrict], typeof(Material)) as Material;

        //newObject.transform.GetChild(2).GetComponent<BuildingPlacer>().buildingCollection = (BuildingCollection)Resources.Load("Building Collections/" + objectBaseName + " Buildings");
        newObject.GetComponent<CityZone>().buildingCollection = Resources.Load("Building Collections/" + districtNames[gridIntDistrict] + " Buildings") as BuildingCollection;

        objectID++;

        if(gridIntSize == 0)
        {
            newObject.GetComponent<CityZone>().DistrictSize = 6;
            newObject.GetComponent<CityZone>().IterLimit = 2;
        }
        if (gridIntSize == 1)
        {
            newObject.GetComponent<CityZone>().DistrictSize = 8;
            newObject.GetComponent<CityZone>().IterLimit = 3;
            newObject.GetComponent<DistrictSnap>().isMedium = true;
        }
        if (gridIntSize == 2)
        {
            newObject.GetComponent<CityZone>().DistrictSize = 12;
            newObject.GetComponent<CityZone>().IterLimit = 3;
        }

        //district spawns at (0,0) so needs to be moved to a space where it is not overlapping with other districts
        MoveDistrict(newObject, gridIntSize);

        
    }


    private void MoveDistrict(GameObject newDistrict, int districtSize)
    {
        newDistrict.transform.parent.GetComponentInChildren<GridManager>().SortDistrict(newDistrict, districtSize);
    }


    private void Generate()
    {
        cityParent = GameObject.FindGameObjectWithTag("City");
        cityParent.GetComponent<CityWhole>().Generate(genTerrain);
    }


    private void SaveCity(string cityName, GameObject selectedCity)
    {
        selectedCity.name = cityName;
        var ctiyChilds = GameObject.FindGameObjectsWithTag("CG_DAP");

        //Code to save generated city and prevent it from being overwritten when the user clicks generate city again
        string saveLocation = "Assets/City Generation Tool/Editor/CityPrefabs/" + selectedCity.name + ".prefab";

        foreach (GameObject child in ctiyChilds)
        {
            DestroyImmediate(child);
        }

        // create an empty prefab in project
        PrefabUtility.SaveAsPrefabAssetAndConnect(selectedCity, saveLocation, InteractionMode.UserAction);

    }

    private void CityBuilderClear()
    {
        cityParent.GetComponent<CityWhole>().RoadPlacer.Reset();
        cityParent.GetComponent<CityWhole>().BuildingPlacer.Reset();
        cityParent.GetComponent<CityWhole>().TerrainHelper.clear();
    }

    private void CityBuilderUndo() 
    {
        Undo.PerformUndo();
    }


    private void CityBuilderRedo()
    {
        Undo.PerformRedo();
    }

    private void setBuildingHeight(GameObject dist, int x)
    {

        string districtName = dist.GetComponent<CityZone>().distType;

        if(x == 0)
        {
            dist.GetComponent<CityZone>().buildingCollection = Resources.Load("Building Collections/" + districtName + " Buildings small") as BuildingCollection;
        }
        if(x == 1)
        {
            dist.GetComponent<CityZone>().buildingCollection = Resources.Load("Building Collections/" + districtName + " Buildings") as BuildingCollection;
        }
        if(x == 2)
        {
            dist.GetComponent<CityZone>().buildingCollection = Resources.Load("Building Collections/" + districtName + " Buildings big") as BuildingCollection;
        }
    }

    //when selecting district this code will show details about it for the user to change.
    

}
