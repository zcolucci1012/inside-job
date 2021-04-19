using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private GameObject[] others;
    private bool notFirst = false;

    private void Awake()
    {
        others = GameObject.FindGameObjectsWithTag("Music");

        foreach (GameObject other in others)
        {
            if (other.scene.buildIndex == -1)
            {
                notFirst = true;
            }
        }
        if (notFirst)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
