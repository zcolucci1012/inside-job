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
        string msg = "You picked up a " + this.weaponName + "!";
        if (weaponInventory.GetInventory().Count == 1)
        {
            msg += "\n(Shift or scroll to switch weapons)";
        }
        weaponInventory.AddWeapon(this.weaponName);
        ui.SpawnPickupMessage(msg);
    }


    public void SetWeaponName(string weaponName)
    {
        this.weaponName = weaponName;
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
        }
        else if (this.weaponName != "")
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
    }

    public string GetWeaponName()
    {
        return this.weaponName;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
