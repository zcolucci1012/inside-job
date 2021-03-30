using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkToSurvey : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToSurvey()
    {
        Application.OpenURL("https://forms.gle/z358d1tTugqHCpRV6");
    }
}
