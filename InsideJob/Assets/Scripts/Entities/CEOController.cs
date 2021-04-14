using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEOController : EnemyController
{
    public GameObject table;
    private bool attacking = false;
    private int attackTick = 0;
    private int r = -1;
    public float TABLE_FORCE = 300f;
    private bool cutscene = true;
    private int cutsceneTicks = 0;
    private GameObject camera;

    new void Start()
    {
        base.Start();
        this.camera = GameObject.Find("Main Camera");
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (awake && cutscene)
        {
            cutsceneTicks++;
            if (cutsceneTicks < 200)
            {
                camera.transform.position = this.transform.position -
                    (this.transform.position - playerTransform.position) * (1 - (float)cutsceneTicks / 200f);
                camera.transform.position = new Vector3(camera.transform.position.x,
                    camera.transform.position.y,
                    -10);
                camera.GetComponent<CameraController>().SetInCutscene(true);
                playerTransform.gameObject.GetComponent<PlayerController>().CanMove(false);
            }
            else if (cutsceneTicks > 300)
            {
                cutscene = false;
                camera.GetComponent<CameraController>().SetInCutscene(false);
                playerTransform.gameObject.GetComponent<PlayerController>().CanMove(true);
            }
        }
        if (awake && !cutscene)
        {
            if (!attacking)
            {
                r = Random.Range(0, 1);
                if (r == 0)
                {
                    GameObject newTable1 = Instantiate(table, this.transform, true);
                    GameObject newTable2 = Instantiate(table, this.transform, true);
                    GameObject newTable3 = Instantiate(table, this.transform, true);

                    newTable1.transform.localPosition = new Vector3(7, -2, -0.25f);
                    newTable2.transform.localPosition = new Vector3(-7, -5, -0.25f);
                    newTable3.transform.localPosition = new Vector3(7, -8, -0.25f);

                    newTable1.GetComponent<Rigidbody2D>().AddForce(new Vector3(-TABLE_FORCE, 0, 0));
                    newTable2.GetComponent<Rigidbody2D>().AddForce(new Vector3(TABLE_FORCE, 0, 0));
                    newTable3.GetComponent<Rigidbody2D>().AddForce(new Vector3(-TABLE_FORCE, 0, 0));
                }
                attacking = true;
            }
            if (attacking)
            {
                if (r == 0)
                {
                    if (attackTick > 200)
                    {
                        attackTick = 0;
                        attacking = false;
                    }
                }
                attackTick++;
            }
        }
    }

}
