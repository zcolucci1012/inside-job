using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    public void Tutorial()
    {
        GameObject.Find("MenuMusic").GetComponent<MenuMusic>().StopMusic();
        SceneManager.LoadScene("Tutorial");
    }
}
