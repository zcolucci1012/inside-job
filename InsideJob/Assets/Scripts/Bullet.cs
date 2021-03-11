using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public int LAYER_TO_HIT;
    public int DAMAGE;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LAYER_TO_HIT)
        {
            EntityWithHealth entity = collision.collider.gameObject.GetComponent<EntityWithHealth>();
            entity.AddHealth(-DAMAGE);
        }
        Destroy(this.gameObject);
    }
}
