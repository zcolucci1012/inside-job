using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : EntityWithHealth
{
    public float RUN_SPEED = 6.0f;
    public UIController ui;
    private bool canMove = true;

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
        if (canMove)
        {
            float xVel = 0;
            float yVel = 0;
            int dx = 0;
            int dy = 0;

            if (Input.GetKey("up") || Input.GetKey("w"))
            {
                yVel += RUN_SPEED;
                dy++;
            }
            if (Input.GetKey("left") || Input.GetKey("a"))
            {
                xVel -= RUN_SPEED;
                dx--;
            }
            if (Input.GetKey("down") || Input.GetKey("s"))
            {
                yVel -= RUN_SPEED;
                dy--;
            }
            if (Input.GetKey("right") || Input.GetKey("d"))
            {
                xVel += RUN_SPEED;
                dx++;
            }

            if (dx == 1 && dy == 1)
            {
                xVel = RUN_SPEED * Mathf.Sqrt(2) / 2;
                yVel = RUN_SPEED * Mathf.Sqrt(2) / 2;
            }
            else if (dx == -1 && dy == 1)
            {
                xVel = -RUN_SPEED * Mathf.Sqrt(2) / 2;
                yVel = RUN_SPEED * Mathf.Sqrt(2) / 2;
            }
            else if (dx == -1 && dy == -1)
            {
                xVel = -RUN_SPEED * Mathf.Sqrt(2) / 2;
                yVel = -RUN_SPEED * Mathf.Sqrt(2) / 2;
            }
            else if (dx == 1 && dy == -1)
            {
                xVel = RUN_SPEED * Mathf.Sqrt(2) / 2;
                yVel = -RUN_SPEED * Mathf.Sqrt(2) / 2;
            }

            rigidbody.velocity = new Vector2(xVel, yVel);
        } else
        {
            rigidbody.velocity = new Vector2(0f, 0f);
        }
        //Debug.Log(this.transform.position.x + ", " + this.transform.position.y);
    }

    public void CanMove(bool canMove)
    {
        this.canMove = canMove;
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
