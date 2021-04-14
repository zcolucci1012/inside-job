using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityWithHealth
{
    public int ROOM_WIDTH = 20;
    public int ROOM_HEIGHT = 16;
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

    // Start is called before the first frame update
    protected new virtual void Start()
    {
        base.Start();
        mask = LayerMask.GetMask("Wall", "Destructable");
        Physics2D.IgnoreLayerCollision(9, 8);
        Physics2D.IgnoreLayerCollision(9, 10);
        playerTransform = GameObject.Find("Player").transform;
    }

    protected override void End()
    {
        playerTransform.gameObject.GetComponent<PlayerController>().AddHealth(REWARD);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
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
            this.awake = InSameRoom(this.playerTransform.position, this.transform.position);
        }
    }

    protected new virtual void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public bool InSameRoom(Vector3 e1, Vector3 e2)
    {
        
        int rx1 = (int)Mathf.Sign(e1.x) * (int)((Mathf.Abs(e1.x) + ROOM_WIDTH / 2) / ROOM_WIDTH);
        int ry1 = (int)Mathf.Sign(e1.y) * (int)((Mathf.Abs(e1.y) + ROOM_HEIGHT / 2) / ROOM_HEIGHT);
        int rx2 = (int)Mathf.Sign(e2.x) * (int)((Mathf.Abs(e2.x) + ROOM_WIDTH / 2) / ROOM_WIDTH);
        int ry2 = (int)Mathf.Sign(e2.y) * (int)((Mathf.Abs(e2.y) + ROOM_HEIGHT / 2) / ROOM_HEIGHT);
        if ((Mathf.Abs(e1.x) + ROOM_WIDTH / 2) % ROOM_WIDTH < 1.5
            || (Mathf.Abs(e2.x) + ROOM_WIDTH / 2) % ROOM_WIDTH < 1.5
            || (Mathf.Abs(e1.y) + ROOM_HEIGHT / 2) % ROOM_HEIGHT < 1.5
            || (Mathf.Abs(e2.y) + ROOM_HEIGHT / 2) % ROOM_HEIGHT < 1.5)
        {
            return false;
        }
        //print(rx1 + ", " + ry1);
        return (rx1 == rx2) && (ry1 == ry2);
    }
}
