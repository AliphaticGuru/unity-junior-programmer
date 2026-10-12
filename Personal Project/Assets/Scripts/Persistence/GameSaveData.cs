
using System;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public int version = 1;

    public int score;
    public int wave;
    public int health;
    public int lives;

    public float enemySpeedBonus;

    public Vector3 playerPosition;
    public float playerFacing;

    public string savedAtUtc;
}
