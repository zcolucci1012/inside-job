using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : Passive
{
    private new void Awake()
    {
        base.Awake();
        passiveName = "The Miracle Crowbar";
        passiveMessage = "Start breaking through file cabinets with ease!";
        shopCost = 30;
    }

    protected override void EffectOnPickup()
    {
        base.EffectOnPickup();
        FileCabinet[] fileCabinets = Resources.FindObjectsOfTypeAll<FileCabinet>();
        foreach (FileCabinet fc in fileCabinets)
        {
            fc.SetHasCrowbar(true);
        }
    }

    public override void PassiveUpdate()
    {

    }
}
