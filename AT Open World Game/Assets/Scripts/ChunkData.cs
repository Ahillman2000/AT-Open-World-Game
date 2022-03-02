using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
class ChunkMesh
{
    public Mesh serializableMesh;
    public Material serializableMaterial;
}

public class ChunkData : MonoBehaviour
{
    Mesh mesh;
    Material material;
    ChunkMesh chunkMesh;

    string json;
    string dataPath;

    [SerializeField] private float drawDistance = 100f;
    private bool loaded;

    private void Start()
    {
        
    }

    public void SetMesh(Mesh _meshToSet, Material _material)
    {
        this.gameObject.AddComponent<MeshFilter>();
        this.gameObject.AddComponent<MeshRenderer>();
        this.gameObject.AddComponent<MeshCollider>();

        this.GetComponent<MeshFilter>().sharedMesh = _meshToSet;
        this.GetComponent<MeshRenderer>().material = _material;
        this.GetComponent<MeshCollider>().sharedMesh = _meshToSet;

        mesh = _meshToSet;
        material = _material;
    }
    public Mesh GetMesh()
    {
        return mesh;
    }

    public void ClearMesh()
    {
        Destroy(this.GetComponent<MeshFilter>());
        Destroy(this.GetComponent<MeshRenderer>());
        Destroy(this.GetComponent<MeshCollider>());

        loaded = false;
    }

    public void Save(string name)
    {
        chunkMesh = new ChunkMesh();
        chunkMesh.serializableMesh = mesh;
        chunkMesh.serializableMaterial = material;

        json = JsonUtility.ToJson(chunkMesh, true);
        Debug.Log(json);

        dataPath = Application.persistentDataPath + "/" + name + ".json";
        File.WriteAllText(dataPath, json);
    }

    public void Load()
    {
        json = File.ReadAllText(dataPath);

        ChunkMesh loadedChunkMesh = JsonUtility.FromJson<ChunkMesh>(json);
         
        SetMesh(loadedChunkMesh.serializableMesh, loadedChunkMesh.serializableMaterial);
        //Debug.Log("Mesh: " + loadedChunkMesh.serializableMesh);
        loaded = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Save(this.gameObject.name);
            Load();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ClearMesh();
            //Load();
        }

        /*if ((Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= drawDistance) && !loaded)
        {
            Load();
        }
        else if ((Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > drawDistance) && loaded)
        {
            ClearMesh();
        }*/
    }
}
