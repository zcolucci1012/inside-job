using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    private SpriteRenderer spriteRenderer;
    private WeaponInventory weaponInventory;
    private string weaponName = "";

    protected override void EffectOnPickup()
    {
        weaponInventory.AddWeapon(this.weaponName);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (this.weaponName == "")
        {
            return;
        }
        this.weaponInventory = GameObject.Find("Weapons").GetComponent<WeaponInventory>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
        List<Transform> weapons = weaponInventory.GetWeapons();
        if (this.weaponName == "random")
        {
            int r = Random.Range(0, weapons.Count);
            this.weaponName = weapons[r].name;
            this.spriteRenderer.sprite = weapons[r].gameObject.GetComponent<SpriteRenderer>().sprite;
            return;
        } else if (this.weaponName != "")
        {
            foreach (Transform weapon in weapons)
            {
                if (weapon.name == weaponName)
                {
                    this.spriteRenderer.sprite = weapon.gameObject.GetComponent<SpriteRenderer>().sprite;
                    return;
                }
            }
        }
        print("name not recognized");   
    }

    public void SetWeaponName(string weaponName)
    {
        this.weaponName = weaponName;
    }

    public string GetWeaponName()
    {
        return this.weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.weaponName == "")
        {
            Start();
        }
    }
}
