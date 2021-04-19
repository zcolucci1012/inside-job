using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private GameObject[] others;
    private bool notFirst = false;
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
