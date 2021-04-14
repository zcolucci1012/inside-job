using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    private bool inCutscene = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = this.GetComponent<Camera>();
        float size = camera.orthographicSize;
        if (!inCutscene)
        {
            this.transform.position = new Vector3(playerTransform.position.x,
            playerTransform.position.y,
            this.transform.position.z);
        }
        
    }

    public void SetInCutscene(bool inCutscene)
    {
        this.inCutscene = inCutscene;
    }

}
