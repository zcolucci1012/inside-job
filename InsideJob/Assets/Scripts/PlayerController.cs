using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : EntityWithHealth
{
    public float RUN_SPEED = 6.0f;
    public UIController ui;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        float xVel = 0;
        float yVel = 0;

        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            yVel += RUN_SPEED;
        }
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            xVel -= RUN_SPEED;
        }
        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            yVel -= RUN_SPEED;
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            xVel += RUN_SPEED;
        }

        rigidbody.velocity = new Vector2(xVel, yVel);

        //Debug.Log(this.transform.position.x + ", " + this.transform.position.y);
    }

    public override void AddHealth(float health)
    {
        base.AddHealth(health);
        if (ui != null)
        {
            ui.SpawnParticle(health);
        }
    }

    protected override void End()
    {
        SceneManager.LoadScene("GameOver");
    }
}
