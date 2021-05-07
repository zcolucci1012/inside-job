using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Pickup : MonoBehaviour
{
    protected GameObject player;
    protected GameObject eKey;
    protected GameObject cost;
    protected UIController ui;
    protected float shopCost;

    // Start is called before the first frame update
    protected void Awake()
    {
        player = GameObject.Find("Player");
        eKey = GameObject.Find("E");
        cost = GameObject.Find("Cost");
        ui = GameObject.Find("/Canvas").GetComponent<UIController>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Player") 
        {
            if (shopCost != 0)
            {
                cost.GetComponent<Text>().enabled = true;
                cost.GetComponent<Text>().text = "-$" + shopCost;
                eKey.transform.parent.localPosition = new Vector3(30, 0, 0);
            } else
            {
                cost.GetComponent<Text>().enabled = false;
                eKey.transform.parent.localPosition = new Vector3(0, 0, 0);
            }
            eKey.GetComponent<Text>().enabled = true;
            eKey.transform.parent.gameObject.GetComponent<Image>().enabled = true;
            if (Input.GetKey("e"))
            {
                if (shopCost != 0)
                {
                    player.GetComponent<PlayerController>().AddHealth(-shopCost, false);
                }
                cost.GetComponent<Text>().enabled = false;
                eKey.GetComponent<Text>().enabled = false;
                eKey.transform.parent.gameObject.GetComponent<Image>().enabled = false;
                EffectOnPickup();
                Destroy(this.gameObject);
            }
        }
    }

    public void SetCost(float cost)
    {
        this.shopCost = cost;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            cost.GetComponent<Text>().enabled = false;
            eKey.GetComponent<Text>().enabled = false;
            eKey.transform.parent.gameObject.GetComponent<Image>().enabled = false;
        }
    }

    protected abstract void EffectOnPickup();
}
