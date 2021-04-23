using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEOController : EnemyController
{
    public GameObject spawner;
    public GameObject table;
    public GameObject stapler;
    public GameObject elevator;
    private bool attacking = false;
    private int attackTick = 0;
    private int r = -1;
    public float TABLE_FORCE = 400f;
    private bool cutscene = true;
    private int cutsceneTicks = 0;
    private new GameObject camera;
    private Vector3[] spawnLocations = new Vector3[4] { new Vector3(-5f, -1f, 0f),
        new Vector3(5f, -1f, 0f), new Vector3(-5f, -6f, 0f), new Vector3(5, -6f, 0f) };
    private Vector3 roomCenter;
    private UIController ui;

    new void Awake()
    {
        base.Awake();
        this.camera = GameObject.Find("Main Camera");
        roomCenter = new Vector3(0, 0, 0);
        
        this.ui = GameObject.Find("/Canvas").GetComponent<UIController>();
    }

    void Start()
    {
        int rx = ((int)(this.transform.position.x + Constants.ROOM_WIDTH / 2) / Constants.ROOM_WIDTH);
        int ry = ((int)(this.transform.position.y + Constants.ROOM_HEIGHT / 2) / Constants.ROOM_HEIGHT);
        if (this.transform.position.x + Constants.ROOM_WIDTH / 2 < 0)
        {
            rx--;
        }
        if (this.transform.position.y + Constants.ROOM_HEIGHT / 2 < 0)
        {
            ry--;
        }
        roomCenter.x = rx * Constants.ROOM_WIDTH;
        roomCenter.y = ry * Constants.ROOM_HEIGHT;
        spawnLocations = new Vector3[4] { new Vector3(-5f, -5f, 0f) + roomCenter,
            new Vector3(5f, -5f, 0f) + roomCenter, new Vector3(-5f, 5f, 0f) + roomCenter, new Vector3(5, 5f, 0f) + roomCenter };
    }

    new void Update()
    {
        base.Update();
        if (awake)
        {
            ui.SetBossHealth(this.currentHealth, this.TOTAL_HEALTH, "CEO");
        }
    }

    protected override void End()
    {
        ui.DisableBossHealth();
        GameObject newElevator = Instantiate(elevator);
        newElevator.transform.position = this.roomCenter;
        base.End();
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
            else if (cutsceneTicks == 300)
            {
                camera.GetComponent<CameraController>().SetInCutscene(false);
                playerTransform.gameObject.GetComponent<PlayerController>().CanMove(true);
            } else if (cutsceneTicks > 400)
            {
                cutscene = false;
            }
        }
        if (awake && !cutscene)
        {
            if (!attacking)
            {
                r = Random.Range(0, 3);
                if (r == 0)
                {
                    System.Action<GameObject> goLeft = (o) =>
                    {
                        o.GetComponent<Rigidbody2D>().isKinematic = false;
                        o.GetComponent<Rigidbody2D>().AddForce(new Vector3(-TABLE_FORCE, 0, 0));
                    };
                    System.Action<GameObject> goRight = (o) => {
                        o.GetComponent<Rigidbody2D>().isKinematic = false;
                        o.GetComponent<Rigidbody2D>().AddForce(new Vector3(TABLE_FORCE, 0, 0));
                    };
                    GameObject spawnTable1 = Instantiate(spawner);
                    spawnTable1.GetComponent<Spawner>().Spawn(table,
                        new Vector3(5f, 2f, -0.25f) + roomCenter, goLeft);

                    GameObject spawnTable2 = Instantiate(spawner);
                    spawnTable2.GetComponent<Spawner>().Spawn(table,
                        new Vector3(-5f, 0f, -0.25f) + roomCenter, goRight);

                    GameObject spawnTable3 = Instantiate(spawner);
                    spawnTable3.GetComponent<Spawner>().Spawn(table,
                        new Vector3(5f, -2f, -0.25f) + roomCenter, goLeft);
                }
                if (r == 1)
                {
                    int rPos = Random.Range(0, 4);
                    GameObject spawnStapler = Instantiate(spawner);
                    spawnStapler.GetComponent<Spawner>().Spawn(stapler,
                        spawnLocations[rPos]);
                }
                if (r == 2)
                {
                    System.Action<GameObject> flip = (o) =>
                    {
                        o.transform.eulerAngles = new Vector3(0, 0, 90);
                    };

                    System.Action<GameObject> goDown = (o) =>
                    {
                        o.GetComponent<Rigidbody2D>().isKinematic = false;
                        o.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, -TABLE_FORCE, 0));
                    };

                    GameObject spawnTable1 = Instantiate(spawner);
                    spawnTable1.GetComponent<Spawner>().Spawn(table,
                        new Vector3(4f, 4f, -0.25f) + roomCenter, goDown, flip);

                    GameObject spawnTable2 = Instantiate(spawner);
                    spawnTable2.GetComponent<Spawner>().Spawn(table,
                        new Vector3(-4f, 4f, -0.25f) + roomCenter, goDown, flip);

                    GameObject spawnTable3 = Instantiate(spawner);
                    spawnTable3.GetComponent<Spawner>().Spawn(table,
                        new Vector3(0f, 1.5f, -0.25f) + roomCenter, goDown, flip);
                }
                attacking = true;
            }
            if (attacking)
            {
                if (r == 0)
                {
                    if (attackTick > 150)
                    {
                        attackTick = 0;
                        attacking = false;
                        this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                    }
                    if (attackTick > 100)
                    {
                        this.GetComponent<SpriteRenderer>().sprite = sprites[1];
                    }
                }
                if (r == 1)
                {
                    if (attackTick > 150)
                    {
                        attackTick = 0;
                        attacking = false;
                        this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                    }
                    if (attackTick > 100)
                    {
                        this.GetComponent<SpriteRenderer>().sprite = sprites[1];
                    }
                }
                if (r == 2)
                {
                    if (attackTick > 150)
                    {
                        attackTick = 0;
                        attacking = false;
                        this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                    }
                    if (attackTick > 100)
                    {
                        this.GetComponent<SpriteRenderer>().sprite = sprites[1];
                    }
                }
                attackTick++;
            }
        }
    }

}
