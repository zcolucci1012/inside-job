using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawLevel : MonoBehaviour
{

    public GameObject sampleRoom;
    private Texture2D[] roomImages;
    private Texture2D[] bigRoomImages;
    public Texture2D endRoom;
    public Texture2D altEndRoom;
    public Texture2D store;
    public Transform playerTransform;
    private GameObject[] gameRooms;

    private Tilemap floor;
    private Tilemap walls;

    private LinkedList<GameObject> enemies;
    public GameObject intern;
    public GameObject stapler;
    public GameObject fileCabinet;
    public GameObject goal;
    public GameObject cashier;
    public GameObject CEO;
    public GameObject marketer;

    public GameObject minimap;

    public Tile[] tiles;
    public Tile[] storeTiles;
    private int[,] rooms;
    private int[,] doorsLocation;
    private int[,] doorsOpen;
    private bool[] roomOver;
    private bool[] roomVisited;
    private int[] roomDistances;
    private int furthestRoomDistance = 0;
    private int furthestRoomIndex = -1;
    private int storeIndex = -1;
    private int currentRoom = -1;
    private int[,] adjacentRooms;
    private string[] roomSize;
    
    
    // Start is called before the first frame update
    void Start()
    {
        enemies = new LinkedList<GameObject>();
        roomImages = Resources.LoadAll<Texture2D>("Tiles/Rooms");
        bigRoomImages = Resources.LoadAll<Texture2D>("Tiles/BigRooms");
        int numRooms = Random.Range(10, 13);
        GenerateMap(numRooms);
        gameRooms = new GameObject[numRooms];

        int allRooms = roomImages.Length;
        int allBigRooms = bigRoomImages.Length;
        bool[] used = new bool[allRooms];
        bool[] bigUsed = new bool[allBigRooms];
        for (int ii = 0; ii < allRooms; ii++)
        {
            used[ii] = false;
        }
        for (int ii = 0; ii < allBigRooms; ii++)
        {
            bigUsed[ii] = false;
        }

        this.storeIndex = Random.Range(1, numRooms);
        while (storeIndex == furthestRoomIndex
            || roomSize[storeIndex] == "big")                          
        {
            storeIndex = Random.Range(1, numRooms);
        }
        for (int ii = 0; ii < numRooms; ii++)
        {
            gameRooms[ii] = Instantiate(sampleRoom);
            gameRooms[ii].transform.SetParent(this.transform);
            int r = -1;
            if (roomSize[ii] == "small")
            {
                r = Random.Range(1, roomImages.Length);
            }
            if (roomSize[ii] == "big")
            {
                r = Random.Range(1, bigRoomImages.Length);
            }

            if (ii == 0)
            {
                r = 0;
            }
            if (ii == furthestRoomIndex)
            {
                if (adjacentRooms[ii, 0] != -1)
                {
                    DrawRoom(gameRooms[ii], rooms[ii, 0], rooms[ii, 1], altEndRoom, ii, "first", roomSize[ii]);
                }
                else
                {
                    DrawRoom(gameRooms[ii], rooms[ii, 0], rooms[ii, 1], endRoom, ii, "first", roomSize[ii]);
                }
            }
            else if (ii == storeIndex)
            {
                DrawRoom(gameRooms[ii], rooms[ii, 0], rooms[ii, 1], store, ii, "store", roomSize[ii]);
            }
            else
            {
                if (roomSize[ii] == "small")
                {
                    while (used[r])
                    {
                        r = Random.Range(1, roomImages.Length);
                    }
                    DrawRoom(gameRooms[ii], rooms[ii, 0], rooms[ii, 1], roomImages[r], ii, "first", roomSize[ii]);
                    used[r] = true;
                }
                else if (roomSize[ii] == "big"){
                    while (bigUsed[r])
                    {
                        r = Random.Range(1, bigRoomImages.Length);
                    }
                    //print(rooms[ii, 0] + ", " + rooms[ii, 1] + ": " + r);
                    DrawRoom(gameRooms[ii], rooms[ii, 0], rooms[ii, 1], bigRoomImages[r], ii, "first", roomSize[ii]);
                    bigUsed[r] = true;
                }
            }
            SpriteRenderer fog = gameRooms[ii].transform.GetChild(3).GetComponent<SpriteRenderer>();
            if (roomSize[ii] == "big")
            {
                fog.transform.localScale = new Vector3(2 * fog.transform.localScale.x,
                    2 * fog.transform.localScale.y,
                    1);
                fog.transform.position = new Vector3(fog.transform.position.x + Constants.ROOM_WIDTH / 2,
                    fog.transform.position.y + Constants.ROOM_HEIGHT / 2,
                    fog.transform.position.z);
            }
        }


        //GridData.PrintGrid();
    }

    void DrawRoom(GameObject gameRoom, int x, int y, Texture2D roomImage, int roomIndex, string style, string size)
    {
        int width = 0;
        int height = 0;
        switch (size)
        {
            case "small":
                width = Constants.ROOM_WIDTH;
                height = Constants.ROOM_HEIGHT;
                break;
            case "big":
                width = Constants.BIG_ROOM_WIDTH;
                height = Constants.BIG_ROOM_HEIGHT;
                break;
        }
        //intentionally Constants.ROOM_WIDTH/HEIGHT because pivot of big room is bottom left
        
        this.walls = gameRoom.transform.GetChild(0).GetComponent<Tilemap>();
        this.floor = gameRoom.transform.GetChild(1).GetComponent<Tilemap>();
        Transform doors = gameRoom.transform.GetChild(2);
        Tilemap upDoors = doors.GetChild(0).GetComponent<Tilemap>();
        Tilemap rightDoors = doors.GetChild(1).GetComponent<Tilemap>();
        Tilemap downDoors = doors.GetChild(2).GetComponent<Tilemap>();
        Tilemap leftDoors = doors.GetChild(3).GetComponent<Tilemap>();

        Tilemap upLeftDoors = null;
        Tilemap upRightDoors = null;
        Tilemap rightUpDoors = null;
        Tilemap rightDownDoors = null;
        Tilemap downRightDoors = null;
        Tilemap downLeftDoors = null;
        Tilemap leftDownDoors = null;
        Tilemap leftUpDoors = null;

        if (size == "big")
        {
            upLeftDoors = Instantiate(upDoors, doors, true);
            upLeftDoors.transform.position = upDoors.transform.position;
            upRightDoors = Instantiate(upDoors, doors, true);
            upRightDoors.transform.position = upDoors.transform.position;

            rightUpDoors = Instantiate(rightDoors, doors, true);
            rightUpDoors.transform.position = rightDoors.transform.position;
            rightDownDoors = Instantiate(rightDoors, doors, true);
            rightDownDoors.transform.position = rightDoors.transform.position;

            downRightDoors = Instantiate(downDoors, doors, true);
            downRightDoors.transform.position = downDoors.transform.position;
            downLeftDoors = Instantiate(downDoors, doors, true);
            downLeftDoors.transform.position = downDoors.transform.position;

            leftDownDoors = Instantiate(leftDoors, doors, true);
            leftDownDoors.transform.position = leftDoors.transform.position;
            leftUpDoors = Instantiate(leftDoors, doors, true);
            leftUpDoors.transform.position = leftDoors.transform.position;


            //Destroy(upDoors.GetComponent<TilemapCollider2D>());
            //Destroy(rightDoors.GetComponent<TilemapCollider2D>());
            //Destroy(downDoors.GetComponent<TilemapCollider2D>());
            //Destroy(leftDoors.GetComponent<TilemapCollider2D>());
            //Destroy(upDoors.GetComponent<TilemapRenderer>());
            //Destroy(rightDoors.GetComponent<TilemapRenderer>());
            //Destroy(downDoors.GetComponent<TilemapRenderer>());
            //Destroy(leftDoors.GetComponent<TilemapRenderer>());
            Destroy(upDoors.gameObject);
            Destroy(rightDoors.gameObject);
            Destroy(downDoors.gameObject);
            Destroy(leftDoors.gameObject);
        }

        gameRoom.transform.position = new Vector3(x * Constants.ROOM_WIDTH - Constants.ROOM_WIDTH / 2, y * Constants.ROOM_HEIGHT - Constants.ROOM_HEIGHT / 2, 0);




        for (int ii = 0; ii < width; ii++)
        {
            for (int jj = 0; jj < height; jj++)
            {
                if (ii == 0)
                {
                    if (jj == 0)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[15]);
                    }
                    else if (jj == height - 1)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[11]);
                    }
                    else if (jj == Constants.ROOM_HEIGHT / 2 - 1 && 
                        size == "small" && doorsLocation[roomIndex, 3] == 1)
                    {
                        leftDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[24]);
                    }
                    else if (jj == Constants.ROOM_HEIGHT / 2 - 1 &&
                        size == "big" && doorsLocation[roomIndex, 6] == 1)
                    {
                        leftDownDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[24]);
                    }
                    else if (jj == Constants.ROOM_HEIGHT / 2 &&
                        size == "small" && doorsLocation[roomIndex, 3] == 1)
                    {
                        leftDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[25]);
                    }
                    else if (jj == Constants.ROOM_HEIGHT / 2 &&
                        size == "big" && doorsLocation[roomIndex, 6] == 1)
                    {
                        leftDownDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[25]);
                    }
                    else if (size == "big"
                        && jj == Constants.ROOM_HEIGHT + Constants.ROOM_HEIGHT / 2 - 1
                        && doorsLocation[roomIndex, 7] == 1)
                    {
                        leftUpDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[24]);
                    }
                    else if (size == "big"
                        && jj == Constants.ROOM_HEIGHT + Constants.ROOM_HEIGHT / 2
                        && doorsLocation[roomIndex, 7] == 1)
                    {
                        leftUpDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[25]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(12, 14, tiles));
                    }
                    AddToGrid(x, y, ii, jj, "Wall");
                } else if (ii == width - 1)
                {
                    if (jj == 0)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[3]);
                    }
                    else if (jj == height - 1)
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[7]);
                    }
                    else if (jj == Constants.ROOM_HEIGHT / 2 - 1 &&
                        size == "small" && doorsLocation[roomIndex, 1] == 1)
                    {
                        rightDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[21]);
                    }
                    else if (jj == Constants.ROOM_HEIGHT / 2 - 1 &&
                        size == "big" && doorsLocation[roomIndex, 3] == 1)
                    {
                        rightDownDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[21]);
                    }
                    else if (jj == Constants.ROOM_HEIGHT / 2 &&
                        size == "small" && doorsLocation[roomIndex, 1] == 1)
                    {
                        rightDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[20]);
                    }
                    else if (jj == Constants.ROOM_HEIGHT / 2 &&
                        size == "big" && doorsLocation[roomIndex, 3] == 1)
                    {
                        rightDownDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[20]);
                    }
                    else if (size == "big"
                        && jj == Constants.ROOM_HEIGHT + Constants.ROOM_HEIGHT / 2 - 1
                        && doorsLocation[roomIndex, 2] == 1)
                    {
                        rightUpDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[21]);
                    }
                    else if (size == "big"
                        && jj == Constants.ROOM_HEIGHT + Constants.ROOM_HEIGHT / 2
                        && doorsLocation[roomIndex, 2] == 1)
                    {
                        rightUpDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[20]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(4, 6, tiles));
                    }
                    AddToGrid(x, y, ii, jj, "Wall");
                } else if (jj == 0)
                {
                    if (ii == Constants.ROOM_WIDTH / 2 - 1 &&
                        size == "small" && doorsLocation[roomIndex, 2] == 1)
                    {
                        downDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[27]);
                    }
                    else if (ii == Constants.ROOM_WIDTH / 2 - 1 &&
                        size == "big" && doorsLocation[roomIndex, 5] == 1)
                    {
                        downLeftDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[27]);
                    }
                    else if (ii == Constants.ROOM_WIDTH / 2 &&
                        size == "small" && doorsLocation[roomIndex, 2] == 1)
                    {
                        downDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[26]);
                    }
                    else if (ii == Constants.ROOM_WIDTH / 2 &&
                        size == "big" && doorsLocation[roomIndex, 5] == 1)
                    {
                        downLeftDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[26]);
                    }
                    else if (size == "big"
                        && ii == Constants.ROOM_WIDTH + Constants.ROOM_WIDTH / 2 - 1
                        && doorsLocation[roomIndex, 4] == 1)
                    {
                        downRightDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[27]);
                    }
                    else if (size == "big"
                        && ii == Constants.ROOM_WIDTH + Constants.ROOM_WIDTH / 2
                        && doorsLocation[roomIndex, 4] == 1)
                    {
                        downRightDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[26]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(0, 2, tiles));
                    }
                    AddToGrid(x, y, ii, jj, "Wall");
                } else if (jj == height - 1)
                {
                    if (ii == Constants.ROOM_WIDTH / 2 - 1 &&
                        size == "small" && doorsLocation[roomIndex, 0] == 1)
                    {
                        upDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[22]);
                    }
                    else if (ii == Constants.ROOM_WIDTH / 2 - 1 &&
                        size == "big" && doorsLocation[roomIndex, 0] == 1)
                    {
                        upLeftDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[22]);
                    }
                    else if (ii == Constants.ROOM_WIDTH / 2 &&
                        size == "small" && doorsLocation[roomIndex, 0] == 1)
                    {
                        upDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[23]);
                    }
                    else if (ii == Constants.ROOM_WIDTH / 2 &&
                        size == "big" && doorsLocation[roomIndex, 0] == 1)
                    {
                        upLeftDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[23]);
                    }
                    else if (size == "big"
                        && ii == Constants.ROOM_WIDTH + Constants.ROOM_WIDTH / 2 - 1
                        && doorsLocation[roomIndex, 1] == 1)
                    {
                        upRightDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[22]);
                    }
                    else if (size == "big"
                        && ii == Constants.ROOM_WIDTH + Constants.ROOM_WIDTH / 2
                        && doorsLocation[roomIndex, 1] == 1)
                    {
                        upRightDoors.SetTile(new Vector3Int(ii, jj, 0), tiles[23]);
                    }
                    else
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(8, 10, tiles));
                    }
                    AddToGrid(x, y, ii, jj, "Wall");
                }
                else if (roomImage.GetPixel(ii, jj) == Color.black)
                {
                    if (style == "first")
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), tiles[46]);
                    }
                    else if (style == "store")
                    {
                        walls.SetTile(new Vector3Int(ii, jj, 0), storeTiles[4]);
                    }
                    AddToGrid(x, y, ii, jj, "Wall");
                }
                else
                {
                    if (roomImage.GetPixel(ii, jj) == Color.red)
                    {
                        GameObject newIntern = Instantiate(intern, this.transform.parent, true);
                        AddToWorld(newIntern, ii, jj, roomImage);
                    }
                    else if (roomImage.GetPixel(ii, jj) == Color.blue)
                    {
                        GameObject newGoal = Instantiate(goal, this.transform.parent, true);
                        AddToWorld(newGoal, ii, jj, roomImage);
                    } else if (roomImage.GetPixel(ii, jj) == Color.green)
                    {
                        GameObject newFileCabinet = Instantiate(fileCabinet, this.transform.parent, true);
                        AddToWorld(newFileCabinet, ii, jj, roomImage);
                    } else if (roomImage.GetPixel(ii, jj) == new Color(1, 1, 0))
                    {
                        GameObject newStapler = Instantiate(stapler, this.transform.parent, true);
                        AddToWorld(newStapler, ii, jj, roomImage);
                    } else if (roomImage.GetPixel(ii, jj) == new Color(0, 1, 1))
                    {
                        AddToWorld(LootTables.Pickup(), ii, jj, roomImage);
                    } else if (roomImage.GetPixel(ii, jj) == new Color(1, 0, 1))
                    {
                        GameObject newCashier = Instantiate(cashier, this.transform.parent, true);
                        AddToWorld(newCashier, ii, jj, roomImage);
                    } else if (roomImage.GetPixel(ii, jj) == new Color(0.2f, 0.2f, 0.2f))
                    {
                        GameObject newCEO = Instantiate(CEO, this.transform.parent, true);
                        AddToWorld(newCEO, ii, jj, roomImage);
                    } else if (roomImage.GetPixel(ii, jj) == new Color(0, 0.8f, 0.8f))
                    {
                        GameObject newMarketer = Instantiate(marketer, this.transform.parent, true);
                        AddToWorld(newMarketer, ii, jj, roomImage);
                    }
                    
                }
                if (style == "first")
                {
                    floor.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(16, 19, tiles));
                }
                else if (style == "store")
                {
                    floor.SetTile(new Vector3Int(ii, jj, 0), GetRandomTile(0, 3, storeTiles));
                }
            }
        }
    }

    private void AddToGrid(int x, int y, int ii, int jj, string name)
    {
        GridData.grid.Add(new int[2] { x * Constants.ROOM_WIDTH - Constants.ROOM_WIDTH / 2 + ii,
            y * Constants.ROOM_HEIGHT - Constants.ROOM_HEIGHT / 2 + jj }, name);
    }

    private void AddToWorld(GameObject obj, int ii, int jj, Texture2D roomImage)
    {
        if (obj.layer == 10)
        {
            enemies.AddLast(obj);
        }
        Vector3 p = floor.CellToWorld(new Vector3Int(ii, jj, 0));
        //print(ii + ", " + jj);
        if ((ii == (Constants.ROOM_WIDTH / 2) || jj == (Constants.ROOM_HEIGHT / 2 - 1))
            && roomImage.GetPixel(ii - 1, jj).a == 0
            && roomImage.GetPixel(ii - 1, jj + 1).a == 0
            && roomImage.GetPixel(ii, jj + 1).a == 0)
        {
            obj.transform.position = new Vector3(p.x, p.y + 1f, p.z - 0.25f);
        } else
        {
            obj.transform.position = new Vector3(p.x + 0.5f, p.y + 0.5f, p.z - 0.25f);
        }
    }

    Tile GetRandomTile(int lb, int ub, Tile[] tiles)
    {
        int val = Random.Range(lb, ub + 1);
        return tiles[val];
    }

    void GenerateMap(int numRooms)
    {
        rooms = new int[numRooms, 2];
        doorsLocation = new int[numRooms, 8];
        doorsOpen = new int[numRooms, 8];
        roomOver = new bool[numRooms];
        roomVisited = new bool[numRooms];
        roomDistances = new int[numRooms];
        adjacentRooms = new int[numRooms, 8];
        roomSize = new string[numRooms];
        for (int ii = 0; ii < numRooms; ii++)
        {
            adjacentRooms[ii,0] = -1;
            adjacentRooms[ii,1] = -1;
            adjacentRooms[ii,2] = -1;
            adjacentRooms[ii,3] = -1;
            adjacentRooms[ii,4] = -1;
            adjacentRooms[ii,5] = -1;
            adjacentRooms[ii,6] = -1;
            adjacentRooms[ii,7] = -1;
        }

        rooms[0, 0] = 0;
        rooms[0, 1] = 0;
        roomOver[0] = true;
        roomVisited[0] = true;
        roomDistances[0] = 0;
        roomSize[0] = "small";
        string[] sizes = new string[2] { "small", "big" };

        int MAX_BIG_ROOMS = 2;
        int numBigRooms = 0;

        for (int ii = 1; ii < numRooms; ii++)
        {
            roomOver[ii] = false;
            roomVisited[ii] = false;
            int ss = Random.Range(0, ii);
            string size = sizes[Random.Range(0, 6) / 5];

            if (size == "small")
            {
                int dx;
                int dy;
                //small: {0, 1, 2, 3} -> {up, right, down, left}

                int oppositeDirection = -1;
                int direction = -1;
                int[] newRoom = null;
                if (roomSize[ss] == "small")
                {
                    direction = Random.Range(0, 4);

                    if (direction == 0 || direction == 1)
                    {
                        oppositeDirection = direction + 2;
                    }
                    else
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

                    newRoom = new int[2] { rooms[ss, 0] + dx, rooms[ss, 1] + dy };
                }
                else if (roomSize[ss] == "big")
                {
                    direction = Random.Range(0, 8);

                    if (direction <= 3)
                    {
                        oppositeDirection = (direction / 2) + 2;
                    }
                    else
                    {
                        oppositeDirection = (direction / 2) - 2;
                    }

                    switch (direction)
                    {
                        case 0:
                            dx = 0;
                            dy = 2;
                            break;
                        case 1:
                            dx = 1;
                            dy = 2;
                            break;
                        case 2:
                            dx = 2;
                            dy = 1;
                            break;
                        case 3:
                            dx = 2;
                            dy = 0;
                            break;
                        case 4:
                            dx = 1;
                            dy = -1;
                            break;
                        case 5:
                            dx = 0;
                            dy = -1;
                            break;
                        case 6:
                            dx = -1;
                            dy = 0;
                            break;
                        default:
                            dx = -1;
                            dy = 1;
                            break;
                    }

                    
                    newRoom = new int[2] { rooms[ss, 0] + dx, rooms[ss, 1] + dy };
                }
                

                bool found = false;
                for (int jj = 0; jj < ii; jj++)
                {
                    if (roomSize[jj] == "small")
                    {
                        if (rooms[jj, 0] == newRoom[0] && rooms[jj, 1] == newRoom[1])
                        {
                            found = true;
                            break;
                        }
                    } else if (roomSize[jj] == "big")
                    {
                        if (rooms[jj, 0] == newRoom[0] && rooms[jj, 1] == newRoom[1]
                            || rooms[jj, 0] + 1 == newRoom[0] && rooms[jj, 1] == newRoom[1]
                            || rooms[jj, 0] == newRoom[0] && rooms[jj, 1] + 1 == newRoom[1]
                            || rooms[jj, 0] + 1 == newRoom[0] && rooms[jj, 1] + 1 == newRoom[1])
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                {
                    ii--;
                }
                else
                {
                    rooms[ii, 0] = newRoom[0];
                    rooms[ii, 1] = newRoom[1];
                    roomSize[ii] = "small";
                    doorsLocation[ss, direction] = 1;
                    doorsLocation[ii, oppositeDirection] = 1;
                    roomDistances[ii] = roomDistances[ss] + 1;
                    adjacentRooms[ss, direction] = ii;
                    adjacentRooms[ii, oppositeDirection] = ss;
                    if (roomDistances[ii] > furthestRoomDistance)
                    {
                        furthestRoomDistance = roomDistances[ii];
                        furthestRoomIndex = ii;
                    }
                }
            }
            else if (size == "big")
            {
                int dx;
                int dy;
                //big: {0, 1, 2, 3, 4, 5, 6, 7} -> {up left, up right, right up, right down,
                //  down right, down left, left down, left up}

                //big room locations determined by the bottom left quadrant

                int oppositeDirection = -1;
                int direction = -1;
                int[] newRoom = null;
                if (roomSize[ss] == "small")
                {
                    direction = Random.Range(0, 4);
                    int offset = Random.Range(0, 2);

                    if (direction == 0 || direction == 1)
                    {
                        oppositeDirection = direction * 2 + 4 + offset;
                    }
                    else
                    {
                        oppositeDirection = direction * 2 - 4 + offset;
                    }

                    switch (direction)
                    {
                        case 0:
                            dx = -1 + offset;
                            dy = 1;
                            break;
                        case 1:
                            dx = 1;
                            dy = -offset;
                            break;
                        case 2:
                            dx = -offset;
                            dy = -2;
                            break;
                        default:
                            dx = -2;
                            dy = -1 + offset;
                            break;
                    }

                    newRoom = new int[2] { rooms[ss, 0] + dx, rooms[ss, 1] + dy };
                }
                else if (roomSize[ss] == "big")
                {
                    direction = Random.Range(0, 8);

                    if (direction <= 3)
                    {
                        oppositeDirection = direction + 4;
                    }
                    else
                    {
                        oppositeDirection = direction - 4;
                    }

                    switch (direction)
                    {
                        case 0:
                            dx = -1;
                            dy = 2;
                            break;
                        case 1:
                            dx = 1;
                            dy = 2;
                            break;
                        case 2:
                            dx = 2;
                            dy = 1;
                            break;
                        case 3:
                            dx = 2;
                            dy = -1;
                            break;
                        case 4:
                            dx = 1;
                            dy = -2;
                            break;
                        case 5:
                            dx = -1;
                            dy = -2;
                            break;
                        case 6:
                            dx = -2;
                            dy = -1;
                            break;
                        default:
                            dx = -2;
                            dy = 1;
                            break;
                    }

                    newRoom = new int[2] { rooms[ss, 0] + dx, rooms[ss, 1] + dy };
                }


                bool found = false;
                for (int jj = 0; jj < ii; jj++)
                {
                    if (roomSize[jj] == "small")
                    {
                        if (rooms[jj, 0] == newRoom[0] && rooms[jj, 1] == newRoom[1]
                            || rooms[jj, 0] == newRoom[0] + 1 && rooms[jj, 1] == newRoom[1]
                            || rooms[jj, 0] == newRoom[0] && rooms[jj, 1] == newRoom[1] + 1
                            || rooms[jj, 0] == newRoom[0] + 1 && rooms[jj, 1] == newRoom[1] + 1)
                        {
                            found = true;
                            break;
                        }
                    }
                    else if (roomSize[jj] == "big")
                    {
                        if (rooms[jj, 0] <= newRoom[0] + 1
                            && rooms[jj, 0] + 1 >= newRoom[0]
                            && rooms[jj, 1] <= newRoom[1] + 1
                            && rooms[jj, 1] + 1 >= newRoom[1])
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found || numBigRooms + 1 > MAX_BIG_ROOMS)
                {
                    ii--;
                }
                else
                {
                    rooms[ii, 0] = newRoom[0];
                    rooms[ii, 1] = newRoom[1];
                    roomSize[ii] = "big";
                    //print("big: " + newRoom[0] + ", " + newRoom[1]);
                    //print("parent: " + rooms[ss, 0] + ", " + rooms[ss, 1]);
                    doorsLocation[ss, direction] = 1;
                    doorsLocation[ii, oppositeDirection] = 1;
                    roomDistances[ii] = roomDistances[ss] + 1;
                    adjacentRooms[ss, direction] = ii;
                    adjacentRooms[ii, oppositeDirection] = ss;
                    numBigRooms++;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int ii = 0; ii < roomVisited.Length; ii++)
        {
            //manage fog
            SpriteRenderer fog = gameRooms[ii].transform.GetChild(3).GetComponent<SpriteRenderer>();
            if (!roomVisited[ii])
            {
                fog.color = new Color(0, 0, 0, 0.90f);
            } else
            {
                fog.color = new Color(0, 0, 0, 0);
            }

            //set room over if enemies found
            bool found = false;
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null && InRoom(rooms[ii, 0], rooms[ii, 1], enemy.transform.position.x, enemy.transform.position.y, roomSize[ii]))
                {
                    found = true;
                }
            }
            if (!found && roomVisited[ii])
            {
                roomOver[ii] = true;
            }

            //unlock doors if roomOver
            if (roomOver[ii])
            {
                if (doorsOpen[ii,0] == 0
                    || doorsOpen[ii, 1] == 0
                    || doorsOpen[ii, 2] == 0
                    || doorsOpen[ii, 3] == 0
                    || doorsOpen[ii, 4] == 0
                    || doorsOpen[ii, 5] == 0
                    || doorsOpen[ii, 6] == 0
                    || doorsOpen[ii, 7] == 0)
                {
                    for (int jj = 0; jj < adjacentRooms.GetLength(1); jj++)
                    {
                        int otherRoom = adjacentRooms[ii, jj];
                        if (otherRoom != -1) {
                            for (int kk = 0; kk < adjacentRooms.GetLength(1); kk++)
                            {
                                if(adjacentRooms[otherRoom, kk] == ii)
                                {
                                    //print("opened door " + kk + " in room " + otherRoom + " at " +
                                    //    rooms[otherRoom, 0] + ", " + rooms[otherRoom, 1]);
                                    doorsOpen[otherRoom, kk] = 1;
                                }
                            }
                        }
                    }
                }
                doorsOpen[ii, 0] = 1;
                doorsOpen[ii, 1] = 1;
                doorsOpen[ii, 2] = 1;
                doorsOpen[ii, 3] = 1;
                doorsOpen[ii, 4] = 1;
                doorsOpen[ii, 5] = 1;
                doorsOpen[ii, 6] = 1;
                doorsOpen[ii, 7] = 1;
            }

            for (int jj = 0; jj < gameRooms[ii].transform.GetChild(2).childCount; jj++)
            {
                if (doorsOpen[ii, jj] == 1)
                {
                    gameRooms[ii].transform.GetChild(2).GetChild(jj).GetComponent<DoorController>().Unlock();
                }
            }

            if (InRoom(rooms[ii, 0], rooms[ii, 1], playerTransform.position.x, playerTransform.position.y, roomSize[ii]))
            {
                //print("in room: " + ii + " at " + rooms[ii, 0] + ", " + rooms[ii, 1]);
                currentRoom = ii;
                roomVisited[ii] = true;
                if (!roomOver[ii])
                {
                    for (int jj = 0; jj < gameRooms[ii].transform.GetChild(2).childCount; jj++)
                    {
                        if (doorsOpen[ii, jj] == 1)
                        {
                            gameRooms[ii].transform.GetChild(2).GetChild(jj).GetComponent<DoorController>().CloseAndLock();
                            doorsOpen[ii, jj] = 0;
                        }
                    }
                } else
                {
                    for (int jj = 0; jj < gameRooms[ii].transform.GetChild(2).childCount; jj++)
                    { 
                        gameRooms[ii].transform.GetChild(2).GetChild(jj).GetComponent<DoorController>().Open();
                    }
                }
            }
        }
        minimap.GetComponent<Minimap>().SetValues(rooms, roomVisited, currentRoom, adjacentRooms, storeIndex, furthestRoomIndex, roomSize);
    }

    bool InRoom(int rx, int ry, float x, float y, string size)
    {
        if (size == "small")
        {
            return (x <= rx * Constants.ROOM_WIDTH + Constants.ROOM_WIDTH / 2 - 1
            && x > rx * Constants.ROOM_WIDTH - Constants.ROOM_WIDTH / 2 + 1
            && y <= ry * Constants.ROOM_HEIGHT + Constants.ROOM_HEIGHT / 2 - 1
            && y > ry * Constants.ROOM_HEIGHT - Constants.ROOM_HEIGHT / 2 + 1);
        }
        else if (size == "big")
        {
            return (x <= rx * Constants.ROOM_WIDTH + 3 * Constants.ROOM_WIDTH / 2 - 1
            && x > rx * Constants.ROOM_WIDTH - Constants.ROOM_WIDTH / 2 + 1
            && y <= ry * Constants.ROOM_HEIGHT + 3 * Constants.ROOM_HEIGHT / 2 - 1
            && y > ry * Constants.ROOM_HEIGHT - Constants.ROOM_HEIGHT / 2 + 1);
        }
        return false;
    }
}
