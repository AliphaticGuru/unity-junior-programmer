
using System;
using System.IO;
using UnityEngine;

public class JsonSaveRepository
{
    private readonly string savePath = Path.Combine(
        Application.persistentDataPath,
        "survival-save.json"
    );

    public bool Exists => File.Exists(savePath);

    public void Save(GameSaveData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        data.savedAtUtc = DateTime.UtcNow.ToString("O");

        string json = JsonUtility.ToJson(data, true);
        string tempPath = savePath + ".tmp";

        File.WriteAllText(tempPath, json);

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        File.Move(tempPath, savePath);
    }

    public GameSaveData Load()
    {
        if (!Exists)
            return null;

        string json = File.ReadAllText(savePath);

        GameSaveData data =
            JsonUtility.FromJson<GameSaveData>(json);

        if (data == null || data.version != 1)
            throw new InvalidDataException(
                "Unsupported or invalid save file."
            );

        return data;
    }

    public void Delete()
    {
        if (Exists)
            File.Delete(savePath);
    }
}
