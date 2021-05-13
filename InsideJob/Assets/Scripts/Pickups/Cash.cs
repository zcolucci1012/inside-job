using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : Pickup
{
    public float value = 25;

    new void Awake()
    {
        base.Awake();
        this.shopCost = 0;
    }

    protected override void EffectOnPickup()
    {
        player.GetComponent<PlayerController>().AddHealth(value);
    }
}
