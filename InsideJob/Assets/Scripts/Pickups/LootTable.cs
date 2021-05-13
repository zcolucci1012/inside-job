using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTables : MonoBehaviour
{
    private static string[] passives = new string[]
    {
        "Running Shoes",
        "Raise",
        "Employee Discount",
        "The Boss's Coffee",
        "Bag of White Stuff",
        "Orange Suit",
        "Blueprints",
        "The Miracle Crowbar"
    };

    public static GameObject FileCabinet()
    {
        int r = Random.Range(0, 100);
        if (r < 90)
        {
            return null;
        }
        else if (r < 92)
        {
            return Instantiate(GameObject.Find("Cash25"));
        }
        else if (r < 95)
        {
            return Instantiate(GameObject.Find("Cash10"));
        }
        else if (r < 99)
        {
            return Free(Instantiate(GameObject.Find("Check")));
        }
        else
        {
            return Free(Pickup());
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

    public static GameObject Passive()
    {
        GameObject passive;
        int r = Random.Range(0, passives.Length);
        passive = Instantiate(GameObject.Find(passives[r]));
        return passive;
    }

    public static GameObject EnemyDrop()
    {
        GameObject cash;
        int r = Random.Range(0, 20);
        if (r <= 1)
        {
            cash = Instantiate(GameObject.Find("Cash10"));
        } else if (r <= 2)
        {
            cash = Instantiate(GameObject.Find("Cash25"));
        }
        else
        {
            cash = null;
        }
        return cash;
    }

    public static GameObject Pickup()
    {
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            return Weapon();
        }
        else
        {
            return Passive();
        }
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
