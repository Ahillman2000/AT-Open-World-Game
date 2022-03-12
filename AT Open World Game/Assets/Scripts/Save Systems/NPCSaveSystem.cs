using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
class NPCData
{
    public Mesh serializableMesh;
    public Material serializableMaterial;

    public Bounds serializableBounds;
    public Transform serializableRoot;

    //public Vector3 serializableCapsuleColliderCenter;
    //public float serializableCapsuleColliderRadius;
    //public float serializableCapsuleColliderHeight;

    //public bool serializableBoxColldierTrigger;
    //public Vector3 serializableBoxColliderCenter;
    //public Vector3 serializableBoxColliderSize;

    public Vector3 serializablePosition;
}

public class NPCSaveSystem : MonoBehaviour
{
    NPCData _NPCData;

    [SerializeField] GameObject body;
    [SerializeField] Transform root;
    SkinnedMeshRenderer skin;
    [SerializeField] GameObject canvas;

    Mesh mesh;
    Material material;

    CapsuleCollider capsuleCollider;
    BoxCollider boxCollider;

    string json;
    string dataPath;

    MeshGenerator meshGenerator;
    int occupiedChunk;

    public bool loaded;

    [SerializeField] private bool debug;

    private void Start()
    {
        meshGenerator = GameObject.FindGameObjectWithTag("MeshGenerator").GetComponent<MeshGenerator>();

        skin = body.GetComponent<SkinnedMeshRenderer>();
        mesh = skin.sharedMesh;
        material = skin.material;

        capsuleCollider = this.GetComponent<CapsuleCollider>();
        boxCollider = this.GetComponent<BoxCollider>();

        Save();

        loaded = true;
    }

    public void Save()
    {
        _NPCData = new NPCData();

        _NPCData.serializableMesh = mesh;
        _NPCData.serializableMaterial = material;

        //_NPCData.serializableBoundsCenter = skin.bounds.center;
        //_NPCData.serializableBoundsExtent = skin.bounds.extents;

        _NPCData.serializableBounds = skin.localBounds;
        _NPCData.serializableRoot = root;

        /*_NPCData.serializableCapsuleColliderCenter = capsuleCollider.center;
        _NPCData.serializableCapsuleColliderRadius = capsuleCollider.radius;
        _NPCData.serializableCapsuleColliderHeight = capsuleCollider.height;

        _NPCData.serializableBoxColldierTrigger = boxCollider.isTrigger;
        _NPCData.serializableBoxColliderCenter = boxCollider.center;
        _NPCData.serializableBoxColliderSize = boxCollider.size;*/

        json = JsonUtility.ToJson(_NPCData, true);
        dataPath = Application.persistentDataPath + "/" + this.gameObject.name + ".json";
        File.WriteAllText(dataPath, json);
    }

    public void Load()
    {
        dataPath = Application.persistentDataPath + "/" + this.gameObject.name + ".json";

        if (!loaded && dataPath != null)
        {
            json = File.ReadAllText(dataPath);
            NPCData loadedNPCData = JsonUtility.FromJson<NPCData>(json);

            if (this.GetComponent<SkinnedMeshRenderer>() == null)
            {
                body.AddComponent<SkinnedMeshRenderer>();

                body.GetComponent<SkinnedMeshRenderer>().localBounds  = loadedNPCData.serializableBounds;

                body.GetComponent<SkinnedMeshRenderer>().sharedMesh = loadedNPCData.serializableMesh;

                body.GetComponent<SkinnedMeshRenderer>().rootBone = loadedNPCData.serializableRoot;

                // Use a helper method, in the following code block, to populate the bone array for the instance's skinned mesh renderer
                SkinnedMeshBoneArrayPopulator.populateBoneArray(body.GetComponent<SkinnedMeshRenderer>());

                body.GetComponent<SkinnedMeshRenderer>().sharedMaterial = loadedNPCData.serializableMaterial;
                body.GetComponent<SkinnedMeshRenderer>().materials[0] = loadedNPCData.serializableMaterial;
            }
            
            /*if (this.GetComponent<CapsuleCollider>() == null)
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
            }*/

            canvas.SetActive(true);
            loaded = true;
        }
    }

    private void UnloadAssets()
    {
        Destroy(body.GetComponent<SkinnedMeshRenderer>());
        //Destroy(this.GetComponent<CapsuleCollider>());
        //Destroy(this.GetComponent<BoxCollider>());

        canvas.SetActive(false);
        loaded = false;
    }

    private void Update()
    {
        occupiedChunk = (int)(this.transform.position.z/32) + ((int)(this.transform.position.x/32) * 32);

        if (debug)
        {
            Debug.Log("this object belongs to: " + meshGenerator.chunks[occupiedChunk].gameObject.name + ": " + meshGenerator.chunks[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded);
        }

        if (meshGenerator.chunks[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded && !loaded)
        {
            Load();
        }
        else if (!meshGenerator.chunks[occupiedChunk].GetComponent<ChunkSaveSystem>().loaded && loaded)
        {
            UnloadAssets();
        }
    }
}

public class SkinnedMeshBoneArrayPopulator
{
    public static void populateBoneArray(SkinnedMeshRenderer skinnedMesh)
    {
        if (skinnedMesh.rootBone == null)
        {
            throw new System.Exception(
                "Missing root bone; please ensure that the root bone is set before attempting"
                + " to populate the bone array for the skinned mesh."
            );
        }

        var boneArray = new List<Transform>();
        var currentBone = skinnedMesh.rootBone;
        recursiveAdd(currentBone, ref boneArray);

        skinnedMesh.bones = boneArray.ToArray();
    }

    private static void recursiveAdd(Transform currentBone, ref List<Transform> boneArray)
    {
        boneArray.Add(currentBone);
        for (var i = 0; i < currentBone.childCount; i++)
        {
            recursiveAdd(currentBone.GetChild(i), ref boneArray);
        }
    }
}
