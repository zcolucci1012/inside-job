using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaplerController : EnemyController
{
    public float SPEED = 5f;
    private bool bite = false;
    private int biteTicks = 0;
    private int BITE_TICKS = 15;
    private int cooldownTicks = 0;
    private int COOLDOWN_TICKS = 30;
    public float DAMAGE = 30;
    private int idleTicks = 0;
    private int IDLE_TICKS = 120;

    protected override void End()
    {
        this.playerTransform.GetComponent<PlayerController>().CanMove(true);
        base.End();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!bite && awake)
        {
            SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
            if (idleTicks > (7 * IDLE_TICKS) / 8)
            {
                renderer.sprite = sprites[1];
            } else
            {
                renderer.sprite = sprites[0];
            }
            renderer.flipX = ex > 0;

            float cos = this.ex / this.d;
            float sin = this.ey / this.d;

            this.GetComponent<Rigidbody2D>().velocity = new Vector2(SPEED * cos, SPEED * sin);

            
        } else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (cooldownTicks > 0)
        {
            cooldownTicks--;
        }
        if (bite)
        {
            if (biteTicks < BITE_TICKS / 6)
            {
                this.GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
            else if (biteTicks < BITE_TICKS - BITE_TICKS / 6)
            {
                this.GetComponent<SpriteRenderer>().sprite = sprites[2];
            } else if (biteTicks < BITE_TICKS - 1)
            {
                this.GetComponent<SpriteRenderer>().sprite = sprites[1];
            } else
            {
                this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                PlayerController player = this.playerTransform.gameObject.GetComponent<PlayerController>();
                player.CanMove(true);
                cooldownTicks = COOLDOWN_TICKS;
                bite = false;
                biteTicks = 0;
            }
            biteTicks++;
        } else
        {
            if (idleTicks == IDLE_TICKS)
            {
                idleTicks = 0;
            }
            idleTicks++;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            if (!bite && cooldownTicks == 0)
            {
                this.bite = true;
                PlayerController player = collider.gameObject.GetComponent<PlayerController>();
                player.AddHealth(-DAMAGE);
                player.CanMove(false);
            }
        }
    }

}
