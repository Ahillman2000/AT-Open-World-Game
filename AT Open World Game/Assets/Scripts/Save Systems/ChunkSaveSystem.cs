using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
class ChunkData
{
    public Mesh serializableMesh;
    public Material serializableMaterial;
}

public class ChunkSaveSystem : MonoBehaviour
{
    Mesh mesh;
    Material material;
    ChunkData chunkMesh;

    string json;
    string dataPath;

    public bool loaded;

    private void Start()
    {

    }

    public void SetMesh(Mesh _meshToSet, Material _material)
    {
        if(this.GetComponent<MeshFilter>() == null)
        {
            this.gameObject.AddComponent<MeshFilter>();
            this.gameObject.AddComponent<MeshRenderer>();
            this.gameObject.AddComponent<MeshCollider>();

            this.GetComponent<MeshFilter>().sharedMesh = _meshToSet;
            this.GetComponent<MeshRenderer>().material = _material;
            this.GetComponent<MeshCollider>().sharedMesh = _meshToSet;

            //Debug.Log("setting chunk...");

            mesh = _meshToSet;
            material = _material;
        }

        loaded = true;
    }
    public Mesh GetMesh()
    {
        return mesh;
    }

    public void Save()
    {
        //Debug.Log("Saving chunk ...");

        chunkMesh = new ChunkData();

        chunkMesh.serializableMesh = mesh;
        chunkMesh.serializableMaterial = material;

        json = JsonUtility.ToJson(chunkMesh, true);
        dataPath = Application.persistentDataPath + "/" + this.gameObject.name + ".json";
        File.WriteAllText(dataPath, json);
    }

    public void Load()
    {
        dataPath = Application.persistentDataPath + "/" + this.gameObject.name + ".json";

        if (!loaded && dataPath != null)
        {
            //Debug.Log("Loading chunk ...");

            json = File.ReadAllText(dataPath);
            ChunkData loadedChunkMesh = JsonUtility.FromJson<ChunkData>(json);

            SetMesh(loadedChunkMesh.serializableMesh, loadedChunkMesh.serializableMaterial);

            loaded = true;
        }
    }

    public void UnloadChunk()
    {
        //Debug.Log("Unloading chunk ...");

        Destroy(this.GetComponent<MeshFilter>());
        Destroy(this.GetComponent<MeshRenderer>());
        Destroy(this.GetComponent<MeshCollider>());

        loaded = false;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Save(this.gameObject.name);
            Load();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ClearMesh();
            //Load();
        }*/

        if ((Vector3.Distance(this.transform.position, Player.Instance.transform.position) <= Player.Instance.drawDistance) && !loaded)
        {
            Load();
        }
        else if ((Vector3.Distance(this.transform.position, Player.Instance.transform.position) > Player.Instance.drawDistance) && loaded)
        {
            UnloadChunk();
        }
    }
}
