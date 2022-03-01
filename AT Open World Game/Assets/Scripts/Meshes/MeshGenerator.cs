using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;
    private int[]     triangles;

    Color[] colours;

    private int xSize;
    private int zSize;

    [SerializeField] private float scale = 20f;

    [SerializeField] Texture2D heightMap;
    [SerializeField] Gradient gradient;

    int chunkSize = 32;
    int numberOfChunks;

    private float minTerrainHeight;
    private float maxTerrainHeight;

    [SerializeField] Material terrainMaterial; 

    Dictionary<Vector3, Chunk> chunkDictionary = new Dictionary<Vector3, Chunk>();

    void Start()
    {
        //this.transform.position = new Vector3(-(xSize / 2), 0, -(zSize / 2));

        xSize = heightMap.width;
        zSize = heightMap.height;

        numberOfChunks = (int)(xSize / chunkSize) + 1;

        for (int x = 0; x < numberOfChunks; x++)
        {
            for (int z = 0; z < numberOfChunks; z++)
            {
                GameObject chunk = new GameObject("chunk " + x + " , " + z );
                chunk.transform.position = new Vector3(x * chunkSize, 0, z * chunkSize);

                chunk.AddComponent<MeshFilter>();
                chunk.AddComponent<MeshRenderer>();
                chunk.AddComponent<MeshCollider>();

                mesh = new Mesh();
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                //GetComponent<MeshFilter>().mesh = mesh;
                CreateShape(x * chunkSize, z * chunkSize, chunkSize);
                UpdateMesh();

                chunk.GetComponent<MeshFilter>().sharedMesh = mesh;
                chunk.GetComponent<MeshRenderer>().material = terrainMaterial;
                chunk.GetComponent<MeshCollider>().sharedMesh = mesh;
            }
        }
    }

    private void CreateShape(int offsetX, int offsetZ, int chunkSize)
    {
        vertices = new Vector3[(chunkSize + 1) * (chunkSize + 1)];
        colours = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++)
            {
                vertices[i] = new Vector3(x, heightMap.GetPixel(x + offsetX, z + offsetZ).r * scale, z);

                float height = vertices[i].y;
                
                if (height > maxTerrainHeight)
                {
                    maxTerrainHeight = height;
                }
                if(height < minTerrainHeight)
                {
                    minTerrainHeight = height;
                }

                height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, height);

                colours[i] = gradient.Evaluate(height);

                i++;
            }
        }

        triangles          = new int[chunkSize * chunkSize * 6];
        int vertexIndex    = 0;
        int trianglesIndex = 0;

        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {

                triangles[trianglesIndex + 0] = vertexIndex + 0;
                triangles[trianglesIndex + 1] = vertexIndex + chunkSize + 1;
                triangles[trianglesIndex + 2] = vertexIndex + 1;
                triangles[trianglesIndex + 3] = vertexIndex + 1;
                triangles[trianglesIndex + 4] = vertexIndex + chunkSize + 1;
                triangles[trianglesIndex + 5] = vertexIndex + chunkSize + 2;

                vertexIndex++;
                trianglesIndex += 6;
            }
            vertexIndex++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        //mesh.vertices  = vertices;
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        //mesh.triangles = triangles;
        mesh.SetColors(colours);
        //mesh.colors = colours;

        mesh.RecalculateNormals();

        mesh.RecalculateBounds();
        //MeshCollider meshCollider = GetComponent<MeshCollider>();
        //meshCollider.sharedMesh = mesh;
    }
}
