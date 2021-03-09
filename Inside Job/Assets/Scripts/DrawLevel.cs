using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawLevel : MonoBehaviour
{
    public int ROOM_WIDTH = 16;
    public int ROOM_HEIGHT = 12;
    public GameObject sampleRoom;
    private Texture2D[] roomImages;
    private GameObject[] gameRooms;
    public Tile[] tiles;
    private int[,] rooms;
    private int[,] doors;
    
    // Start is called before the first frame update
    void Start()
    {
        roomImages = Resources.LoadAll<Texture2D>("Rooms/rooms");
        tiles = Resources.LoadAll<Tile>("Tiles/Floor1Tiles");

        int numRooms = Random.Range(8, 12);
        GenerateMap(numRooms);
        gameRooms = new GameObject[numRooms];
        for (int ii = 0; ii < numRooms; ii++)
        {
            gameRooms[ii] = Instantiate(sampleRoom);
            gameRooms[ii].transform.SetParent(this.transform);
            DrawRoom(gameRooms[ii], rooms[ii,0], rooms[ii,1]);
        }
    }

    void DrawRoom(GameObject gameRoom, int x, int y)
    {
        gameRoom.transform.position = new Vector3(x * ROOM_WIDTH - ROOM_WIDTH / 2, y * ROOM_HEIGHT - ROOM_HEIGHT / 2, 0);
        Tilemap walls = gameRoom.transform.GetChild(0).GetComponent<Tilemap>();
        Tilemap floors = gameRoom.transform.GetChild(1).GetComponent<Tilemap>();
        for (int ii = 0; ii < ROOM_WIDTH; ii++)
        {
            for (int jj = 0; jj < ROOM_HEIGHT; jj++)
            {
                if (ii == 0)
                {
                    if (jj == 0)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[15]);
                    }
                    else if (jj == ROOM_HEIGHT - 1)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[11]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(2, 2));
                    }
                } else if (ii == ROOM_WIDTH)
                {
                    if (jj == 0)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[3]);
                    }
                    else if (jj == ROOM_HEIGHT - 1)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[7]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(4, 6));
                    }
                } else if (jj == 0)
                {
                    walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(8, 10));
                } else if (jj == ROOM_HEIGHT - 1)
                {
                    walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(0, 2));
                }
            }
        }
    }

    Tile GetRandomTile(int lb, int ub)
    {
        int val = Random.Range(lb, ub + 1);
        return tiles[val];
    }

    void GenerateMap(int numRooms)
    {
        rooms = new int[numRooms, 2];
        doors = new int[numRooms - 1, 2];

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

    void DrawRoom(int x, int y)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
