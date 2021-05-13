using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarketerController : EnemyController
{
    // Start is called before the first frame update
    public GameObject bullet;
    private GameObject newBullet;
    public int ATTACK_INTERVAL;
    public int ATTACK_TIME;
    public int THROW_ANIM_LENGTH;
    public float SIGHT_RANGE;
    public AudioClip sound;
    private int attackTick = 0;
    private int animTick = 0;
    private int throwTick = 0;
    private bool justSaw = true;
    private bool throwing = false;
    private Func<int, Vector2> path;
    private int sign = 1;

    new void Awake()
    {
        base.Awake();
        attackTick = UnityEngine.Random.Range(0, ATTACK_INTERVAL / 2);
        Physics2D.IgnoreLayerCollision(9, 8);
        Physics2D.IgnoreLayerCollision(9, 10);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (seePlayer && onScreen && awake)
        {
            if (ex > 0)
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
                bullet.transform.position = new Vector3(this.transform.position.x - 0.4f,
                    this.transform.position.y + 0.475f,
                    this.bullet.transform.position.z);
            }
            else
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
                this.bullet.transform.position = new Vector3(this.transform.position.x + 0.4f,
                    this.transform.position.y + 0.475f,
                    this.bullet.transform.position.z);
            }

            float x = playerTransform.position.x - this.bullet.transform.position.x;
            float y = playerTransform.position.y - this.bullet.transform.position.y;
            float rad = Mathf.Atan2(y, x);
            float rotation = 180 * rad / Mathf.PI;
            //bullet.transform.eulerAngles = new Vector3(0, 0, rotation);

            if (attackTick >= ATTACK_INTERVAL)
            {
                this.newBullet = Instantiate(bullet, this.transform, true);
                this.newBullet.transform.position = new Vector3(this.bullet.transform.position.x,
                    this.bullet.transform.position.y,
                    this.bullet.transform.position.z);
                print(newBullet.transform.position.z);
                this.newBullet.GetComponent<SpriteRenderer>().enabled = true;
                this.newBullet.GetComponent<BoxCollider2D>().enabled = true;
                var adj = new Vector3(0, 0, 0);// (playerTransform.position - newBullet.transform.position).normalized / 2;
                path = GeneratePath(newBullet.transform.position, playerTransform.position + adj, ATTACK_TIME);
                throwing = true;
                attackTick = 0;
                animTick = THROW_ANIM_LENGTH;
                sign = -sign;

                AudioSource.PlayClipAtPoint(sound, this.playerTransform.position, 1f);
            }
        }
    }

    private Func<int, Vector2> GeneratePath(Vector2 start, Vector2 end, int totalTime)
    {
        float cx = (start.x + end.x) / 2;
        float cy = (start.y + end.y) / 2;
        float rmaj = (end - start).magnitude / 2;
        float rmin = 1.5f;
        float rad = Mathf.Atan2(end.y - start.y, end.x - start.x);

        //print(cx + ", " + cy + ", " + rmaj + ", " + rmin + ", " + rad);
        
        Func<int, Vector2> path = new Func<int, Vector2>(TimeToPoint);

        Vector2 TimeToPoint(int time)
        {
            float adjTime = (time * 2 * Mathf.PI / totalTime) + Mathf.PI;
            float x = rmaj * Mathf.Cos(adjTime) * Mathf.Cos(rad) -
                rmin * Mathf.Sin(adjTime) * Mathf.Sin(rad) +
                cx;
            float y = rmaj * Mathf.Cos(adjTime) * Mathf.Sin(rad) +
                rmin * Mathf.Sin(adjTime) * Mathf.Cos(rad) +
                cy;

            return new Vector2(x, y);
        }

        return path;
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (throwing)
        {
            if (this.newBullet != null)
            {
                this.newBullet.transform.position = path(sign * throwTick);
                this.newBullet.transform.eulerAngles += new Vector3(0, 0, 20);
                throwTick++;

                if (throwTick < ATTACK_TIME / 10)
                {
                    this.newBullet.GetComponent<BoxCollider2D>().enabled = false;
                } else
                {
                    this.newBullet.GetComponent<BoxCollider2D>().enabled = true;
                }

                if (throwTick >= ATTACK_TIME)
                {
                    throwTick = 0;
                    throwing = false;
                    Destroy(this.newBullet);
                }
            } else
            {
                throwTick = 0;
                throwing = false;
            }
        }
        if (justSaw && seePlayer && onScreen && awake)
        {
            attackTick = ATTACK_INTERVAL / 2;
        }
        if (seePlayer && onScreen && awake)
        {
            justSaw = false;
            attackTick++;

            if (animTick == 0)
            {
                this.GetComponent<SpriteRenderer>().sprite = this.sprites[0];
                this.bullet.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                this.GetComponent<SpriteRenderer>().sprite = this.sprites[1];
                this.bullet.GetComponent<SpriteRenderer>().enabled = false;
                animTick--;
            }
        }
        else
        {
            justSaw = true;
            attackTick = 0;
            animTick = 0;
        }
    }
}
