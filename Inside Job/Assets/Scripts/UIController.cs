using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject player;
    public Text health;
    public GameObject reticle;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        reticle.transform.position = Input.mousePosition;
        health.text = player.GetComponent<EntityWithHealth>().GetHealth().ToString("c2");
    }
}
