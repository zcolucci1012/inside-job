using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpstock : Weapon
{
    protected override void Fire()
    {
        float offset = Random.Range(-0.1f, 0.1f);
        GameObject newBullet = Instantiate(bullet, this.transform, true);
        newBullet.transform.SetParent(this.transform.parent.parent);
        newBullet.GetComponent<SpriteRenderer>().enabled = true;
        newBullet.GetComponent<CircleCollider2D>().enabled = true;
        newBullet.GetComponent<Rigidbody2D>().isKinematic = false;
        newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(Mathf.Cos(rad + offset) * BULLET_FORCE, Mathf.Sin(rad + offset) * BULLET_FORCE, 0));
    }
}
