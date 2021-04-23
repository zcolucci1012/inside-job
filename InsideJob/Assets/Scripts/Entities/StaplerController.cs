using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaplerController : EnemyController
{
    public float SPEED = 5f;
    public float DAMAGE = 30;
    public AudioClip sound;
    private bool bite = false;
    private int biteTicks = 0;
    private int BITE_TICKS = 15;
    private int cooldownTicks = 0;
    private int COOLDOWN_TICKS = 30;
    private int idleTicks = 0;
    private int IDLE_TICKS = 120;
    private bool colliding = false;

    protected override void End()
    {
        this.playerTransform.GetComponent<PlayerController>().CanMove(true);
        base.End();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!bite && awake)
        {
            SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
            if (idleTicks > (7 * IDLE_TICKS) / 8)
            {
                renderer.sprite = sprites[1];
            } else
            {
                renderer.sprite = sprites[0];
            }
            renderer.flipX = ex > 0;

            float cos = this.ex / this.d;
            float sin = this.ey / this.d;

            this.GetComponent<Rigidbody2D>().velocity = GetVelocity();
            //new Vector2(SPEED * cos, SPEED * sin);

            
        } else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (cooldownTicks > 0)
        {
            cooldownTicks--;
        }
        if (bite)
        {
            if (biteTicks < BITE_TICKS / 6)
            {
                this.GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
            else if (biteTicks < BITE_TICKS - BITE_TICKS / 6)
            {
                this.GetComponent<SpriteRenderer>().sprite = sprites[2];
            } else if (biteTicks < BITE_TICKS - 1)
            {
                this.GetComponent<SpriteRenderer>().sprite = sprites[1];
            } else
            {
                this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                PlayerController player = this.playerTransform.gameObject.GetComponent<PlayerController>();
                player.CanMove(true);
                cooldownTicks = COOLDOWN_TICKS;
                bite = false;
                biteTicks = 0;
            }
            biteTicks++;
        } else
        {
            if (idleTicks == IDLE_TICKS)
            {
                idleTicks = 0;
            }
            idleTicks++;
        }
        
    }

    

    private Vector2 GetVelocity()
    {
        FastPriorityQueue<Node> pq = new FastPriorityQueue<Node>(500);
        Node start = new Node(this.currCell, "Stapler(Clone)");
        //print("start: " + this.currCell[0] + ", " + this.currCell[1]);
        Node goal = new Node(playerTransform.gameObject.GetComponent<EntityWithHealth>().GetCurrCell(), "Player");
        if (start.gridPos[0] == goal.gridPos[0]
            && start.gridPos[1] == goal.gridPos[1])
        {
            Vector2 dir = ((this.playerTransform.position - this.transform.position).normalized) * SPEED;
            return dir;
        }
        pq.Enqueue(start, 0f);
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>(new SameNode());
        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>(new SameNode());
        cameFrom[start] = null;
        costSoFar[start] = 0;

        while (pq.Count != 0)
        {
            Node current = pq.Dequeue();

            if (current.gridPos[0] == goal.gridPos[0]
                && current.gridPos[1] == goal.gridPos[1])
            {
                break;
            }

            foreach (Node next in Neighbors(current))
            {
                if (next.name != "" && next.name != null && next.name != "Player")
                {
                    continue;
                }
                //print(next.gridPos[0] + ", " + next.gridPos[1]);
                float newCost = costSoFar[current] + 1;
                if (!costSoFar.ContainsKey(next)
                    || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    float priority = newCost + Vector2.Distance(this.playerTransform.position,
                        new Vector2(next.gridPos[0], next.gridPos[1]));
                    //print("queing " + next.gridPos[0] + ", " + next.gridPos[1] + ", " + next.name + ": " + priority);
                    pq.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        //Destroy(this.gameObject);

        Node curr = goal;
        try
        {
            while (cameFrom[curr] != start)
            {
                curr = cameFrom[curr];
            }
        } catch (KeyNotFoundException)
        {
            return new Vector2(0, 0);
        }
        

        int[] direction = new int[2] { curr.gridPos[0] - start.gridPos[0],
            curr.gridPos[1] - start.gridPos[1] };
        Vector2 velocity = new Vector2(0, 0) ;
        if (!colliding)
        {
            velocity = new Vector2(direction[0] * SPEED, direction[1] * SPEED);
        } else
        {
            Vector2 center = new Vector2(this.currCell[0] + 0.5f, this.currCell[1] + 0.5f);
            if (direction[0] == 0)
            {
                float sign = Mathf.Sign(center.x - this.transform.position.x);
                velocity = new Vector2(sign * SPEED, 0);
            } else if (direction[1] == 0)
            {
                float sign = Mathf.Sign(center.y - this.transform.position.y);
                velocity = new Vector2(0, sign * SPEED);
            }
        }
        

        
        return velocity;
    }

    private Node[] Neighbors(Node node)
    {
        Node[] neighbors = new Node[4];
        int[] up = new int[2] { node.gridPos[0], node.gridPos[1] + 1 };
        string upName;
        if (!GridData.grid.TryGetValue(up, out upName))
        {
            upName = "";
        }
        neighbors[0] = new Node(up, upName);

        int[] right = new int[2] { node.gridPos[0] + 1, node.gridPos[1] };
        string rightName;
        if (!GridData.grid.TryGetValue(right, out rightName))
        {
            rightName = "";
        }
        neighbors[1] = new Node(right, rightName);

        int[] down = new int[2] { node.gridPos[0], node.gridPos[1] - 1 };
        string downName;
        if (!GridData.grid.TryGetValue(down, out downName))
        {
            downName = "";
        }
        neighbors[2] = new Node(down, downName);

        int[] left = new int[2] { node.gridPos[0] - 1, node.gridPos[1] };
        string leftName;
        if (!GridData.grid.TryGetValue(left, out leftName))
        {
            leftName = "";
        }
        neighbors[3] = new Node(left, leftName);

        return neighbors;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            if (!bite && cooldownTicks == 0)
            {
                this.bite = true;
                PlayerController player = collider.gameObject.GetComponent<PlayerController>();
                player.AddHealth(-DAMAGE);
                player.CanMove(false);
                AudioSource.PlayClipAtPoint(sound, this.transform.position);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            this.colliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            this.colliding = false;
        }
    }

}

class SameNode : EqualityComparer<Node>
{
    public override bool Equals(Node x, Node y)
    {
        if (x == null && y == null)
        {
            return true;
        } else if (x == null || y == null)
        {
            return false;
        }

        return (x.gridPos[0] == y.gridPos[0]
            && x.gridPos[1] == y.gridPos[1]
            && x.name == y.name);
    }

    public override int GetHashCode(Node obj)
    {
        int code = (17 * obj.gridPos[0] ^ 41 * obj.gridPos[1]);
        if (obj.name != null)
        {
            code += obj.name.GetHashCode();
        }
        return code;
    }
}


