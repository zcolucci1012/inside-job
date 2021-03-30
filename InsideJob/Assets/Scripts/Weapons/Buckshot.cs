using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buckshot : Weapon
{
    private int PELLETS = 5;

    protected override void Fire()
    {
        for (int ii = 0; ii < PELLETS; ii++)
        {
            float offset = Random.Range(-0.25f, 0.25f);
            GameObject newBullet = Instantiate(bullet, this.transform, true);
            newBullet.transform.SetParent(this.transform.parent.parent);
            newBullet.GetComponent<SpriteRenderer>().enabled = true;
            newBullet.GetComponent<CircleCollider2D>().enabled = true;
            newBullet.GetComponent<Rigidbody2D>().isKinematic = false;
            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(Mathf.Cos(rad + offset) * BULLET_FORCE, Mathf.Sin(rad + offset) * BULLET_FORCE, 0));
        }  
    }
}
