using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public float COST;
    public GameObject player;
    // Start is called before the first frame update

    void Start()
    {
        if (player.GetComponent<PlayerController>().GetHealth() > COST)
        {
            player.GetComponent<PlayerController>().AddHealth(-COST);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
