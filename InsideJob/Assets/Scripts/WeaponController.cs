using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float BULLET_FORCE;
    public int FIRE_RATE;
    private int fireTick;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        fireTick = FIRE_RATE;
    }

    // Update is called once per frame
    void Update()
    {
        bullet.transform.position = this.transform.position;

        float x = Input.mousePosition.x - Screen.width / 2;
        float y = Input.mousePosition.y - Screen.height / 2;
        float rad = Mathf.Atan2(y, x);
        float rotation = 180 * rad / Mathf.PI;
        transform.eulerAngles = new Vector3(0, 0, rotation);
        transform.position = new Vector3(this.transform.parent.position.x + Mathf.Cos(rad) * 0.4f,
            this.transform.parent.position.y + Mathf.Sin(rad) * 0.4f - 0.05f,
            this.transform.position.z);
        this.GetComponent<SpriteRenderer>().flipY = x < 0;

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) && fireTick == FIRE_RATE)
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = new Vector3(this.transform.position.x + Mathf.Cos(rad) * 0.2f,
                this.transform.position.y + Mathf.Sin(rad) * 0.2f,
                bullet.transform.position.z);
            newBullet.transform.SetParent(this.transform.parent.parent);
            newBullet.GetComponent<SpriteRenderer>().enabled = true;
            newBullet.GetComponent<CircleCollider2D>().enabled = true;
            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(Mathf.Cos(rad) * BULLET_FORCE, Mathf.Sin(rad) * BULLET_FORCE, 0));
            Physics2D.IgnoreCollision(newBullet.GetComponent<CircleCollider2D>(),
                this.transform.parent.gameObject.GetComponent<BoxCollider2D>());
            fireTick = 0;
        }
    }

    private void FixedUpdate()
    {
        if (fireTick < FIRE_RATE)
        {
            fireTick++;
        }
    }
}
