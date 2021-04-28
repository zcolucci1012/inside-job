using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponInventory : MonoBehaviour
{
    private List<Transform> weapons;
    private List<Transform> inventory;
    private Transform activeWeapon;

    // Start is called before the first frame update
    void Awake()
    {
        weapons = new List<Transform>();
        inventory = new List<Transform>();
        GameObject weaponPickups = GameObject.Find("Weapon Pickups");
        for (int ii = 0; ii < this.transform.childCount; ii++)
        {
            weapons.Add(this.transform.GetChild(ii));
            GameObject weaponPickup = Instantiate(GameObject.Find("Sample Weapon Pickup"), weaponPickups.transform, true);
            string name = this.transform.GetChild(ii).name;
            weaponPickup.name = name + " Pickup";
            weaponPickup.GetComponent<WeaponPickup>().SetWeaponName(name);
        }
        Destroy(GameObject.Find("Sample Weapon Pickup"));
        inventory.Add(this.transform.GetChild(0));
        this.activeWeapon = this.transform.GetChild(0);
        inventory[0].SetParent(GameObject.Find("Player").transform);
        inventory[0].GetComponent<Weapon>().SetActive(true);
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            AddWeapon("Buckshot");
        }
        AddWeapon("Pumpstock");
    }

    public void AddWeapon(string name)
    {
        if (inventory.Count < 3)
        {
            foreach (Transform weapon in this.weapons)
            {
                if (weapon.name == name && !inventory.Contains(weapon))
                {
                    inventory.Add(weapon);
                    break;
                }
            }
        } else
        {
            for (int ii = 0; ii < inventory.Count; ii++)
            {
                if (inventory[ii].Equals(this.activeWeapon))
                {
                    foreach (Transform weapon in this.weapons)
                    {
                        if (weapon.name == name && inventory.Contains(weapon))
                        {
                            inventory.Remove(activeWeapon);
                            inventory.Insert(ii, weapon);
                            this.activeWeapon = weapon;
                        }
                    }
                }
            }
        }
    }

    public void IncWeapon()
    {
        for (int ii = 0; ii < inventory.Count; ii++)
        {
            if (inventory[ii].Equals(this.activeWeapon))
            {
                this.activeWeapon = inventory[(ii + 1) % inventory.Count];
                break;
            }
        }
    }

    public void DecWeapon()
    {
        for (int ii = 0; ii < inventory.Count; ii++)
        {
            if (inventory[ii].Equals(this.activeWeapon))
            {
                this.activeWeapon = inventory[(ii + inventory.Count - 1) % inventory.Count];
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform weapon in weapons)
        {
            if (inventory.Contains(weapon))
            {
                GameObject player = GameObject.Find("Player");
                if (player != null)
                {
                    weapon.SetParent(player.transform);
                }
                if (weapon.Equals(this.activeWeapon))
                {
                    weapon.GetComponent<Weapon>().SetActive(true);
                } else
                {
                    weapon.GetComponent<Weapon>().SetActive(false);
                }
            }
            else
            {
                weapon.SetParent(this.transform);
            }
        }
    }

    public List<Transform> GetWeapons()
    {
        return this.weapons;
    }

    public List<Transform> GetInventory()
    {
        return this.inventory;
    }

    public Transform GetActiveWeapon()
    {
        return this.activeWeapon;
    }
}
