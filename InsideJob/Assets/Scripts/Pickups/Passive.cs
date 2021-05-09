using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : Pickup
{
    protected string passiveName;

    protected override void EffectOnPickup()
    {
        player.GetComponent<PlayerController>().AddPassive(this);
        ui.SpawnPickupMessage("You picked up " + passiveName + "!");
    }
}
