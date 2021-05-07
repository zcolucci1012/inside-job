using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTables : MonoBehaviour
{

    public static GameObject FileCabinet()
    {
        int r = Random.Range(0, 100);
        if (r < 95)
        {
            return null;
        } else if (r < 99)
        {
            return Free(Instantiate(GameObject.Find("Check")));
        } else
        {
            return Free(Weapon());
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
            weapon.GetComponent<WeaponPickup>().SetWeaponName("Buckshot", false);
        } else
        {
            weapon = Instantiate(GameObject.Find("Pumpstock Pickup"), null);
            weapon.GetComponent<WeaponPickup>().SetWeaponName("Pumpstock", false);
        }
        return weapon;
    }

    public static GameObject Pickup()
    {
        int r = Random.Range(0, 2);
        return Weapon();
    }

    public static GameObject FreePickup()
    {
        return Free(Pickup());
    }

    private static GameObject Free(GameObject obj)
    {
        obj.GetComponent<Pickup>().SetCost(0);
        return obj;
    }
}
