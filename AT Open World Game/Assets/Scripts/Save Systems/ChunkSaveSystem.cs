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
        //Save(this.gameObject.name);
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

            Debug.Log("setting chunk...");
        }

        mesh = _meshToSet;
        material = _material;

        loaded = true;
    }
    public Mesh GetMesh()
    {
        return mesh;
    }

    public void ClearMesh()
    {
        Debug.Log("Unloading chunk ...");

        Destroy(this.GetComponent<MeshFilter>());
        Destroy(this.GetComponent<MeshRenderer>());
        Destroy(this.GetComponent<MeshCollider>());

        loaded = false;
    }

    public void Save()
    {
        chunkMesh = new ChunkData();
        chunkMesh.serializableMesh = mesh;
        chunkMesh.serializableMaterial = material;

        json = JsonUtility.ToJson(chunkMesh, true);
        //Debug.Log(json);

        //Debug.Log(Application.persistentDataPath + "/" + name + ".json");
        dataPath = Application.persistentDataPath + "/" + this.gameObject.name + ".json";
        File.WriteAllText(dataPath, json);
    }

    public void Load()
    {
        dataPath = Application.persistentDataPath + "/" + this.gameObject.name + ".json";
        //Debug.Log(dataPath);

        if (!loaded && dataPath != null)
        {
            Debug.Log("Loading chunk ...");
            json = File.ReadAllText(dataPath);

            ChunkData loadedChunkMesh = JsonUtility.FromJson<ChunkData>(json);

            SetMesh(loadedChunkMesh.serializableMesh, loadedChunkMesh.serializableMaterial);
            //Debug.Log("Mesh: " + loadedChunkMesh.serializableMesh);
            loaded = true;
        }
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

        if ((Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= Player.Instance.drawDistance) && !loaded)
        {
            Load();
        }
        else if ((Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > Player.Instance.drawDistance) && loaded)
        {
            ClearMesh();
        }
    }
}
