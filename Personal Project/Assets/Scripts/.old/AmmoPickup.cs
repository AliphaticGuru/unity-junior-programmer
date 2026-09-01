using UnityEngine;

public class AmmoPickup : PickupBase
{
    [SerializeField] private int ammoAmount = 6;

    protected override bool Apply(GameObject player)
    {
        var shooting = player.GetComponent<ShootingController>();
        if (shooting == null) return false;
        shooting.AddAmmo(ammoAmount);
        return true;
    }
}