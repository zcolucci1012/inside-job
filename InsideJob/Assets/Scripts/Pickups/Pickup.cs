using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    protected GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Player") 
        {
            if (Input.GetKey("e"))
            {
                EffectOnPickup();
                Destroy(this.gameObject);
            }
        }
    }

    protected abstract void EffectOnPickup();
}
