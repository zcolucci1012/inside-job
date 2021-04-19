using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMoneyController : MonoBehaviour
{
    int spawnTicks = 0;
    public GameObject menuMoney;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.spawnTicks > 5)
        {
            GameObject newMenuMoney = Instantiate(menuMoney);
            bool fromLeft = Random.Range(0, 2) == 1;
            if (fromLeft)
            {
                newMenuMoney.transform.position = new Vector3(-7f, Random.Range(-5f, 5f), 0);
            }
            else
            {
                newMenuMoney.transform.position = new Vector3(7f, Random.Range(-5f, 5f), 0);
            }
            Rigidbody2D rb = newMenuMoney.GetComponent<Rigidbody2D>();
            rb.AddForce(fromLeft ? new Vector3(150f, 0, 0) : new Vector3(-150f, 0, 0));
            newMenuMoney.transform.eulerAngles = new Vector3(0, 0, Random.Range(-90f, 90f));
            this.spawnTicks = 0;
        }
        spawnTicks++;
    }
}
