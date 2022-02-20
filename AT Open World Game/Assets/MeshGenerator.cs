using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;
    private int[]     triangles;

    [SerializeField] private int xSize = 20;
    [SerializeField] private int zSize = 20;

    void Start()
    {
        this.transform.position = new Vector3(-xSize / 2, 0, zSize / 2);

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.color = Color.green;
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles          = new int[xSize * zSize * 6];
        int vertexIndex    = 0;
        int trianglesIndex = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[trianglesIndex + 0] = vertexIndex + 0;
                triangles[trianglesIndex + 1] = vertexIndex + xSize + 1;
                triangles[trianglesIndex + 2] = vertexIndex + 1;
                triangles[trianglesIndex + 3] = vertexIndex + 1;
                triangles[trianglesIndex + 4] = vertexIndex + xSize + 1;
                triangles[trianglesIndex + 5] = vertexIndex + xSize + 2;

                vertexIndex++;
                trianglesIndex += 6;
            }
            vertexIndex++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices  = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        mesh.RecalculateBounds();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
}
