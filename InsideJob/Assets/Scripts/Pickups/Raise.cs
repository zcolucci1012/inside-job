using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raise : Passive
{
    private bool[] roomOver = null;

    private new void Awake()
    {
        base.Awake();
        passiveName = "Raise";
        passiveMessage = "Get $10 per room completed!";
        shopCost = 30;
    }

    protected override void EffectOnPickup()
    {
        base.EffectOnPickup();
        player.GetComponent<PlayerController>().RUN_SPEED *= 1.2f;
    }

    public override void PassiveUpdate()
    {
        bool[] newRoomOver = GameObject.Find("Grid").GetComponent<DrawLevel>().GetRoomOver();

        //string str2 = "old: ";
        //if (roomOver != null)
        //{
        //    for (int ii = 0; ii < roomOver.Length; ii++)
        //    {
        //        str2 += roomOver[ii] + ", ";
        //    }
        //    print(str2);
        //}
        //string str = "new: ";
        //for (int ii = 0; ii < newRoomOver.Length; ii++)
        //{
        //    str += newRoomOver[ii] + ", ";
        //}
        //print(str);
        

        for (int ii = 0; ii < newRoomOver.Length; ii++)
        {
            if (roomOver != null && (newRoomOver[ii] != roomOver[ii]))
            {
                player.GetComponent<PlayerController>().AddHealth(10);
                //print("done");
            }
        }

        roomOver = (bool[])newRoomOver.Clone();
        
    }
}
