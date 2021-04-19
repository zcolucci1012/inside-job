using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void Play()
    {
        GameObject.Find("MenuMusic").GetComponent<MenuMusic>().StopMusic();
        if (GameObject.Find("Music"))
        {
            GameObject.Find("Music").GetComponent<BackgroundMusic>().PlayMusic();
        }
        SceneManager.LoadScene("MainScene");
    }
}
