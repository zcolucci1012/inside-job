using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cashier : EntityWithHealth
{
    public GameObject message;

    public override void AddHealth(float health)
    {
        base.AddHealth(health);
        GameObject newMessage = Instantiate(message, this.transform.GetChild(0), true);
        newMessage.GetComponent<Message>().ToggleFreeze();
        newMessage.GetComponent<Text>().text = "ow";
        newMessage.GetComponent<Text>().enabled = true;
    }

    protected override void End()
    {
        this.AddHealth(this.TOTAL_HEALTH);
    }
}
