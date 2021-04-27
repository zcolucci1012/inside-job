using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    private bool inCutscene = false;
    public float cameraSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = playerTransform.position;
    }

    void FixedUpdate()
    {
        if (!inCutscene)
        {
            Vector3 finalPosition = playerTransform.position;
            finalPosition.z = -10;
            Vector3 lerpPosition = Vector3.Lerp(this.transform.position, finalPosition, cameraSpeed);
            this.transform.position = lerpPosition;
        }
    }

    public void SetInCutscene(bool inCutscene)
    {
        this.inCutscene = inCutscene;
    }

}
