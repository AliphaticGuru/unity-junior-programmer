using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public string playerName;
    public int userScore;

    // Persistent storage structures
    public int globalHighScore;
    public string globalHighScoreName = "Nobody";

    // A Dictionary to store user name
    public Dictionary<string, int> userRecords = new Dictionary<string, int>();

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set up local file path (saves to standard AppData/Application support folder)
            saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
            LoadGameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetPersonalBest(string name)
    {
        string cleanName = name.Trim();
        if (userRecords.ContainsKey(cleanName))
        {
            return userRecords[cleanName];
        }
        return 0;
    }

    public void UpdateScores(int currentSessionScore)
    {
        userScore = currentSessionScore;
        string cleanName = playerName.Trim();

        // update personal best
        if (!userRecords.ContainsKey(cleanName))
        {
            userRecords.Add(cleanName, userScore);
        }
        else if (userScore > userRecords[cleanName])
        {
            userRecords[cleanName] = userScore;
        }

        // Update Global All-Time Record
        if (userScore > globalHighScore)
        {
            globalHighScore = userScore;
            globalHighScoreName = cleanName;
        }

        // Save progress to disk instantly
        SaveGameData();
    }

    public void ResetSingleUserData(string targetName)
    {
        string cleanName = targetName.Trim();

        // Remove the user from the history dictionary if existing
        if (userRecords.ContainsKey(cleanName))
        {
            userRecords.Remove(cleanName);
        }

        // Clear out active session values if it is the current player
        if (playerName.Trim() == cleanName)
        {
            userScore = 0;
        }

        // Save the modified profile list back to the JSON file
        SaveGameData();
        Debug.Log($"Profile data for '{cleanName}' has been wiped.");
    }

    public void ResetAllData()
    {
        playerName = "";
        userScore = 0;

        globalHighScore = 0;
        globalHighScoreName = "Nobody";

        userRecords.Clear();

        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        Debug.Log("All game data and profiles have been completely wiped.");
    }

    #region JSON Serialization (Persistence Between Sessions)
    [System.Serializable]
    private class SaveDataWrapper
    {
        public int globalHighScore;
        public string globalHighScoreName;
        public List<string> keys = new List<string>();
        public List<int> values = new List<int>();
    }

    public void SaveGameData()
    {
        SaveDataWrapper data = new SaveDataWrapper();
        data.globalHighScore = globalHighScore;
        data.globalHighScoreName = globalHighScoreName;

        // Serialize Dictionary to lists
        foreach (var user in userRecords)
        {
            data.keys.Add(user.Key);
            data.values.Add(user.Value);
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadGameData()
    {
        if (File.Exists(saveFilePath))
        {
           string json = File.ReadAllText(saveFilePath); 
           SaveDataWrapper data = JsonUtility.FromJson<SaveDataWrapper>(json);

           globalHighScore = data.globalHighScore;
           globalHighScoreName = data.globalHighScoreName;

           userRecords.Clear();
           for (int i = 0; i < data.keys.Count; i++)
            {
                userRecords[data.keys[i]] = data.values[i];
            }
        } 
    }
    #endregion
}
