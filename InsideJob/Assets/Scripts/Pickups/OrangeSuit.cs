using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeSuit : Passive
{
    private new void Awake()
    {
        base.Awake();
        passiveName = "Orange Suit";
        passiveMessage = "It feels pretty protective.";
        shopCost = 30;
    }

    protected override void EffectOnPickup()
    {
        base.EffectOnPickup();
        player.GetComponent<PlayerController>().ARMOR_MOD *= 0.8f;
    }

    public override void PassiveUpdate()
    {

    }
}
