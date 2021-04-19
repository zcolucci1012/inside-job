using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMoney : MonoBehaviour
{
    public Sprite[] sprites;
    private int lifeTicks = 0;
    // Start is called before the first frame update
    void Awake()
    {
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    private void FixedUpdate()
    {
        lifeTicks++;
        if (lifeTicks > 300)
        {
            Destroy(this.gameObject);
        }
    }
}
