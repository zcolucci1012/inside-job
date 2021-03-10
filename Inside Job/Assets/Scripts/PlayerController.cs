using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float runSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        float xVel = 0;
        float yVel = 0;

        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            yVel += runSpeed;
        }
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            xVel -= runSpeed;
        }
        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            yVel -= runSpeed;
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            xVel += runSpeed;
        }

        rigidbody.velocity = new Vector2(xVel, yVel);

        //Debug.Log(this.transform.position.x + ", " + this.transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Bullet(Clone)")
        {
            Debug.Log("hmm");
            
        }
    }

}
