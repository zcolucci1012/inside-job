using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWithHealth : MonoBehaviour
{
    public float TOTAL_HEALTH;
    private float currentHealth;
    private bool flashGreen = false;
    private bool flashRed = false;
    private int FLASH_TICKS = 10;
    private int flashTick = -1;

    // Start is called before the first frame update
    protected void Start()
    {
        currentHealth = TOTAL_HEALTH;
    }
    // Update is called once per frame
    protected void FixedUpdate()
    {
        Color newColor = new Color(0, 0, 0);
        if (flashGreen)
        {
            newColor = GetColor(flashTick, FLASH_TICKS, 200, 0, 0);
        }
        if (flashRed)
        {
            newColor = GetColor(flashTick, FLASH_TICKS, 200, 0, 0);
        }
        if (flashTick >= 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = newColor;
            flashTick--;
        } else
        {
            flashGreen = false;
            flashRed = false;
        }
    }

    Color GetColor(int tick, int max, int r, int g, int b)
    {
        float rf = (((255 - r) / max) * Mathf.Abs(tick - max) + r) / 255f;
        float gf = (((255 - g) / max) * Mathf.Abs(tick - max) + g) / 255f;
        float bf = (((255 - b) / max) * Mathf.Abs(tick - max) + b) / 255f;
        return new Color(rf, gf, bf);
    }

    public void AddHealth(int health)
    {
        flashRed = health < 0;
        flashGreen = health > 0;
        flashTick = FLASH_TICKS;
        
        this.currentHealth += health;
        if (this.currentHealth > TOTAL_HEALTH)
        {
            this.currentHealth = TOTAL_HEALTH;
        }
        else if (this.currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public float GetHealth()
    {
        return this.currentHealth;
    }
}
