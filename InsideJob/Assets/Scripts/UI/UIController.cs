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
    public GameObject message;
    public GameObject minimap;
    public GameObject bossHealth;
    public GameObject bossName;
    public GameObject pause;

    private bool paused = false;
    

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
        if (Input.GetKey("space"))
        {
            minimap.SetActive(true);
        } else
        {
            minimap.SetActive(false);
        }
        reticle.transform.position = Input.mousePosition;
        healthText.text = player.GetComponent<EntityWithHealth>().GetHealth().ToString("c2");

        if (Input.GetKeyDown("escape"))
        {
            paused = !paused;
            Time.timeScale = paused ? 0 : 1;
            pause.SetActive(paused);
            Weapon[] weapons = Resources.FindObjectsOfTypeAll<Weapon>();
            foreach (Weapon weapon in weapons)
            {
                weapon.enabled = !paused;
            }
            //AudioListener.volume = paused ? 0.25f : 1;
            Cursor.visible = paused;
        }
    }

    public void SpawnParticle(float val)
    {
        GameObject newParticle = Instantiate(particle, this.transform.GetChild(0), true);
        newParticle.GetComponent<Particle>().ToggleFreeze();
        string sign = val < 0 ? "-" : "+";
        newParticle.GetComponent<Text>().text = sign + "$" + Mathf.Abs(val);
        newParticle.GetComponent<Text>().enabled = true;
        newParticle.GetComponent<Text>().color = val < 0 ? Color.red : new Color(0f, 0.7f, 0f);
    }

    public void SpawnPickupMessage(string msg)
    {
        GameObject newMessage = Instantiate(message, this.transform.GetChild(0), true);
        newMessage.GetComponent<Message>().ToggleFreeze();
        newMessage.GetComponent<Text>().text = msg;
        newMessage.GetComponent<Text>().enabled = true; 
    }

    public void SetBossHealth(float health, float total, string name)
    {
        bossHealth.SetActive(true);
        bossHealth.GetComponent<Slider>().maxValue = total;
        bossHealth.GetComponent<Slider>().value = health;
        bossName.GetComponent<Text>().text = name;
    }

    public void DisableBossHealth()
    {
        bossHealth.SetActive(false);
    }
}
