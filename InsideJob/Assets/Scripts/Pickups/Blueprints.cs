using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprints : Passive
{
    private new void Awake()
    {
        base.Awake();
        passiveName = "Blueprints";
        passiveMessage = "The office is your oyster.";
        shopCost = 30;
    }

    protected override void EffectOnPickup()
    {
        base.EffectOnPickup();
        var maps = Resources.FindObjectsOfTypeAll<Minimap>();
        foreach (Minimap m in maps)
        {
            m.SetHasBlueprints(true);
        }
    }

    public override void PassiveUpdate()
    {

    }
}
