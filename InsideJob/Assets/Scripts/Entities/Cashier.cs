using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cashier : EntityWithHealth
{
    public GameObject message;
    private int messageNum = 0;

    public override void AddHealth(float health)
    {
        base.AddHealth(health);
        GameObject newMessage = Instantiate(message, this.transform.GetChild(0), true);
        newMessage.GetComponent<Message>().ToggleFreeze();
        string msg;
        switch(messageNum)
        {
            case 2:
                msg = "seriously, ow";
                break;
            case 3:
                msg = "please stop, that hurts";
                break;
            case 4:
                msg = "owwwww";
                break;
            case 5:
                msg = "this isn't gonna get you a discount";
                break;
            case 6:
                msg = "bruh";
                break;
            case 7:
                msg = "this is trivial";
                break;
            case 8:
                msg = "you're getting on my nerves";
                break;
            case 9:
                msg = "ok im done talking to you";
                break;
            default:
                msg = "ow";
                break;
        }
        newMessage.GetComponent<Text>().text = msg;
        newMessage.GetComponent<Text>().enabled = true;
        messageNum++;
    }

    protected override void End()
    {
        this.AddHealth(this.TOTAL_HEALTH);
    }
}
