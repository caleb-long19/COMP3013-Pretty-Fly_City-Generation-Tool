using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public Gradient gradient;

    private float minHeight;
    private float maxHeight;

    List<Vector3Int> allRoads;

    public void generate(int xSize, int zSize, List<Vector3Int> allRoads)
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        this.allRoads = allRoads;
        CreateShape(xSize, zSize);
        UpdateMesh();
    }
    
    private void CreateShape(int xSize, int zSize )
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                int roadCount = PlacementHelper.findRoadsForTerrain(new Vector3Int(x * 2, 0, z * 2) + Vector3Int.RoundToInt(transform.position), allRoads);

                float y;
                if (roadCount == 0)
                {
                    y = UnityEngine.Random.Range(0, 3);
                }
                else
                {
                    y = 0;
                    
                }
                vertices[i] = new Vector3(x * 2, y, z * 2);

                if(y> maxHeight)
                {
                    maxHeight = y;
                }
                if (y < minHeight)
                {
                    minHeight = y;
                }
                i++;

                
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }


        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minHeight,maxHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);

                i++;
            }
        }

        this.transform.position += new Vector3(0, -0.1f, 0);



    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }

    public void clear()
    {
        mesh.Clear();
    }


}
