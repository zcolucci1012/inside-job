using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive : Pickup
{
    protected string passiveName;
    protected string passiveMessage;

    protected override void EffectOnPickup()
    {
        player.GetComponent<PlayerController>().AddPassive(this);
        ui.SpawnPickupMessage("You picked up " + passiveName + "!\n" + passiveMessage);
    }

    public abstract void PassiveUpdate();
}
