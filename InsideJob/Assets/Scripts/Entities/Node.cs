using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Node : FastPriorityQueueNode
{
    public int[] gridPos;
    public string name;

    public Node(int[] gridPos, string name)
    {
        this.gridPos = gridPos;
        this.name = name;
    }
}

