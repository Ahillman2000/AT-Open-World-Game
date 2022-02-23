using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector3 positon;
    public TerrainData terrainData;

    public void SaveChunk()
    {
        SaveSystem.SaveChunk(this);
    }

    public void UnloadChunk()
    {
        //
    }

    public void LoadChunk()
    {
        ChunkData data = SaveSystem.LoadChunk();

        Terrain.CreateTerrainGameObject(terrainData);

        positon.x = data.position[0];
        positon.y = data.position[1];
        positon.z = data.position[2];
    }
}
