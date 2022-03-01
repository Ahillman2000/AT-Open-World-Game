using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] Texture2D heightmap;

    private int width;
    private int height;
    private int depth;

    public float scale = 20;

    private void Start()
    {
        width = heightmap.width;
        height = heightmap.height;
    }

    private void Update()
    {
        this.transform.position = new Vector3(-width / 2, 0, -height / 2);
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        Debug.Log(terrainData.GetHeight(0, 0));

        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float) x / width * scale;
        float yCoord = (float) y / height * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
