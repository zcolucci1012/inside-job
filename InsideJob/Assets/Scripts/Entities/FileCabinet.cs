using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileCabinet : EntityWithHealth
{
    public Sprite[] sprites;
    private SpriteRenderer render;
    private bool dead = false;
    private int deadTicks = 10;

    protected override void End()
    {
        this.render.sprite = sprites[3];
        dead = true;
    }

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        render = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentHealth == this.TOTAL_HEALTH)
        {
            this.render.sprite = sprites[0];
        }
        else if (this.currentHealth >  this.TOTAL_HEALTH / 2)
        {
            this.render.sprite = sprites[1];
        } else if (this.currentHealth > 0)
        {
            this.render.sprite = sprites[2];
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (dead)
        {
            deadTicks--;
            if (deadTicks <= 0)
            {
                GameObject pickup = LootTables.FileCabinet();
                if (pickup != null)
                {
                    pickup.transform.position = this.transform.position;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
