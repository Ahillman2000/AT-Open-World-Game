using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
class ChunkData
{
    public Mesh serializableMesh;
    public Material serializableMaterial;

    public ObjectData serializableGameObjects;
}

[SerializeField]
class ObjectData
{
    public string serializableName;

    public Mesh serializableMeshs;
    public Material serializableMaterials;

    public Vector3 serializableScale;
    public Vector3 serializablePositions;
    public Quaternion serializableRotations;
}

/*[SerializeField]
class ObjectMeshData
{
    public Mesh serializableMeshs;
    public Material serializableMaterials;
}*/

public class ChunkSaveSystem : MonoBehaviour
{
    Mesh mesh;
    Material material;
    ChunkData chunkData;
    ObjectData[] objectDatas;
    //ObjectMeshData[] objectMeshDatas;

    string json;
    string dataPath;

    public List<string> pathNames = new List<string>();
    public List<GameObject> gameObjectsInChunk = new List<GameObject>();
    public List<GameObject> npcsInChunk = new List<GameObject>();

    public bool loaded;

    private void Start()
    {
        InvokeRepeating(nameof(StreamChunks), 0f, 1f);
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

    public void SaveChunk()
    {
        //Debug.Log("Saving chunk ...");

        chunkData = new ChunkData
        {
            serializableMesh = mesh,
            serializableMaterial = material
        };

        json = JsonUtility.ToJson(chunkData, true);
        //dataPath = Application.persistentDataPath + "/" + this.gameObject.name + ".json";
        dataPath = Application.persistentDataPath + "/" + this.gameObject.GetInstanceID() + ".json";
        File.WriteAllText(dataPath, json);
    }

    public void LoadChunk()
    {
        //dataPath = Application.persistentDataPath + "/" + this.gameObject.name + ".json";
        dataPath = Application.persistentDataPath + "/" + this.gameObject.GetInstanceID() + ".json";

        if (!loaded && dataPath != null)
        {
            //Debug.Log("Loading chunk ...");

            json = File.ReadAllText(dataPath);
            ChunkData loadedChunkMesh = JsonUtility.FromJson<ChunkData>(json);

            SetMesh(loadedChunkMesh.serializableMesh, loadedChunkMesh.serializableMaterial);

            LoadChunkObjects();

            loaded = true;
        }
    }

    public void UnloadChunk()
    {
        //Debug.Log("Unloading chunk ...");

        Destroy(this.GetComponent<MeshFilter>());
        Destroy(this.GetComponent<MeshRenderer>());
        Destroy(this.GetComponent<MeshCollider>());

        UnloadChunkObjects();

        loaded = false;
    }

    public void SaveChunkObjects()
    {
        //Debug.Log("Saving chunk objects ...");

        objectDatas = new ObjectData[gameObjectsInChunk.Count];
        int i = 0;
        foreach (GameObject chunkObject in gameObjectsInChunk)
        {
            objectDatas[i] = new ObjectData
            {
                serializableName = chunkObject.name,

                serializableMeshs = chunkObject.GetComponent<MeshFilter>().sharedMesh,
                serializableMaterials = chunkObject.GetComponent<MeshRenderer>().sharedMaterial,

                serializableScale = gameObjectsInChunk[i].transform.localScale,
                serializablePositions = gameObjectsInChunk[i].transform.position,
                serializableRotations = gameObjectsInChunk[i].transform.rotation
            };

            json = JsonUtility.ToJson(objectDatas[i], true);
            //dataPath = Application.persistentDataPath + "/" + /*chunkObject.GetComponent<MeshFilter>().sharedMesh.name*/ chunkObject.name + ".json";
            dataPath = Application.persistentDataPath + "/" + chunkObject.GetInstanceID() + ".json";

            pathNames.Add(dataPath);

            File.WriteAllText(dataPath, json);
            i++;
        }
    }

    public void LoadChunkObjects()
    {
        //Debug.Log("Loading chunk objects ...");

        /*int i = 0;
        foreach (GameObject chunkObject in gameObjectsInChunk)
        {
            dataPath = pathNames[i];

            json = File.ReadAllText(dataPath);
            ObjectData objectData = JsonUtility.FromJson<ObjectData>(json);

            GameObject _chunkObject = new GameObject(objectData.serializableName);

            _chunkObject.AddComponent<MeshFilter>();
            _chunkObject.AddComponent<MeshRenderer>();
            _chunkObject.AddComponent<MeshCollider>();

            _chunkObject.GetComponent<MeshFilter>().sharedMesh   = objectData.serializableMeshs;
            _chunkObject.GetComponent<MeshRenderer>().material   = objectData.serializableMaterials;
            _chunkObject.GetComponent<MeshCollider>().sharedMesh = objectData.serializableMeshs;

            _chunkObject.transform.localScale = objectData.serializableScale;
            _chunkObject.transform.position = objectData.serializablePositions;
            _chunkObject.transform.rotation = objectData.serializableRotations;

            //gameObjectsInChunk.Add(_chunkObject);
            gameObjectsInChunk[i] = _chunkObject;

            Debug.Log("index count: " + i);

            i++;
        }*/

        for (int i = 0; i < gameObjectsInChunk.Count; i++)
        {
            dataPath = pathNames[i];

            json = File.ReadAllText(dataPath);
            ObjectData objectData = JsonUtility.FromJson<ObjectData>(json);

            GameObject _chunkObject = new GameObject(objectData.serializableName);

            _chunkObject.AddComponent<MeshFilter>();
            _chunkObject.AddComponent<MeshRenderer>();
            _chunkObject.AddComponent<MeshCollider>();

            _chunkObject.GetComponent<MeshFilter>().sharedMesh = objectData.serializableMeshs;
            _chunkObject.GetComponent<MeshRenderer>().material = objectData.serializableMaterials;
            _chunkObject.GetComponent<MeshCollider>().sharedMesh = objectData.serializableMeshs;

            _chunkObject.transform.localScale = objectData.serializableScale;
            _chunkObject.transform.position = objectData.serializablePositions;
            _chunkObject.transform.rotation = objectData.serializableRotations;

            gameObjectsInChunk[i] = _chunkObject;
        }
    }


    public void UnloadChunkObjects()
    {
        //Debug.Log("Unloading chunk objects ...");

        /*foreach (GameObject chunkObject in gameObjectsInChunk)
        {
            Destroy(chunkObject);
        }*/

        for (int i = 0; i < gameObjectsInChunk.Count; i++)
        {
            Destroy(gameObjectsInChunk[i]);
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
    }

    private void StreamChunks()
    {
        if ((Vector3.Distance(this.transform.position, Player.Instance.transform.position) <= Player.Instance.drawDistance) && !loaded)
        {
            LoadChunk();
        }
        else if ((Vector3.Distance(this.transform.position, Player.Instance.transform.position) > Player.Instance.drawDistance) && loaded)
        {
            UnloadChunk();
        }
    }
}
