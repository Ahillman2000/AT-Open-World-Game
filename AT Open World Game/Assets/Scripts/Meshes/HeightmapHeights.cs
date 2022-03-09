using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightmapHeights : MonoBehaviour
{
    [SerializeField] private Texture2D heightmap;

    public int xSize;
    public int zSize;

    public float[] heights;

    void Start()
    {
        xSize = heightmap.width;
        zSize = heightmap.height;

        heights = new float[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                heights[i] = heightmap.GetPixel(x, z).r;

                i++;
            }
        }
    }
}
