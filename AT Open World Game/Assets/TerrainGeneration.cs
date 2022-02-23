using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public GameObject EmptyGameObject;
    public TerrainData[] terrainData;

    //private List<GameObject> chunks = new List<GameObject>(); 
    private GameObject[] chunks;

    private void Awake()
    {
        chunks = new GameObject[terrainData.Length];

        /*for (int i = 0; i < Mathf.Sqrt(terrainData.Length); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(terrainData.Length); j++)
            {
                GameObject _chunk = Instantiate(EmptyGameObject);
                _chunk.name = "Chunk (" + i + "," + j + ")";
                _chunk.AddComponent<ChunkLoader>();
                _chunk.GetComponent<ChunkLoader>().LoadChunk((terrainData[j + (i * (int)Mathf.Sqrt(terrainData.Length))]), new Vector3(i * 32, 0, j * 32));
                chunks[i * j] = _chunk;
            }
        }*/
    }
     
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < Mathf.Sqrt(terrainData.Length); i++)
            {
                for (int j = 0; j < Mathf.Sqrt(terrainData.Length); j++)
                {
                    GameObject _chunk = Instantiate(EmptyGameObject);
                    _chunk.name = "Chunk (" + i + "," + j + ")";
                    _chunk.GetComponent<ChunkLoader>().LoadChunk((terrainData[j + (i * (int)Mathf.Sqrt(terrainData.Length))]), new Vector3(i * 32, 0, j * 32));
                    chunks[i * j] = _chunk;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 0; i < Mathf.Sqrt(terrainData.Length); i++)
            {
                for (int j = 0; j < Mathf.Sqrt(terrainData.Length); j++)
                {
                    chunks[i * j].GetComponent<ChunkLoader>().UnloadChunk();
                }
            }
        }
    }
}
