using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawLevel : MonoBehaviour
{
    private Tilemap[] floors;
    private Tilemap[] walls;
    private Tile[] tiles;
    
    // Start is called before the first frame update
    void Start()
    {
        tiles = Resources.LoadAll<Tile>("Tiles/Floor1Tiles");

        int numRooms = Random.Range(8, 12);
        GenerateMap(numRooms);

    }

    void GenerateMap(int numRooms)
    {
        int[,] rooms = new int[numRooms, 2];
        int[,] doors = new int[numRooms - 1, 2];

        rooms[0, 0] = 0;
        rooms[0, 1] = 0;

        for (int ii = 1; ii < numRooms; ii++)
        {
            
            int ss = Random.Range(0, ii);
            
            int dx;
            int dy;
            int direction = Random.Range(0, 4);
            switch (direction)
            {
                case 0:
                    dx = 0;
                    dy = 1;
                    break;
                case 1:
                    dx = 1;
                    dy = 0;
                    break;
                case 2:
                    dx = 0;
                    dy = -1;
                    break;
                default:
                    dx = -1;
                    dy = 0;
                    break;
            }

            int[] newRoom = new int[2] { rooms[ss,0] + dx, rooms[ss,1] + dy};

            bool found = false;
            for (int jj = 0; jj < ii; jj++)
            {
                if (rooms[jj, 0] == newRoom[0] && rooms[jj, 1] == newRoom[1])
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                ii--;
            } else
            {
                rooms[ii, 0] = newRoom[0];
                rooms[ii, 1] = newRoom[1];
                doors[ii - 1, 0] = ss;
                doors[ii - 1, 1] = ii;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
