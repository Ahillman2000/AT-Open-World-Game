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

    private string mapName = "===== MAP =====";

    [SerializeField] private float scale = 20f;

    [SerializeField] Texture2D heightMap;
    [SerializeField] Gradient gradient;

    public int chunkSize = 32;
    public int numberOfChunks;

    private float minTerrainHeight = 1000;
    private float maxTerrainHeight = -1000;

    [SerializeField] Material terrainMaterial;

    public List<GameObject> chunks;

    void Start()
    {
        //this.transform.position = new Vector3(-(xSize / 2), 0, -(zSize / 2));

        //GenerateMap();
    }

    [ContextMenu ("GenerateMap")]
    private void GenerateMap()
    {
        chunks = new List<GameObject>();
        
        if(GameObject.Find(mapName) != null)
        {
            DestroyImmediate(GameObject.Find(mapName));
        }

        xSize = heightMap.width;
        zSize = heightMap.height;

        numberOfChunks = (int)(xSize / chunkSize);

        GameObject map = new GameObject(mapName);

        for (int x = 0; x < numberOfChunks; x++)
        {
            for (int z = 0; z < numberOfChunks; z++)
            {
                GameObject chunk = new GameObject("chunk " + x + " , " + z);

                chunks.Add(chunk);

                chunk.transform.position = new Vector3(x * chunkSize, 0, z * chunkSize);

                mesh = new Mesh();
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                CreateShape(x * chunkSize, z * chunkSize, chunkSize);
                SetColour();

                chunk.AddComponent<ChunkSaveSystem>();
                chunk.GetComponent<ChunkSaveSystem>().SetMesh(mesh, terrainMaterial);
                chunk.GetComponent<ChunkSaveSystem>().Save();

                UpdateMesh();

                chunk.transform.parent = map.transform;
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

                if (vertices[i].y > maxTerrainHeight)
                {
                    maxTerrainHeight = vertices[i].y;
                }
                if (vertices[i].y < minTerrainHeight)
                {
                    minTerrainHeight = vertices[i].y;
                }

                i++;
            }
        }

        triangles = new int[chunkSize * chunkSize * 6];
        int vertexIndex = 0;
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

    private void SetColour()
    {
        for (int i = 0; i < vertices.Length; i++)
        {

            float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);

            colours[i] = gradient.Evaluate(height);
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetColors(colours);
        
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

    }
}
