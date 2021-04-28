using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payshooter : Weapon
{
    protected override void Fire()
    {
        GameObject newBullet = Instantiate(bullet, this.transform, true);
        newBullet.transform.SetParent(this.transform.parent.parent);
        newBullet.GetComponent<SpriteRenderer>().enabled = true;
        newBullet.GetComponent<CircleCollider2D>().enabled = true;
        newBullet.GetComponent<Rigidbody2D>().isKinematic = false;
        newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * BULLET_FORCE);
    }
}
