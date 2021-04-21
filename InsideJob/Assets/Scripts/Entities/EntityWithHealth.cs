using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityWithHealth : MonoBehaviour
{
    public float TOTAL_HEALTH;
    public bool FLASHES = true;
    protected float currentHealth;
    private bool flashGreen = false;
    private bool flashRed = false;
    private int FLASH_TICKS = 10;
    private int flashTick = -1;
    private bool ended = false;
    protected int[] currCell;
    private int[] prevCell = null;

    protected virtual void Awake()
    {
        currentHealth = TOTAL_HEALTH;
        currCell = new int[2] { (int)this.transform.position.x, (int)this.transform.position.y };
        GridData.grid[currCell] = this.gameObject.name;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (FLASHES)
        {
            Color newColor = new Color(255, 255, 255);
            if (flashGreen)
            {
                newColor = GetColor(flashTick, FLASH_TICKS, 0, 200, 0);
            }
            if (flashRed)
            {
                newColor = GetColor(flashTick, FLASH_TICKS, 200, 0, 0);
            }
            if (flashTick >= 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = newColor;
                flashTick--;
            }
            else
            {
                flashGreen = false;
                flashRed = false;
            }
        }
    }

    protected void Update()
    {
        currCell = new int[2] { (int)this.transform.position.x, (int)this.transform.position.y };
        if (transform.position.x < 0)
        {
            currCell[0]--;
        }
        if (transform.position.y < 0)
        {
            currCell[1]--;
        }


        if (prevCell == null || !prevCell.Equals(currCell))
        {
            if (prevCell != null)
            {
                GridData.grid[prevCell] = "";
            }
            GridData.grid[currCell] = this.gameObject.name;
        }

        prevCell = currCell;
    }

    Color GetColor(int tick, int max, int r, int g, int b)
    {
        float rf = (((255 - r) / max) * Mathf.Abs(tick - max) + r) / 255f;
        float gf = (((255 - g) / max) * Mathf.Abs(tick - max) + g) / 255f;
        float bf = (((255 - b) / max) * Mathf.Abs(tick - max) + b) / 255f;
        return new Color(rf, gf, bf);
    }

    public virtual void AddHealth(float health)
    {
        if (FLASHES)
        {
            flashRed = health <= -5f;
            flashGreen = health >= 5f;
            if (Mathf.Abs(health) >= 5f)
            {

                flashTick = FLASH_TICKS;
            }
        }

        this.currentHealth += health;
        if (this.currentHealth <= 0f && this.ended == false)
        {
            this.ended = true;
            End();
        }
    }

    protected abstract void End();

    public float GetHealth()
    {
        return this.currentHealth;
    }

    public int[] GetCurrCell()
    {
        return this.currCell;
    }
}
