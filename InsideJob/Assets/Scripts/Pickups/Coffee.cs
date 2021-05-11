using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Passive
{
    private new void Awake()
    {
        base.Awake();
        passiveName = "The Boss's Coffee";
        passiveMessage = "You feel jumpy, start shooting faster!";
        shopCost = 30;
    }

    protected override void EffectOnPickup()
    {
        base.EffectOnPickup();

        Weapon[] weapons = GameObject.FindObjectsOfType<Weapon>();
        foreach (Weapon w in weapons)
        {
            w.FIRE_RATE = (int)(w.FIRE_RATE * 0.75f);
        }
    }

    public override void PassiveUpdate()
    {

    }


}
