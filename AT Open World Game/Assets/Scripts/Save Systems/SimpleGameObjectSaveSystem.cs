using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGameObjectSaveSystem : MonoBehaviour
{
    MeshGenerator meshGenerator;

    MeshRenderer _renderer;
    MeshCollider _collider;

    int occupiedChunk;

    public bool loaded;

    [SerializeField] private bool debug;

    void Start()
    {
        meshGenerator = GameObject.FindGameObjectWithTag("MeshGenerator").GetComponent<MeshGenerator>();
        _renderer = this.GetComponent<MeshRenderer>();
        _collider = this.GetComponent<MeshCollider>();

        occupiedChunk = (int)(this.transform.position.z / 32) + ((int)(this.transform.position.x / 32) * 32);

        /*if (meshGenerator.chunks[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded) { loaded = true; }
        else { loaded = false; }*/

        //meshGenerator.chunks[occupiedChunk].GetComponent<ChunkSaveSystem>().objectsInChunk.Add(this.gameObject);
    }

    void Update()
    {
        if (meshGenerator.chunks[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded && !loaded)
        {
            _renderer.enabled = true;
            _collider.enabled = true;

            loaded = true;
        }
        else if (!meshGenerator.chunks[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded && loaded)
        {
            _renderer.enabled = false;
            _collider.enabled = false;

            loaded = false;
        }
    }
}
