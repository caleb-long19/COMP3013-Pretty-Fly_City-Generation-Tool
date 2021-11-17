using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityZone : MonoBehaviour
{
    public LSystemGenerator GenerationSystem;
    public Roads roads;
    public Buildings buildings;
    private int districtSize;
    private int iterLimit;
    private float angle = 90;
    public int DistrictSize { get => districtSize; set => districtSize = value; }

    public int Length
    {
        get
        {
            if (DistrictSize > 0)
            {
                return DistrictSize;
            }
            else
            {
                return 1;
            }
        }
        set => DistrictSize = value;
    }

    public int IterLimit { get => iterLimit; set {iterLimit = value; GenerationSystem.iterationLimit = value; }  }

    public void Generate(GameObject district)
    {
        reset(district.transform.GetChild(2));
        reset(district.transform.GetChild(3));
        roads.Reset();
        buildings.Reset();


        var sequence = GenerationSystem.GenerateSentence();
        LayRoad(sequence);
        roads.FixRoad();
        buildings.PlaceStructuresAroundRoad(roads.GetRoadPositions());


    




        //district.GetComponentInChildren<Visualiser>().CreateTown();
        //DestroyImmediate(district.transform.GetChild(0).gameObject);
    }

    private void LayRoad(string sequence)
{
    int length = DistrictSize;

    Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
    var currentPosition = this.gameObject.transform.position;

    Vector3 direction = Vector3.forward;
    Vector3 tempPosition = Vector3.zero;

    foreach (var letter in sequence)
    {
        referenceLetters reference = (referenceLetters)letter;
        switch (reference)
        {
            case referenceLetters.save:
                savePoints.Push(new AgentParameters
                {
                    position = currentPosition,
                    direction = direction,
                    length = Length
                });
                break;
            case referenceLetters.load:
                if (savePoints.Count > 0)
                {
                    var agentParameter = savePoints.Pop();
                    currentPosition = agentParameter.position;
                    direction = agentParameter.direction;
                    Length = agentParameter.length;
                }
                else
                {
                    throw new System.Exception("No Save Point in Stack");
                }
                break;
            case referenceLetters.draw:
                tempPosition = currentPosition;
                currentPosition += direction * length;
                roads.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), length);
                Length -= 2;

                break;
            case referenceLetters.turnRight:
                direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                break;
            case referenceLetters.turnLeft:
                direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                break;
            default:
                break;
        }
    } 
}
        

    // Used for the random prefab selection
    public GameObject[] prefabPool;
    public GameObject[] prefabRandom;



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
    

    public enum referenceLetters 
    {
        unkown = '1',
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-'
    }


    private void reset(Transform parent)
    {
        int i = 0;

        GameObject[] allChildren = new GameObject[parent.transform.childCount];

        foreach (Transform child in parent)
        {
            allChildren[i] = child.gameObject;
            i++;
        }
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}
