using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : EntityWithHealth
{
    public float RUN_SPEED = 6.0f;
    public UIController ui;
    public WeaponInventory weaponInventory;
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int direction = 1;
    private bool canMove = true;
    private int walkTime = 0;
    private int WALK_INTERVAL = 10;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    new void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.LeftShift)
            || Input.GetKeyDown(KeyCode.RightShift)
            || Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            weaponInventory.IncWeapon();
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            weaponInventory.DecWeapon();
        }

        if (Input.GetKeyDown("j"))
        {
            GridData.PrintGrid();
        }

        if (Input.mousePosition.x < Camera.main.WorldToScreenPoint(this.transform.position).x)
        {
            if (Input.mousePosition.y > 5 * Screen.height / 8)
            {
                direction = 2;
            } else
            {
                direction = 3;
            }
        } else
        {
            if (Input.mousePosition.y > 5 * Screen.height / 8)
            {
                direction = 1;
            } else
            {
                direction = 4;
            }
        }
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        float xVel = 0;
        float yVel = 0;
        int dx = 0;
        int dy = 0;
        if (canMove)
        {
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

        if (Mathf.Abs(xVel) > 0 || Mathf.Abs(yVel) > 0)
        {
            if (walkTime % WALK_INTERVAL < WALK_INTERVAL / 2)
            {
                spriteRenderer.sprite = sprites[(direction - 1) * 2 + 1];
            } else
            {
                spriteRenderer.sprite = sprites[(direction - 1) * 2];
            }
            walkTime++;
        } else
        {
            spriteRenderer.sprite = sprites[(direction - 1) * 2];
            walkTime = 0;
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
