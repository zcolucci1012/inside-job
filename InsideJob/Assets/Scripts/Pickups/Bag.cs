using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : Passive
{
    private new void Awake()
    {
        base.Awake();
        passiveName = "Bag of White Stuff";
        passiveMessage = "You feel weird. Run fast to run faster";
        shopCost = 30;
    }

    protected override void EffectOnPickup()
    {
        base.EffectOnPickup();
        player.GetComponent<PlayerController>().RUN_SPEED *= 0.7f;
    }

    public override void PassiveUpdate()
    {
        
    }

    public override void PassiveFixedUpdate()
    {
        if (Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.x) > 0
            || Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.y) > 0)
        {
            if (player.GetComponent<PlayerController>().runMod < 4)
            {
                player.GetComponent<PlayerController>().runMod += 0.02f;
            }
        } else
        {
            player.GetComponent<PlayerController>().runMod = 0;
        }
    }
}
