using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public BackgroundMusic music;

    public void AdjLeft()
    {
        music.AdjVolume(-0.05f);
    }

    public void AdjRight()
    {
        music.AdjVolume(0.05f);
    }

    private void Update()
    {
        this.GetComponent<Slider>().value = music.GetVolume();
    }
}
