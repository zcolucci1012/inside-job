using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : Pickup
{

    protected override void EffectOnPickup()
    {
        float value = LootTables.CheckValues();
        ui.SpawnPickupMessage("You got a check for $" + value);
        player.GetComponent<PlayerController>().AddHealth(value);
    }
}
