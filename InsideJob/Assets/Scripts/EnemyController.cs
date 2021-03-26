using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityWithHealth
{
    public float REWARD;
    public Transform playerTransform;
    public Sprite[] sprites;
    protected bool seePlayer = false;
    protected bool onScreen = false;
    protected float ex;
    protected float ey;
    protected float d;
    private int mask;

    // Start is called before the first frame update
    protected new virtual void Start()
    {
        base.Start();
        mask = LayerMask.GetMask("Wall", "Destructable");
        Physics2D.IgnoreLayerCollision(9, 8);
        Physics2D.IgnoreLayerCollision(9, 10);
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
    }

    protected new virtual void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
