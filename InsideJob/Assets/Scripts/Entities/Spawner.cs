using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    private GameObject obj;
    private GameObject newObj;
    private Vector3 location;
    private Action<GameObject> onActive = null;
    private Action<GameObject> onSpawn = null;
    bool spawning = false;
    int spawnTicks = 0;
    MonoBehaviour[] scripts;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.spawning)
        {
            if (spawnTicks > 30)
            {
                foreach (MonoBehaviour script in scripts)
                {
                    if (script != null)
                    {
                        script.enabled = true;
                    }
                }
                if (onActive != null && newObj != null)
                {
                    onActive.Invoke(newObj);
                }
                Destroy(this.gameObject);
            }
            spawnTicks++;
        }
    }

    public void Spawn(GameObject obj, Vector3 location, Action<GameObject> onActive, Action<GameObject> onSpawn)
    {
        this.obj = obj;
        this.location = location;
        this.onActive = onActive;
        this.onSpawn = onSpawn;
        this.spawning = true;

        newObj = Instantiate(obj);
        newObj.transform.position = this.location;
        this.scripts = newObj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
        if (this.onSpawn != null && newObj != null)
        {
            onSpawn.Invoke(newObj);
        }
    }

    public void Spawn(GameObject obj, Vector3 location, Action<GameObject> onActive)
    {
        Spawn(obj, location, onActive, null);
    }

    public void Spawn(GameObject obj, Vector3 location)
    {
        Spawn(obj, location, null, null);
    }
}
