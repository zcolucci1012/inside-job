using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float COST;
    public float BULLET_FORCE;
    public int FIRE_RATE;
    public bool auto = false;
    protected float x;
    protected float y;
    protected float rad;
    protected float rotation;
    protected int fireTick;
    protected GameObject bullet;
    protected GameObject player;
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        fireTick = FIRE_RATE;
        bullet = this.transform.GetChild(0).gameObject;
        Physics2D.IgnoreLayerCollision(8, 11);
        Physics2D.IgnoreLayerCollision(8, 8);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    { 
        if (this.active)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.x = Input.mousePosition.x - Screen.width / 2;
            this.y = Input.mousePosition.y - Screen.height / 2;
            this.rad = Mathf.Atan2(y, x);
            this.rotation = 180 * rad / Mathf.PI;
            this.transform.eulerAngles = new Vector3(0, 0, rotation);
            this.transform.position = new Vector3(this.transform.parent.position.x + Mathf.Cos(rad) * 0.4f,
                this.transform.parent.position.y + Mathf.Sin(rad) * 0.4f - 0.05f,
                this.transform.position.z);
            this.GetComponent<SpriteRenderer>().flipY = x < 0;

            if ((auto && (Input.GetMouseButton(0) || Input.GetKey("space")) && fireTick == FIRE_RATE)
                || !auto && (Input.GetMouseButton(0) || Input.GetKey("space")) && fireTick == FIRE_RATE)
            {
                player.GetComponent<PlayerController>().AddHealth(-COST);
                Fire();
                fireTick = 0;
            }
        } else
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    abstract protected void Fire();

    private void FixedUpdate()
    {
        if (this.active)
        {
            if (fireTick < FIRE_RATE)
            {
                fireTick++;
            }
        }
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }
}
