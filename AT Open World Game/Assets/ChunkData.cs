using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkData : MonoBehaviour
{
    int heigthMapResolution;
    int baseMapResolution;
    float[,] heights;

    public float[] position;

    public ChunkData(Chunk chunk)
    {
        heigthMapResolution = chunk.terrainData.heightmapResolution;
        baseMapResolution   = chunk.terrainData.baseMapResolution;
        heights             = chunk.terrainData.GetHeights(0,0,33,33);

        position = new float[3];
        position[0] = chunk.positon.x;
        position[1] = chunk.positon.y;
        position[2] = chunk.positon.z;
    }
}
