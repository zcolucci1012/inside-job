using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public WeaponInventory inventory;
    private Transform image;
    private Slider slider;
    private Text ammo;

    // Start is called before the first frame update
    void Start()
    {
        this.image = this.transform.Find("Weapon Image");
        this.slider = this.transform.Find("Reload Bar").Find("Slider").GetComponent<Slider>();
        this.ammo = this.transform.Find("Ammo").Find("Ammo Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform activeWeapon = inventory.GetActiveWeapon();

        this.image.GetComponent<Image>().sprite = 
            activeWeapon.GetComponent<SpriteRenderer>().sprite;
        if (activeWeapon.GetComponent<Weapon>().GetNumBullets() <= 0)
        {
            slider.maxValue = activeWeapon.GetComponent<Weapon>().RELOAD_TIME;
            slider.value = activeWeapon.GetComponent<Weapon>().GetReloadTick();
        }
        else
        {
            slider.maxValue = activeWeapon.GetComponent<Weapon>().CLIP_SIZE;
            slider.value = activeWeapon.GetComponent<Weapon>().GetNumBullets();
        }

        this.ammo.text = "";
        int leadingZeros = activeWeapon.GetComponent<Weapon>().CLIP_SIZE.ToString().Length -
            activeWeapon.GetComponent<Weapon>().GetNumBullets().ToString().Length;
        for (int ii = 0; ii < leadingZeros; ii++)
        {
            this.ammo.text += "0";
        }
        this.ammo.text += activeWeapon.GetComponent<Weapon>().GetNumBullets() + "/"
            + activeWeapon.GetComponent<Weapon>().CLIP_SIZE;

        
        
    }
}
