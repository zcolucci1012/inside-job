using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Particle : MonoBehaviour
{
    private float PARTICLE_SPEED = 1f;
    private int TOTAL_LIFESPAN = 30;
    private int lifespan;
    private bool freeze = true;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        lifespan = TOTAL_LIFESPAN;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.freeze)
        {

        }
        else if (lifespan > 0)
        {
            this.transform.position = new Vector3(this.transform.position.x + PARTICLE_SPEED,
                this.transform.position.y,
                this.transform.position.z);
            text.color = new Color(text.color.r, text.color.g, text.color.b, (float)lifespan / (float)TOTAL_LIFESPAN);
            lifespan--;
        } else
        {
            Destroy(this.gameObject);
        }  
    }

    public void ToggleFreeze()
    {
        this.freeze = !this.freeze;
    }
}
