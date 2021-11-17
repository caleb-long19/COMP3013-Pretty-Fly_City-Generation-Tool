using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ProceduralCity/Rule")]

//this is a sriptable object for creating the rules which dictate the generation of the districts
//you can create new rule object by right clicking / create new / ProceduralCity / Rule

//The rules are based on the L System for procedural generation

public class Rule : ScriptableObject
{

    public string letter;
    [SerializeField]
    private string[] results = null;
    [SerializeField]
    private bool randomResult = false;

    public string GetResult()
    {
        if (randomResult)
        {
            int randomIndex = UnityEngine.Random.Range(0, results.Length);
            return results[randomIndex];
        }
        return results[0];
    }
}
