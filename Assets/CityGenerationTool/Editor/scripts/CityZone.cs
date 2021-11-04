using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityZone : MonoBehaviour
{

    private string zoneType;
    public List<Transform> buildingList = new List<Transform>();
    
    public float buildingSpace;

    GameObject newObj;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Clear(Transform plane)
    {
        int childs = plane.transform.childCount;
        
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(plane.transform.GetChild(i).gameObject);
        }
    }

    //method for randomly generating selected buildings on top of plane
    public void Generate()
    {
        Transform plane = this.transform.GetChild(0);
        Clear(plane);

        for (int i = 0; i < 3; i++)
        {

            Debug.Log(plane.transform.position.x);
            Debug.Log(plane.transform.position.z);

            float randomX = Random.Range(plane.transform.position.x - plane.transform.GetComponent<Renderer>().bounds.size.x / 2, plane.transform.parent.position.x + plane.transform.GetComponent<Renderer>().bounds.size.x / 2);
            float randomZ = Random.Range(plane.transform.parent.position.z - plane.transform.GetComponent<Renderer>().bounds.size.z / 2, plane.transform.parent.position.z + plane.transform.GetComponent<Renderer>().bounds.size.z / 2);
            Vector3 randomPosition = new Vector3(randomX, 0, randomZ);

            
            

            
            Instantiate(buildingList[i], plane, true);
            plane.GetChild(i).name = buildingList[i].name;
            plane.GetChild(i).localScale = plane.GetChild(i).localScale / 8;
            plane.GetChild(i).position = randomPosition;

        }

        
    }
}
