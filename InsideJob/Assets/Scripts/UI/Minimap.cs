using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    private int[,] rooms;
    private bool[] roomVisited;
    private int currentRoom;
    private int[,] adjacentRooms;
    private int shopIndex;
    private int endIndex;
    private string[] roomSize;
    public GameObject square;
    public GameObject shopIcon;
    public GameObject bossIcon;
    private GameObject[] roomSquares;
    private bool initialized = false;
    private float squareSize = 0;
    private bool hasBlueprints = false;


    public void SetValues(int[,] rooms, bool[] roomVisited, int currentRoom, int[,] adjacentRooms, int shopIndex, int endIndex, string[] roomSize)
    {
        this.rooms = rooms;
        this.roomVisited = roomVisited;
        this.currentRoom = currentRoom;
        this.adjacentRooms = adjacentRooms;
        this.shopIndex = shopIndex;
        this.endIndex = endIndex;
        this.roomSize = roomSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        squareSize = ((RectTransform)square.transform).rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (rooms != null)
        {
            if (this.initialized == false)
            {
                roomSquares = new GameObject[rooms.GetLength(0)];
                for (int ii = 0; ii < rooms.GetLength(0); ii++)
                {
                    GameObject newSquare = Instantiate(square, this.transform, true);
                    float x = (rooms[ii, 0] - rooms[currentRoom, 0]) * (squareSize + 10); 
                    float y = (rooms[ii, 1] - rooms[currentRoom, 1]) * (squareSize + 10);
                    newSquare.transform.localPosition = new Vector3(x, y, newSquare.transform.position.z);
                    if (roomSize[ii] == "big")
                    {   
                        newSquare.transform.localScale *= (squareSize * 2 + 10) / squareSize;
                        newSquare.transform.localPosition += new Vector3(squareSize / 2 + 10, squareSize / 2 + 10);
                    }

                    if (ii == shopIndex)
                    {
                        GameObject newShopIcon = Instantiate(shopIcon, newSquare.transform, true);
                        newShopIcon.transform.localPosition = new Vector3(0, 0, newSquare.transform.position.z);
                        newShopIcon.SetActive(true);
                    }
                    else if (ii == endIndex)
                    {
                        GameObject newBossIcon = Instantiate(bossIcon, newSquare.transform, true);
                        newBossIcon.transform.localPosition = new Vector3(0, 0, newSquare.transform.position.z);
                        newBossIcon.SetActive(true);
                    }

                    if (roomVisited[ii])
                    {
                        newSquare.SetActive(true);
                    }
                    else
                    {
                        for (int jj = 0; jj < 4; jj++)
                        {
                            if (adjacentRooms[ii, jj] != -1 && roomVisited[adjacentRooms[ii, jj]])
                            {
                                newSquare.SetActive(true);
                            }
                        }
                    }
                    roomSquares[ii] = newSquare;
                }
                this.initialized = true;
            } else
            {
                for (int ii = 0; ii < rooms.GetLength(0); ii++)
                {
                    if (roomVisited[ii])
                    {
                        roomSquares[ii].GetComponent<Image>().color = new Color32(225, 225, 225, 150);
                        
                    }
                    else
                    {
                        roomSquares[ii].GetComponent<Image>().color = new Color32(150, 150, 150, 150);
                    }

                    float x = (rooms[ii, 0] - rooms[currentRoom, 0]) * (squareSize + 10);
                    float y = (rooms[ii, 1] - rooms[currentRoom, 1]) * (squareSize + 10);
                    if (roomSize[this.currentRoom] == "big")
                    {
                        x -= squareSize / 2 + 5;
                        y -= squareSize / 2 + 5;
                    }
                    roomSquares[ii].transform.localPosition = new Vector3(x, y, roomSquares[ii].transform.position.z);
                    if (roomSize[ii] == "big")
                    {
                        roomSquares[ii].transform.localPosition += new Vector3(squareSize / 2 + 5, squareSize / 2 + 5);
                    }


                    if (roomVisited[ii] || hasBlueprints)
                    {
                        roomSquares[ii].SetActive(true);
                    }
                    else
                    {
                        for (int jj = 0; jj < 8; jj++)
                        {
                            if (adjacentRooms[ii, jj] != -1 && roomVisited[adjacentRooms[ii, jj]])
                            {
                                roomSquares[ii].SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }

    public void SetHasBlueprints(bool hasBlueprints)
    {
        this.hasBlueprints = hasBlueprints;
    }
}
