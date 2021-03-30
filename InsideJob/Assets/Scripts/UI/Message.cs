using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public float PARTICLE_SPEED = 0.5f;
    private int TOTAL_LIFESPAN = 100;
    private int lifespan;
    private bool freeze = true;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        lifespan = TOTAL_LIFESPAN;
        text = this.GetComponent<Text>();
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
            this.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y + PARTICLE_SPEED,
                this.transform.position.z);
            text.color = new Color(text.color.r, text.color.g, text.color.b, (float)lifespan / (float)TOTAL_LIFESPAN);
            lifespan--;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ToggleFreeze()
    {
        this.freeze = !this.freeze;
    }
}
