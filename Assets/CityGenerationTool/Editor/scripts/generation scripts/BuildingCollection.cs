using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralCity/Building Collection")]
public class BuildingCollection : ScriptableObject
{
    public BuildingType[] buildings;
}
