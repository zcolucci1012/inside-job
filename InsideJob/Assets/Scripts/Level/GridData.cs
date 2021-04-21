using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridData : MonoBehaviour
{
    public static Dictionary<int[], string> grid = new Dictionary<int[], string>(new SamePos());

    public static void PrintGrid()
    {

        foreach (int[] key in grid.Keys)
        {
            if (grid[key] != "")
            {
                print(key[0] + ", " + key[1] + ": " + grid[key]);
            }
        }
    }
}

class SamePos : EqualityComparer<int[]>
{
    public override bool Equals(int[] x, int[] y)
    {
        if (x == null && y == null)
        {
            return true;
        }
        else if (x == null || y == null)
        {
            return false;
        }

        return (x[0] == y[0]
            && x[1] == y[1]);
    }

    public override int GetHashCode(int[] obj)
    {
        int code = (17 * obj[0] ^ 41 * obj[1]);
        return code;
    }
}
