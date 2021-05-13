using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileCabinet : EntityWithHealth
{
    public Sprite[] sprites;
    public AudioClip destroyed;
    private SpriteRenderer render;
    private bool dead = false;
    private int deadTicks = 10;
    private bool hasCrowbar = false;

    protected override void End()
    {
        this.render.sprite = sprites[3];
        AudioSource.PlayClipAtPoint(destroyed, GameObject.Find("Player").transform.position);
        dead = true;
    }

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        render = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
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
                    float rad = Random.Range(0, 2 * Mathf.PI);
                    pickup.GetComponent<Rigidbody2D>().AddForce(40f * new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)));
                }
                GridData.grid[currCell] = "";
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player" && hasCrowbar)
        {
            this.AddHealth(-1000);
        }
    }

    public void SetHasCrowbar(bool hasCrowbar)
    {
        this.hasCrowbar = hasCrowbar;
    }
}
