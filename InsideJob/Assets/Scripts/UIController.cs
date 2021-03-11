using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Font[] fonts;
    public GameObject player;
    public Text healthText;
    public GameObject reticle;
    public GameObject particle;
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        foreach (Font font in fonts)
        {
            var mat = font.material;
            var texture = mat.mainTexture;
            texture.filterMode = FilterMode.Point;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        reticle.transform.position = Input.mousePosition;
        healthText.text = player.GetComponent<EntityWithHealth>().GetHealth().ToString("c2");
    }

    public void SpawnParticle(float val)
    {
        GameObject newParticle = Instantiate(particle, this.transform.GetChild(0), true);
        newParticle.GetComponent<Particle>().ToggleFreeze();
        string sign = val < 0 ? "-" : "+";
        newParticle.GetComponent<Text>().text = sign + "$" + Mathf.Abs(val);
        newParticle.GetComponent<Text>().enabled = true;
        newParticle.GetComponent<Text>().color = val < 0 ? Color.red : Color.green;
    }
}
