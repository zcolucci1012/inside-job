using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityWithHealth
{
    protected int ROOM_WIDTH = Constants.ROOM_WIDTH;
    protected int ROOM_HEIGHT = Constants.ROOM_HEIGHT;
    public float REWARD;
    protected Transform playerTransform;
    public Sprite[] sprites;
    protected bool seePlayer = false;
    protected bool onScreen = false;
    protected float ex;
    protected float ey;
    protected float d;
    protected float sameRoom;
    private int mask;
    protected bool awake = false;

    protected override void Awake()
    {
        base.Awake();
        mask = LayerMask.GetMask("Wall", "Destructable");
        Physics2D.IgnoreLayerCollision(9, 8);
        Physics2D.IgnoreLayerCollision(9, 10);
        playerTransform = GameObject.Find("Player").transform;
    }

    protected override void End()
    {
        playerTransform.gameObject.GetComponent<PlayerController>().AddHealth(REWARD);
        GridData.grid[currCell] = "";
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        this.ex = playerTransform.position.x - this.transform.position.x;
        this.ey = playerTransform.position.y - this.transform.position.y;
        this.d = Mathf.Sqrt(Mathf.Pow(ex, 2) + Mathf.Pow(ey, 2));
        this.seePlayer = !Physics2D.Raycast(new Vector2(this.transform.position.x,
            this.transform.position.y),
            new Vector2(this.ex, this.ey),
            this.d,
            mask);
        this.onScreen = this.GetComponent<SpriteRenderer>().isVisible;
        if (!this.awake)
        {
            this.awake = (InSameRoom(this.playerTransform.position, this.transform.position) || this.onScreen);
        }
    }

    public bool InSameRoom(Vector3 e1, Vector3 e2)
    {
        int rx1 = (int)((e1.x + ROOM_WIDTH / 2) / ROOM_WIDTH);
        int ry1 = (int)((e1.y + ROOM_HEIGHT / 2) / ROOM_HEIGHT);
        int rx2 = (int)((e2.x + ROOM_WIDTH / 2) / ROOM_WIDTH);
        int ry2 = (int)((e2.y + ROOM_HEIGHT / 2) / ROOM_HEIGHT);
        if (e1.x + ROOM_WIDTH / 2 < 0)
        {
            rx1--;
        }
        if (e1.y + ROOM_HEIGHT/ 2 < 0)
        {
            ry1--;
        }
        if (e2.x + ROOM_WIDTH / 2 < 0)
        {
            rx2--;
        }
        if (e2.y + ROOM_HEIGHT / 2 < 0)
        {
            ry2--;
        }
        if (e1.x < rx1 * ROOM_WIDTH - ROOM_WIDTH / 2 + 1
            || e1.x > rx1 * ROOM_WIDTH + ROOM_WIDTH / 2 - 1
            || e1.y < ry1 * ROOM_HEIGHT - ROOM_HEIGHT / 2 + 1
            || e1.y > ry1 * ROOM_HEIGHT + ROOM_HEIGHT / 2 - 1)
        {
            return false;
        }
        //print(e1.x + ", " + e1.y);
        //print(rx1 + ", " + ry1 + ", " + e1.x + ", " + e1.y);
        return (rx1 == rx2) && (ry1 == ry2);
    }
}
