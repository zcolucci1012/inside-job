using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int[] LAYERS_TO_HIT;
    public int DAMAGE;
    protected bool canHit = false;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(8, 14);
        Physics2D.IgnoreLayerCollision(9, 14);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.canHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canHit)
        {
            for (int ii = 0; ii < LAYERS_TO_HIT.Length; ii++)
            {
                if (collision.collider.gameObject.layer == LAYERS_TO_HIT[ii] && canHit)
                {
                    EntityWithHealth entity = collision.collider.gameObject.GetComponent<EntityWithHealth>();
                    entity.AddHealth(-DAMAGE);
                }
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (canHit)
        {
            for (int ii = 0; ii < LAYERS_TO_HIT.Length; ii++)
            {
                if (collider.gameObject.layer == LAYERS_TO_HIT[ii])
                {
                    EntityWithHealth entity = collider.gameObject.GetComponent<EntityWithHealth>();
                    entity.AddHealth(-DAMAGE);
                }
            }
            Destroy(this.gameObject);
        }
        
    }
}
