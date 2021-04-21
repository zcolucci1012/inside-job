using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTables : MonoBehaviour
{

    public static GameObject FileCabinet()
    {
        int r = Random.Range(0, 100);
        if (r < 75)
        {
            return null;
        } else if (r < 95)
        {
            return Instantiate(GameObject.Find("Check"));
        } else
        {
            return Weapon();
        }
    }

    public static int CheckValues()
    {
        int r = Random.Range(0, 20);
        if (r < 10)
        {
            return 20;
        }
        else if (r < 13)
        {
            return 50;
        } else if (r < 16)
        {
            return 10;
        } else if (r < 19)
        {
            return 5;
        } else
        {
            return 1;
        }
    }

    public static GameObject Weapon()
    {
        GameObject weapon;
        int r = Random.Range(0, 100);
        if (r < 50)
        {
            weapon = Instantiate(GameObject.Find("Buckshot Pickup"), null);
            weapon.GetComponent<WeaponPickup>().SetWeaponName("Buckshot");
        } else
        {
            weapon = Instantiate(GameObject.Find("Pumpstock Pickup"), null);
            weapon.GetComponent<WeaponPickup>().SetWeaponName("Pumpstock");
        }
        return weapon;
    }

    public static GameObject Pickup()
    {
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            GameObject check = Instantiate(GameObject.Find("Check"));
            check.GetComponent<Pickup>().SetCost(20);
            return check;
        } else
        {
            GameObject weapon = Weapon();
            weapon.GetComponent<WeaponPickup>().SetCost(50);
            return weapon;
        }
    }
}
