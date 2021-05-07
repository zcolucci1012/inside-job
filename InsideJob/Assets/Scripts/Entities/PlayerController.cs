using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : EntityWithHealth
{
    public float RUN_SPEED = 6.0f;
    public UIController ui;
    public WeaponInventory weaponInventory;
    public Sprite[] sprites;
    public Sprite[] altSprites;
    public AudioClip hurt;

    private SpriteRenderer spriteRenderer;
    private int direction = 1;
    private bool canMove = true;
    private int walkTime = 0;
    private int WALK_INTERVAL = 10;
    private int I_FRAMES = 100;
    private int iFrameTick = 0;
    private Sprite[] currSprites;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
        currSprites = sprites;
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
        if (Input.GetKeyDown("k"))
        {
            currSprites = altSprites;
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
                spriteRenderer.sprite = currSprites[(direction - 1) * 2 + 1];
            } else
            {
                spriteRenderer.sprite = currSprites[(direction - 1) * 2];
            }
            walkTime++;
        } else
        {
            spriteRenderer.sprite = currSprites[(direction - 1) * 2];
            walkTime = 0;
        }
        //Debug.Log(this.transform.position.x + ", " + this.transform.position.y);
        if (iFrameTick > 0)
        {
            iFrameTick--;
            if (iFrameTick % (I_FRAMES / 4) >= I_FRAMES / 16) {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            }
            else
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            }
            
        } else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }

    public void CanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public override void AddHealth(float health)
    {
        AddHealth(health, true);
    }
    
    /// <summary>
    /// <param name="ext">is the damage from an external source? (i.e. gives player I-Frames)</param>
    /// </summary>
    public void AddHealth(float health, bool ext)
    {
        if (!ext || iFrameTick <= 0)
        {
            if (ext && health < 0)
            {
                iFrameTick = I_FRAMES;
                AudioSource.PlayClipAtPoint(hurt, this.transform.position, 4);
            }
            base.AddHealth(health);
            if (ui != null)
            {
                ui.SpawnParticle(health);
            }
        }
        
    }

    protected override void End()
    {
        SceneManager.LoadScene("GameOver");
    }
}
