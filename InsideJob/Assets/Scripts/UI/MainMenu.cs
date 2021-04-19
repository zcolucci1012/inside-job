using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }

    public void Menu()
    {
        if (GameObject.Find("Music"))
        {
            GameObject.Find("Music").GetComponent<BackgroundMusic>().StopMusic();
        }
        SceneManager.LoadScene("MainMenu");
    }
}
