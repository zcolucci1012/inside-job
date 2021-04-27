using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float COST;
    public float BULLET_FORCE;
    public int FIRE_RATE;
    public bool auto = false;
    public AudioClip sound;
    public float SHOP_COST;
    protected float x;
    protected float y;
    protected float rad;
    protected float rotation;
    protected int fireTick;
    protected GameObject bullet;
    protected GameObject player;
    protected new GameObject camera;
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        fireTick = FIRE_RATE;
        bullet = this.transform.GetChild(0).gameObject;
        Physics2D.IgnoreLayerCollision(8, 11);
        Physics2D.IgnoreLayerCollision(8, 8);
        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    { 
        if (this.active)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.x = Input.mousePosition.x - Camera.main.WorldToScreenPoint(this.transform.parent.position).x;
            this.y = Input.mousePosition.y - Camera.main.WorldToScreenPoint(this.transform.parent.position).y;
            this.rad = Mathf.Atan2(y, x);
            this.rotation = 180 * rad / Mathf.PI;
            this.transform.eulerAngles = new Vector3(0, 0, rotation);

            if (x < -40)
            {
                this.transform.position = new Vector3(this.transform.parent.position.x + Mathf.Cos(rad)
                * ((RectTransform)this.transform).rect.height - 0.2f,
                this.transform.parent.position.y + Mathf.Sin(rad)
                * ((RectTransform)this.transform).rect.width - 0.15f,
                this.transform.parent.position.z - 0.1f);
            }
            else if (x <= 40)
            {
                this.transform.position = new Vector3(this.transform.parent.position.x + Mathf.Cos(rad)
                * ((RectTransform)this.transform).rect.height + 0.005f * x,
                this.transform.parent.position.y + Mathf.Sin(rad)
                * ((RectTransform)this.transform).rect.width - 0.15f,
                this.transform.parent.position.z - 0.1f);
            }
            else
            {
                this.transform.position = new Vector3(this.transform.parent.position.x + Mathf.Cos(rad)
                * ((RectTransform)this.transform).rect.height + 0.2f,
                this.transform.parent.position.y + Mathf.Sin(rad)
                * ((RectTransform)this.transform).rect.width - 0.15f,
                this.transform.parent.position.z - 0.1f);
            }

            if (y > 0 && x > -40 && x <= 40)
            {
                this.transform.position = new Vector3(this.transform.position.x,
                    this.transform.position.y,
                    0);
            }
            
            this.GetComponent<SpriteRenderer>().flipY = x < 0;

            if ((auto && (Input.GetMouseButton(0)) && fireTick == FIRE_RATE)
                || !auto && (Input.GetMouseButton(0)) && fireTick == FIRE_RATE)
            {
                player.GetComponent<PlayerController>().AddHealth(-COST);
                Fire();
                if (sound != null)
                {
                    AudioSource.PlayClipAtPoint(sound, this.transform.position);
                }
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
