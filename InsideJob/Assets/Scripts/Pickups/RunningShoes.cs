using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningShoes : Passive
{
    private new void Awake()
    {
        base.Awake();
        passiveName = "Running Shoes";
        passiveMessage = "Run real fast!";
        shopCost = 30;
    }

    protected override void EffectOnPickup()
    {
        base.EffectOnPickup();
        player.GetComponent<PlayerController>().RUN_SPEED *= 1.2f;
    }

    public override void PassiveUpdate()
    {
        
    }
}
