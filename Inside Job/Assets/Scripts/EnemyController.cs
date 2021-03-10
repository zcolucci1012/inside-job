using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public Transform playerTransform;
    public int ATTACK_INTERVAL;
    public float BULLET_FORCE;
    public int THROW_ANIM_LENGTH;
    public Sprite[] throwing;
    private int attackTick;
    private int animTick;

    void Start()
    {
        attackTick = Random.Range(0, ATTACK_INTERVAL / 2);
    }

    // Update is called once per frame
    void Update()
    {
        float ex = playerTransform.position.x - this.transform.position.x;
        if (ex > 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            bullet.transform.position = new Vector3(this.transform.position.x + 0.3f,
                this.transform.position.y + 0.375f,
                this.bullet.transform.position.z);
        }
        else 
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            this.bullet.transform.position = new Vector3(this.transform.position.x - 0.3f,
                this.transform.position.y + 0.475f,
                this.bullet.transform.position.z);
        }

        float x = playerTransform.position.x - this.bullet.transform.position.x;
        float y = playerTransform.position.y - this.bullet.transform.position.y;
        float rad = Mathf.Atan2(y, x);
        float rotation = 180 * rad / Mathf.PI;
        bullet.transform.eulerAngles = new Vector3(0, 0, rotation);

        Debug.Log(this.bullet.transform.position.x + ", " + this.bullet.transform.position.y);

        if (attackTick >= ATTACK_INTERVAL)
        {
            GameObject newBullet = Instantiate(bullet, this.transform, true);
            newBullet.transform.position = new Vector3(this.bullet.transform.position.x,
                this.bullet.transform.position.y,
                bullet.transform.position.z);
            newBullet.GetComponent<SpriteRenderer>().enabled = true;
            newBullet.GetComponent<BoxCollider2D>().enabled = true;
            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(Mathf.Cos(rad) * BULLET_FORCE, Mathf.Sin(rad) * BULLET_FORCE, 0));
            Physics2D.IgnoreCollision(newBullet.GetComponent<BoxCollider2D>(),
                this.transform.gameObject.GetComponent<BoxCollider2D>()); //enemy
            Physics2D.IgnoreCollision(newBullet.GetComponent<BoxCollider2D>(),
                playerTransform.GetChild(0).GetChild(0).gameObject.GetComponent<CircleCollider2D>()); //player bullet
            attackTick = 0;
            animTick = THROW_ANIM_LENGTH;
        }

        
    }

    private void FixedUpdate()
    {
        attackTick++;

        if (animTick == 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = this.throwing[0];
            this.bullet.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = this.throwing[1];
            this.bullet.GetComponent<SpriteRenderer>().enabled = false;
            animTick--;
        }
    }
}
