using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeDiscount : Passive
{
    private new void Awake()
    {
        base.Awake();
        passiveName = "Employee Discount";
        passiveMessage = "20% off in the gift shop!";
        shopCost = 30;
    }

    protected override void EffectOnPickup()
    {
        base.EffectOnPickup();

        Pickup[] pickups = GameObject.FindObjectsOfType<Pickup>();
        foreach (Pickup p in pickups)
        {
            p.SetCost(p.GetCost() * 0.8f);
        }
    }

    public override void PassiveUpdate()
    {

    }
}
