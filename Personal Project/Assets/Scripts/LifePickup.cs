using UnityEngine;

public class LifePickup : PickupBase
{
    protected override bool Apply(GameObject player)
    {
        var lives = player.GetComponent<PlayerLives>();
        if (lives == null) return false;
        lives.AddLife(1);
        return true;
    }
}
