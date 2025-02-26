﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float COST;
    public float BULLET_FORCE;
    public int FIRE_RATE;
    public int CLIP_SIZE;
    public float RELOAD_TIME;
    public bool auto = false;
    public AudioClip sound;
    public AudioClip reload;
    public float SHOP_COST;

    protected float x;
    protected float y;
    protected float px;
    protected float py;
    protected float rad;
    protected float rotation;
    protected int fireTick;
    protected GameObject bullet;
    protected GameObject player;
    protected new GameObject camera;
    protected int numBullets = 0;
    protected int reloadTick = 0;

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
        numBullets = CLIP_SIZE;
    }

    // Update is called once per frame
    void Update()
    { 
        if (this.active)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.x = Input.mousePosition.x - Camera.main.WorldToScreenPoint(this.transform.position).x;
            this.y = Input.mousePosition.y - Camera.main.WorldToScreenPoint(this.transform.position).y;
            this.px = Input.mousePosition.x - Camera.main.WorldToScreenPoint(player.transform.position).x;
            this.py = Input.mousePosition.y - Camera.main.WorldToScreenPoint(player.transform.position).y;
            this.rad = Mathf.Atan2(y, x);
            this.rotation = 180 * rad / Mathf.PI;
            this.transform.eulerAngles = new Vector3(0, 0, rotation);

            if (px < -40)
            {
                this.transform.position = new Vector3(player.transform.position.x + Mathf.Cos(rad)
                * ((RectTransform)this.transform).rect.height - 0.2f,
                player.transform.position.y + Mathf.Sin(rad)
                * ((RectTransform)this.transform).rect.width - 0.15f,
                player.transform.position.z - 0.1f);
            }
            else if (px <= 40)
            {
                this.transform.position = new Vector3(player.transform.position.x + Mathf.Cos(rad)
                    * ((RectTransform)this.transform).rect.height + 0.005f * px,
                    player.transform.position.y + Mathf.Sin(rad)
                    * ((RectTransform)this.transform).rect.width - 0.15f,
                    player.transform.position.z - 0.1f);
            }
            else
            {
                this.transform.position = new Vector3(player.transform.position.x + Mathf.Cos(rad)
                * ((RectTransform)this.transform).rect.height + 0.2f,
                player.transform.position.y + Mathf.Sin(rad)
                * ((RectTransform)this.transform).rect.width - 0.15f,
                player.transform.position.z - 0.1f);
            }

            if (y > 0 && px > -40 && px <= 40)
            {
                this.transform.position = new Vector3(this.transform.position.x,
                    this.transform.position.y,
                    0);
            }

            this.GetComponent<SpriteRenderer>().flipY = x < 0;

            if (numBullets > 0
                && ((auto && (Input.GetMouseButton(0)) && fireTick >= FIRE_RATE)
                || !auto && (Input.GetMouseButtonDown(0)) && fireTick >= FIRE_RATE))
            {
                player.GetComponent<PlayerController>().AddHealth(-COST, false);
                Fire();
                numBullets--;
                if (sound != null)
                {
                    AudioSource.PlayClipAtPoint(sound, this.transform.position);
                }
                fireTick = 0;
            }

            if (Input.GetKeyDown("r"))
            {
                numBullets = 0;
            }
            
        } else
        {
            reloadTick = 0;
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
            if (this.numBullets <= 0)
            {
                if (reloadTick == 0)
                {
                    AudioSource.PlayClipAtPoint(reload, this.transform.position);
                }
                reloadTick++;
                if (reloadTick == RELOAD_TIME)
                {
                    reloadTick = 0;
                    numBullets = CLIP_SIZE;
                }
            }
        }
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }

    public int GetNumBullets()
    {
        return this.numBullets;
    }

    public int GetReloadTick()
    {
        return this.reloadTick;
    }
}
