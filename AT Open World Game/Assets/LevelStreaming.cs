using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStreaming : MonoBehaviour
{
    //[SerializeField] private float loadDistance = 100f;
    //[SerializeField] private Terrain[] terrainChunks;
    //[SerializeField] GameObject player;

    public Chunk chunkToSave;

    void Start()
    {
        chunkToSave.SaveChunk();
    }

    void Update()
    {
        /*foreach(Terrain chunk in terrainChunks)
        {
            if(Vector3.Distance(player.transform.position, chunk.transform.position) > loadDistance)
            {
                chunk.enabled = false;
            }
            else
            {
                chunk.enabled = true;
            }
        }*/
    }
}
