using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Particle : MonoBehaviour
{
    public float PARTICLE_SPEED = 1f;
    private int TOTAL_LIFESPAN = 50;
    private int lifespan;
    private float offset = 0;
    private bool freeze = true;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        lifespan = TOTAL_LIFESPAN;
        offset = Random.Range(-0.4f, 0.4f);
        PARTICLE_SPEED += Random.Range(-0.5f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.freeze)
        {
            return;
        }
        else if (lifespan > 0)
        {
            this.transform.position = new Vector3(this.transform.position.x + PARTICLE_SPEED,
                this.transform.position.y + offset,
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
