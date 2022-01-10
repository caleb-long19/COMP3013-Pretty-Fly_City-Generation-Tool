using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DistrictSnap : MonoBehaviour
{
#if UNITY_EDITOR
    public float snapValue = 30.0f;
    public bool isMedium = false;

    void Update()
    {
        transform.position = RoundTransform(transform.position, snapValue);   
    }

    Vector3 RoundTransform(Vector3 v, float snapValue) 
    {
        if (isMedium)
        {
            return new Vector3
            (
                (snapValue * Mathf.Round(v.x / snapValue)) + 15.0f,
                snapValue * Mathf.Round(v.y / snapValue),
                (snapValue * Mathf.Round(v.z / snapValue)) + 15.0f
            );
        }
        else 
        {
            return new Vector3
            (
                snapValue * Mathf.Round(v.x / snapValue),
                snapValue * Mathf.Round(v.y / snapValue),
                snapValue * Mathf.Round(v.z / snapValue)
            );
        }
    }
#endif
}
