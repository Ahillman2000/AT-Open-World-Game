using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveChunk(Chunk chunk)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/chunk.dat";

        FileStream stream = new FileStream(path, FileMode.Create);

        ChunkData data = new ChunkData(chunk);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ChunkData LoadChunk()
    {
        string path = Application.persistentDataPath + "/chunk.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ChunkData data = formatter.Deserialize(stream) as ChunkData;
            stream.Close();
            return data;

        }
        else
        {
            Debug.LogError("Save file not found");
            return null;
        }
    }
}
