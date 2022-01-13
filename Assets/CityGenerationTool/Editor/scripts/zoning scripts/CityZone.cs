using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CityZone : MonoBehaviour
{
    public LSystemGenerator GenerationSystem;
    public Transform roads;
    public Transform buildings;
    private int districtSize;
    private int iterLimit;
    private float angle = 90;

    public BuildingCollection buildingCollection;
    public List<Vector3Int> localRoadCoordinates;

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

    public void Generate(GameObject district, Roads roadPlacer, GameObject connectingDistrict)
    {
        reset(roads);
        reset(buildings);


        connectDisricts(roadPlacer, connectingDistrict);
        var sequence = GenerationSystem.GenerateSentence();

        
        LayRoad(sequence, roadPlacer);
        
    }
    //code to connect districts
    private void connectDisricts(Roads roadPlacer, GameObject district)
    {
        Vector3Int direction;
        int length =0;
        
        Vector3Int pos1 = Vector3Int.RoundToInt(this.transform.position);

        Vector3Int pos2 = Vector3Int.RoundToInt(district.transform.position);

        if (pos1.x < pos2.x)
        {
            direction = new Vector3Int(1, 0, 0);
        }
        else
        {
            direction = new Vector3Int(-1, 0, 0);
        }
        if(pos2.x - pos1.x < 0)
        {
            length = -1 * (pos2.x - pos1.x);
        }
        else
        {
            length =  (pos2.x - pos1.x);
        }
        roadPlacer.PlaceConnectingRoads(pos1, direction, length / 2, roads);

        if (pos1.z < pos2.z)
        {
            direction = new Vector3Int(0, 0, 1);
        }
        else
        {
            direction = new Vector3Int(0, 0, -1);
        }
        if (pos2.z - pos1.z < 0)
        {
            length = -1 * (pos2.z - pos1.z);
        }
        else
        {
            length = (pos2.z - pos1.z);
        }
        roadPlacer.PlaceConnectingRoads(pos1 + new Vector3Int((pos2.x - pos1.x),0, 0), direction, length / 2, roads);

    }



    public void LayRoad(string sequence, Roads roadPlacer)
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
                    roadPlacer.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), length, roads);
                    Length -= 2;

                    break;
                case referenceLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                case referenceLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;


                    //need to create curve prefabs and then set the current position based on where the curve ends up
                    //also need to implement curve letters into generation string
                case referenceLetters.smallUTurn:
                    if (Random.Range(0, 2) == 0)
                    {
                        roadPlacer.PlaceCurvedStreet(currentPosition, Vector3Int.RoundToInt(direction), "smallU", "left", roads);
                    }
                    else
                    {
                        roadPlacer.PlaceCurvedStreet(currentPosition, Vector3Int.RoundToInt(direction), "smallU", "right", roads);
                    }

                    break;

                case referenceLetters.bigUTurn:
                    if (Random.Range(0, 2) == 0)
                    {
                        roadPlacer.PlaceCurvedStreet(currentPosition, Vector3Int.RoundToInt(direction), "bigU", "left", roads);
                        
                    }
                    else
                    {
                        roadPlacer.PlaceCurvedStreet(currentPosition, Vector3Int.RoundToInt(direction), "bigU", "right", roads);
                    }

                    break;

                case referenceLetters.small90:
                    if (Random.Range(0, 2) == 0)
                    {
                        roadPlacer.PlaceCurvedStreet(currentPosition, Vector3Int.RoundToInt(direction), "small90", "left", roads);
                    }
                    else
                    {
                        roadPlacer.PlaceCurvedStreet(currentPosition, Vector3Int.RoundToInt(direction), "small90", "right", roads);
                    }

                    break;

                case referenceLetters.big90:
                    if (Random.Range(0, 2) == 0)
                    {
                        roadPlacer.PlaceCurvedStreet(currentPosition, Vector3Int.RoundToInt(direction), "big90", "left", roads);
                    }
                    else
                    {
                        roadPlacer.PlaceCurvedStreet(currentPosition, Vector3Int.RoundToInt(direction), "big90", "right", roads);
                    }

                    break;

                default:
                    break;
            }
        } 
    }
        

    

    public enum referenceLetters 
    {
        
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-',
        smallUTurn = 'u',
        bigUTurn = 'U',
        small90 = 'c',
        big90 = 'C'

    }


    private void reset(Transform parent)
    {
        int i = 0;

        localRoadCoordinates.Clear();

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
