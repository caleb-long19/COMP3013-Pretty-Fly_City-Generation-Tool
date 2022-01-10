using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    List<DistrictPosition> districtPositions = new List<DistrictPosition>();

    public void SortDistrict(GameObject newDistrict, int newDistrictSize) 
    {
        int newDistrictX = 0;
        int newDistrictZ = 0;
        int size = newDistrictSize + 1;

        if (districtPositions.Count > 0) 
        {
            newDistrictX = districtPositions[districtPositions.Count - 1].x + districtPositions[districtPositions.Count - 1].size;
            newDistrictZ = districtPositions[districtPositions.Count - 1].z + districtPositions[districtPositions.Count - 1].size;
        }

        districtPositions.Add(new DistrictPosition(newDistrictX, newDistrictZ, size));
        newDistrict.transform.position = new Vector3((newDistrictX * 30) + (15 * newDistrictSize), 0, (newDistrictZ * 30) + (15 * newDistrictSize));
    }
}

public class DistrictPosition 
{
    public int x;
    public int z;
    public int size;

    public DistrictPosition(int x, int z, int size) 
    {
        this.x = x;
        this.z = z;
        this.size = size;
    }

}
