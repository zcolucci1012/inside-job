using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawLevel : MonoBehaviour
{
    public float ROOM_WIDTH;
    public float ROOM_HEIGHT;
    public GameObject sampleRoom;
    private Texture2D[] roomImages;
    public Texture2D endRoom;
    public Transform playerTransform;
    private GameObject[] gameRooms;
    private LinkedList<GameObject> enemies;
    public GameObject intern;
    public GameObject goal;
    public Tile[] tiles;
    private int[,] rooms;
    private int[,] doorsLocation;
    private int[,] doorsOpen;
    private bool[] roomOver;
    private bool[] roomVisited;
    private int[] roomDistances;
    private int furthestRoomDistance = 0;
    private int furthestRoomIndex = -1;
    
    
    // Start is called before the first frame update
    void Start()
    {
        enemies = new LinkedList<GameObject>();
        roomImages = Resources.LoadAll<Texture2D>("Tiles/Rooms");
        int numRooms = Random.Range(8, 12);
        GenerateMap(numRooms);
        Debug.Log(rooms[furthestRoomIndex, 0] + ", " + rooms[furthestRoomIndex, 1] + ": " + furthestRoomDistance);
        gameRooms = new GameObject[numRooms];
        for (int ii = 0; ii < numRooms; ii++)
        {
            gameRooms[ii] = Instantiate(sampleRoom);
            gameRooms[ii].transform.SetParent(this.transform);
            int r = Random.Range(1, roomImages.Length);
            if (ii == 0)
            {
                r = 0;
            }
            if (ii == furthestRoomIndex) //probably change which room is goal
            {
                DrawRoom(gameRooms[ii], rooms[ii, 0], rooms[ii, 1], endRoom, ii);
            } else
            {
                DrawRoom(gameRooms[ii], rooms[ii, 0], rooms[ii, 1], roomImages[r], ii);
            }
            
        }
        Destroy(intern);
        Destroy(goal);
    }

    void DrawRoom(GameObject gameRoom, int x, int y, Texture2D roomImage, int roomIndex)
    {
        gameRoom.transform.position = new Vector3(x * ROOM_WIDTH - ROOM_WIDTH / 2, y * ROOM_HEIGHT - ROOM_HEIGHT / 2, 0);
        Tilemap walls = gameRoom.transform.GetChild(0).GetComponent<Tilemap>();
        Tilemap floor = gameRoom.transform.GetChild(1).GetComponent<Tilemap>();
        Transform doors = gameRoom.transform.GetChild(2);
        Tilemap upDoors = doors.GetChild(0).GetComponent<Tilemap>();
        Tilemap rightDoors = doors.GetChild(1).GetComponent<Tilemap>();
        Tilemap downDoors = doors.GetChild(2).GetComponent<Tilemap>();
        Tilemap leftDoors = doors.GetChild(3).GetComponent<Tilemap>();

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
                    else if (jj == ROOM_HEIGHT / 2 - 1 && doorsLocation[roomIndex, 3] == 1)
                    {
                        leftDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[24]);
                    }
                    else if (jj == ROOM_HEIGHT / 2  && doorsLocation[roomIndex, 3] == 1)
                    {
                        leftDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[25]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(12, 14));
                    }
                } else if (ii == ROOM_WIDTH - 1)
                {
                    if (jj == 0)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[3]);
                    }
                    else if (jj == ROOM_HEIGHT - 1)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[7]);
                    }
                    else if (jj == ROOM_HEIGHT / 2 - 1&& doorsLocation[roomIndex, 1] == 1)
                    {
                        rightDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[21]);
                    }
                    else if (jj == ROOM_HEIGHT / 2 && doorsLocation[roomIndex, 1] == 1)
                    {
                        rightDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[20]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(4, 6));
                    }
                } else if (jj == 0)
                {
                    if (ii == ROOM_WIDTH / 2 - 1&& doorsLocation[roomIndex, 2] == 1)
                    {
                        downDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[27]);
                    }
                    else if (ii == ROOM_WIDTH / 2 && doorsLocation[roomIndex, 2] == 1)
                    {
                        downDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[26]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(0, 2));
                    }
                } else if (jj == ROOM_HEIGHT - 1)
                {
                    if (ii == ROOM_WIDTH / 2 - 1 && doorsLocation[roomIndex, 0] == 1)
                    {
                        upDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[22]);
                    }
                    else if (ii == ROOM_WIDTH / 2 && doorsLocation[roomIndex, 0] == 1)
                    {
                        upDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[23]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(8, 10));
                    }
                } else if (roomImage.GetPixel(ii, jj) == Color.black)
                {
                    walls.SetTile(new Vector3Int(ii, jj, 0), tiles[46]);
                } else if (roomImage.GetPixel(ii, jj) == Color.red)
                {
                    GameObject newIntern = Instantiate(intern, this.transform.parent, true);
                    enemies.AddLast(newIntern);
                    Vector3 e = floor.CellToWorld(new Vector3Int(ii, jj, 0));
                    newIntern.transform.position = new Vector3(e.x + 0.5f, e.y + 0.5f, e.z - 0.25f);
                    floor.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(16, 19));
                } else if (roomImage.GetPixel(ii, jj) == Color.blue)
                {
                    //TEMPORARY GOAL
                    GameObject newGoal = Instantiate(goal, this.transform.parent, true);
                    Vector3 g = floor.CellToWorld(new Vector3Int(ii, jj, 0));
                    newGoal.transform.position = new Vector3(g.x + 0.5f, g.y + 0.5f, g.z - 0.25f);
                    floor.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(16, 19));
                }
                else
                {
                    floor.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(16, 19));
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
        doorsLocation = new int[numRooms, 4];
        doorsOpen = new int[numRooms, 4];
        roomOver = new bool[numRooms];
        roomVisited = new bool[numRooms];
        roomDistances = new int[numRooms];


        rooms[0, 0] = 0;
        rooms[0, 1] = 0;
        roomOver[0] = true;
        roomVisited[0] = true;
        roomDistances[0] = 0;

        for (int ii = 1; ii < numRooms; ii++)
        {
            roomOver[ii] = false;
            roomVisited[ii] = false;
            int ss = Random.Range(0, ii);
            
            int dx;
            int dy;
            int direction = Random.Range(0, 4);
            int oppositeDirection;
            if (direction == 0 || direction == 1)
            {
                oppositeDirection = direction + 2;
            } else
            {
                oppositeDirection = direction - 2;
            }
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
                doorsLocation[ss, direction] = 1;
                doorsLocation[ii, oppositeDirection] = 1;
                roomDistances[ii] = roomDistances[ss] + 1;
                if (roomDistances[ii] > furthestRoomDistance)
                {
                    furthestRoomDistance = roomDistances[ii];
                    furthestRoomIndex = ii;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int ii = 0; ii < roomVisited.Length; ii++)
        {
            SpriteRenderer fog = gameRooms[ii].transform.GetChild(3).GetComponent<SpriteRenderer>();
            if (!roomVisited[ii])
            {
                fog.color = new Color(0, 0, 0, 0.90f);
            } else
            {
                fog.color = new Color(0, 0, 0, 0);
            }

            bool found = false;
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null && InRoom(rooms[ii, 0], rooms[ii, 1], enemy.transform.position.x, enemy.transform.position.y))
                {
                    found = true;
                }
            }
            if (!found && roomVisited[ii])
            {
                roomOver[ii] = true;
            }

            if (roomOver[ii])
            {
                if (doorsOpen[ii,0] == 0
                    || doorsOpen[ii, 1] == 0
                    || doorsOpen[ii, 2] == 0
                    || doorsOpen[ii, 3] == 0)
                {
                    for (int jj = 0; jj < roomOver.Length; jj++)
                    {
                        if (rooms[ii, 0] == rooms[jj, 0])
                        {
                            if (rooms[ii, 1] + 1 == rooms[jj, 1])
                            {
                                doorsOpen[jj, 2] = 1;
                            }
                            if (rooms[ii, 1] - 1 == rooms[jj, 1])
                            {
                                doorsOpen[jj, 0] = 1;
                            }
                        }
                        if (rooms[ii, 1] == rooms[jj, 1])
                        {
                            if (rooms[ii, 0] + 1 == rooms[jj, 0])
                            {
                                doorsOpen[jj, 3] = 1;
                            }
                            if (rooms[ii, 0] - 1 == rooms[jj, 0])
                            {
                                doorsOpen[jj, 1] = 1;
                            }
                        }
                    }
                }
                doorsOpen[ii, 0] = 1;
                doorsOpen[ii, 1] = 1;
                doorsOpen[ii, 2] = 1;
                doorsOpen[ii, 3] = 1;
            }

            for (int jj = 0; jj < 4; jj++)
            {
                if (doorsOpen[ii, jj] == 1)
                {
                    gameRooms[ii].transform.GetChild(2).GetChild(jj).GetComponent<DoorController>().Open();
                }
            }

            if (InRoom(rooms[ii, 0], rooms[ii, 1], playerTransform.position.x, playerTransform.position.y))
            {
                roomVisited[ii] = true;
                if (!roomOver[ii])
                {
                    for (int jj = 0; jj < 4; jj++)
                    {
                        if (doorsOpen[ii, jj] == 1)
                        {
                            gameRooms[ii].transform.GetChild(2).GetChild(jj).GetComponent<DoorController>().CloseAndLock();
                            doorsOpen[ii, jj] = 0;
                        }
                    }
                }
            }
        }
        
    }

    bool InRoom(int rx, int ry, float x, float y)
    {
        return (x <= rx * ROOM_WIDTH + ROOM_WIDTH / 2 - 1
            && x > rx * ROOM_WIDTH - ROOM_WIDTH / 2 + 1
            && y <= ry * ROOM_HEIGHT + ROOM_HEIGHT / 2 - 1
            && y > ry * ROOM_HEIGHT - ROOM_HEIGHT / 2 + 1); 
    }

}
