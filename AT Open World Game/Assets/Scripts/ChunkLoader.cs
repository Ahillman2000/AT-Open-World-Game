using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    private GameObject chunk;
    [SerializeField] private TerrainData terrainData;

    private bool loaded;

    public void LoadChunk(TerrainData terrainData, Vector3 position, GameObject parent)
    {
        var _terrainData = Resources.Load<TerrainData>("Terrain/Terrain_split/" + terrainData.name);

        chunk = Terrain.CreateTerrainGameObject(_terrainData);

        chunk.transform.position = new Vector3(position.x, position.y, position.z);

        chunk.transform.SetParent(parent.transform);
    }

    public void UnloadChunk()
    {
        if(chunk != null)
        {
            Destroy(chunk);
        }
        Resources.UnloadUnusedAssets();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadChunk(terrainData, new Vector3(0,0,0));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnloadChunk();
        }*/
    }
}