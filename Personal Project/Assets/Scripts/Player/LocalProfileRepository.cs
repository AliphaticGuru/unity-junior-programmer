using System;
using System.IO;
using UnityEngine;

public class LocalProfileRepository
{
    private readonly string profilePath;

    public LocalProfileRepository()
    {
        profilePath = Path.Combine(
            Application.persistentDataPath,
            "player-profile.json"
        );
    }

    public bool Exists => File.Exists(profilePath);

    public PlayerProfile Load()
    {
        if (!Exists)
            return null;

        return JsonUtility.FromJson<PlayerProfile>(
            File.ReadAllText(profilePath)
        );
    }

    public PlayerProfile Register(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");

        username = username.Trim();

        if (username.Length < 3 || username.Length > 20)
            throw new ArgumentException(
                "Username must be 3–20 characters."
            );

        foreach (char character in username)
        {
            if (!char.IsLetterOrDigit(character) &&
                character != '_')
            {
                throw new ArgumentException(
                    "Use letters, numbers or underscores only."
                );
            }
        }

        if (Exists)
            throw new InvalidOperationException(
                "A local profile already exists."
            );

        PlayerProfile profile = new PlayerProfile
        {
            id = Guid.NewGuid().ToString("N"),
            username = username,
            createdAtUtc = DateTime.UtcNow.ToString("O")
        };

        File.WriteAllText(
            profilePath,
            JsonUtility.ToJson(profile, true)
        );

        return profile;
    }
}
