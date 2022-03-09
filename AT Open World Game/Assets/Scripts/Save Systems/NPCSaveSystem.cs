using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
class NPCData
{
    public Mesh serializableMesh;
    public Material serializableMaterial;

    public Vector3 serializableCapsuleColliderCenter;
    public float serializableCapsuleColliderRadius;
    public float serializableCapsuleColliderHeight;

    public bool serializableBoxColldierTrigger;
    public Vector3 serializableBoxColliderCenter;
    public Vector3 serializableBoxColliderSize;
}

public class NPCSaveSystem : MonoBehaviour
{
    NPCData _NPCData;
    Mesh mesh;
    Material material;

    CapsuleCollider capsuleCollider;
    BoxCollider boxCollider;

    string json;
    string dataPath;

    MeshGenerator meshGenerator;
    int occupiedChunk;

    public bool loaded;

    private void Start()
    {
        meshGenerator = GameObject.FindGameObjectWithTag("MeshGenerator").GetComponent<MeshGenerator>();

        mesh = this.GetComponent<MeshFilter>().mesh;
        material = this.GetComponent<MeshRenderer>().material;
        capsuleCollider = this.GetComponent<CapsuleCollider>();
        boxCollider = this.GetComponent<BoxCollider>();

        loaded = true;
    }

    public void Save()
    {
        _NPCData = new NPCData();
        _NPCData.serializableMesh = mesh;
        _NPCData.serializableMaterial = material;

        _NPCData.serializableCapsuleColliderCenter = capsuleCollider.center;
        _NPCData.serializableCapsuleColliderRadius = capsuleCollider.radius;
        _NPCData.serializableCapsuleColliderHeight = capsuleCollider.height;

        _NPCData.serializableBoxColldierTrigger = boxCollider.isTrigger;
        _NPCData.serializableBoxColliderCenter = boxCollider.center;
        _NPCData.serializableBoxColliderSize = boxCollider.size;

        json = JsonUtility.ToJson(_NPCData, true);
        //Debug.Log(json);

        dataPath = Application.persistentDataPath + "/" + this.gameObject.name + ".json";
        File.WriteAllText(dataPath, json);
    }

    private void UnloadAssets()
    {
        Destroy(this.GetComponent<MeshFilter>());
        Destroy(this.GetComponent<MeshRenderer>());
        Destroy(this.GetComponent<CapsuleCollider>());
        Destroy(this.GetComponent<BoxCollider>());

        loaded = false;
    }

    public void Load()
    {
        if(!loaded && dataPath != null)
        {
            json = File.ReadAllText(dataPath);
            NPCData loadedNPCData = JsonUtility.FromJson<NPCData>(json);

            if (this.GetComponent<MeshFilter>() == null)
            {
                this.gameObject.AddComponent<MeshFilter>();
                this.GetComponent<MeshFilter>().sharedMesh = loadedNPCData.serializableMesh;
            }
            if (this.GetComponent<MeshRenderer>() == null)
            {
                this.gameObject.AddComponent<MeshRenderer>();
                this.GetComponent<MeshRenderer>().material = loadedNPCData.serializableMaterial;
            }
            if (this.GetComponent<CapsuleCollider>() == null)
            {
                this.gameObject.AddComponent<CapsuleCollider>();
                capsuleCollider = this.GetComponent<CapsuleCollider>();

                this.GetComponent<CapsuleCollider>().center = loadedNPCData.serializableCapsuleColliderCenter;
                this.GetComponent<CapsuleCollider>().radius = loadedNPCData.serializableCapsuleColliderRadius;
                this.GetComponent<CapsuleCollider>().height = loadedNPCData.serializableCapsuleColliderHeight;
            }
            if (this.GetComponent<BoxCollider>() == null)
            {
                this.gameObject.AddComponent<BoxCollider>();
                boxCollider = this.GetComponent<BoxCollider>();

                this.GetComponent<BoxCollider>().isTrigger = loadedNPCData.serializableBoxColldierTrigger;
                this.GetComponent<BoxCollider>().center = loadedNPCData.serializableBoxColliderCenter;
                this.GetComponent<BoxCollider>().size = loadedNPCData.serializableBoxColliderSize;
            }
            loaded = true;
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Save();
            UnloadAssets();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Load();
        }*/

        occupiedChunk = (int)(this.transform.position.z/32) + ((int)(this.transform.position.x/32) * 16);
        //Debug.Log(meshGenerator.chunks[occupiedChunk].gameObject.name);

        if (meshGenerator.chunks[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded && !loaded)
        //if (Chunks.list[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded && !loaded)
        {
            Load();
        }
        else if (!meshGenerator.chunks[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded && loaded)
        //else if (!Chunks.list[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded && loaded)
        {
            Save();
            UnloadAssets();
        }

    }
}
