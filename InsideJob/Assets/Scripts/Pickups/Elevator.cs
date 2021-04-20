using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    public Sprite[] sprites;
    private bool running = false;
    private new GameObject camera;
    private int animTicks = 0;
    private SpriteRenderer spriteRenderer;

    protected GameObject player;
    protected GameObject eKey;
    protected GameObject cost;
    protected UIController ui;

    private void Awake()
    {
        player = GameObject.Find("Player");
        eKey = GameObject.Find("E");
        cost = GameObject.Find("Cost");
        ui = GameObject.Find("/Canvas").GetComponent<UIController>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.camera = GameObject.Find("Main Camera");
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Player" && !running)
        {
            cost.GetComponent<Text>().enabled = false;
            eKey.transform.parent.localPosition = new Vector3(0, 0, 0);
            eKey.GetComponent<Text>().enabled = true;
            eKey.transform.parent.gameObject.GetComponent<Image>().enabled = true;
            if (Input.GetKey("e"))
            {
                cost.GetComponent<Text>().enabled = false;
                eKey.GetComponent<Text>().enabled = false;
                eKey.transform.parent.gameObject.GetComponent<Image>().enabled = false;
                EffectOnPickup();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            cost.GetComponent<Text>().enabled = false;
            eKey.GetComponent<Text>().enabled = false;
            eKey.transform.parent.gameObject.GetComponent<Image>().enabled = false;
        }
    }

    protected void EffectOnPickup()
    {
        this.running = true;
        player.GetComponent<PlayerController>().CanMove(false);
        this.camera.GetComponent<CameraController>().SetInCutscene(true);
        this.camera.transform.position = this.transform.position + new Vector3(0, 0, -10f);
        foreach (Transform child in player.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (running)
        {
            if (animTicks < 40)
            {
                spriteRenderer.sprite = sprites[0];
            }
            else if (animTicks < 50)
            {
                spriteRenderer.sprite = sprites[1];
            }
            else if (animTicks < 100)
            {
                spriteRenderer.sprite = sprites[2];
            }
            else if (animTicks < 150)
            {
                this.player.transform.position = this.transform.position + new Vector3(0, 0, -0.25f);
                this.player.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            }
            else if (animTicks < 160)
            {
                spriteRenderer.sprite = sprites[1];
            }
            else if (animTicks < 205)
            {
                this.player.SetActive(false);
                spriteRenderer.sprite = sprites[0];
            }
            else if (animTicks < 400)
            {
                this.transform.parent.position += new Vector3(0, 0.05f, 0);
            }
            else 
            {
                SceneManager.LoadScene("Win");
            }
            animTicks++;
        }
    }
}
