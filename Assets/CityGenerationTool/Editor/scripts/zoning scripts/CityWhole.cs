using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityWhole : MonoBehaviour
{
    //this script will carry out methods that apply to every district

    // a list of every road object will be stored here
    // this will stop roads from different districts overlapping
    // road placer method should be in here

    //method to connect district roads should also be here

    public GameObject roadStraight, roadCorner, road3Way, road4Way, roadEnd;
    Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

    public void ConnectRoads()
    {

    }
}
