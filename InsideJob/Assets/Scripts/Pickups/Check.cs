using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : Pickup
{

    protected override void EffectOnPickup()
    {
        player.GetComponent<PlayerController>().AddHealth(LootTables.CheckValues());
    }
}
