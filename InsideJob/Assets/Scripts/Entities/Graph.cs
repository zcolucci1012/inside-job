using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : Bullet
{
    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12
            || collision.gameObject.layer == 13)
        {
            Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(),
                collision.gameObject.GetComponent<Collider2D>());
        } else
        {
            base.OnCollisionEnter2D(collision);
        }
    }
}
