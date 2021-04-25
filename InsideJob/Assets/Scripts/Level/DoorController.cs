using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorController : MonoBehaviour
{
    public Tile[] openDoors;
    public int direction;
    private bool unlocked = false;
    private bool open = false;
    private Vector3Int[] possibleLocations;

    // Start is called before the first frame update
    void Start()
    {
        this.unlocked = false;
        this.possibleLocations = new Vector3Int[] {
            //top
            new Vector3Int(Constants.ROOM_WIDTH / 2 - 1, Constants.ROOM_HEIGHT - 1, 0),
            new Vector3Int(Constants.ROOM_WIDTH / 2, Constants.ROOM_HEIGHT - 1, 0),
            new Vector3Int(Constants.ROOM_WIDTH / 2 - 1, Constants.BIG_ROOM_HEIGHT - 1, 0),
            new Vector3Int(Constants.ROOM_WIDTH + Constants.ROOM_WIDTH / 2 - 1, Constants.BIG_ROOM_HEIGHT - 1, 0),
            new Vector3Int(Constants.ROOM_WIDTH / 2, Constants.BIG_ROOM_HEIGHT - 1, 0),
            new Vector3Int(Constants.ROOM_WIDTH + Constants.ROOM_WIDTH / 2, Constants.BIG_ROOM_HEIGHT - 1, 0),
            //right
            new Vector3Int(Constants.ROOM_WIDTH - 1, Constants.ROOM_HEIGHT / 2 - 1, 0),
            new Vector3Int(Constants.ROOM_WIDTH - 1, Constants.ROOM_HEIGHT / 2, 0),
            new Vector3Int(Constants.BIG_ROOM_WIDTH - 1, Constants.ROOM_HEIGHT / 2 - 1, 0),
            new Vector3Int(Constants.BIG_ROOM_WIDTH - 1, Constants.ROOM_HEIGHT + Constants.ROOM_HEIGHT / 2 - 1, 0),
            new Vector3Int(Constants.BIG_ROOM_WIDTH - 1, Constants.ROOM_HEIGHT / 2, 0),
            new Vector3Int(Constants.BIG_ROOM_WIDTH - 1, Constants.ROOM_HEIGHT + Constants.ROOM_HEIGHT / 2, 0),
            //bottom
            new Vector3Int(Constants.ROOM_WIDTH / 2, 0, 0),
            new Vector3Int(Constants.ROOM_WIDTH / 2 - 1, 0, 0),
            new Vector3Int(Constants.ROOM_WIDTH + Constants.ROOM_WIDTH / 2 - 1, 0, 0),
            new Vector3Int(Constants.ROOM_WIDTH + Constants.ROOM_WIDTH / 2, 0, 0),
            //left
            new Vector3Int(0, Constants.ROOM_HEIGHT / 2, 0),
            new Vector3Int(0, Constants.ROOM_HEIGHT / 2 - 1, 0),
            new Vector3Int(0, Constants.ROOM_HEIGHT + Constants.ROOM_HEIGHT / 2 - 1, 0),
            new Vector3Int(0, Constants.ROOM_HEIGHT + Constants.ROOM_HEIGHT / 2, 0)
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player" && this.unlocked)
        {
            Open();
        }
    }

    public void Unlock()
    {
        this.unlocked = true;
    }

    public void CloseAndLock()
    {
        if (open)
        {
            Tilemap tiles = this.GetComponent<Tilemap>();

            for (int ii = 0; ii < possibleLocations.Length; ii++)
            {
                TileBase tile = tiles.GetTile(possibleLocations[ii]);
                if (tile != null) {
                    int tileNum = System.Int32.Parse(tile.name.Substring(7));
                    tile = openDoors[tileNum - 20];
                    tiles.SetTile(possibleLocations[ii], tile);
                }
            }

            this.GetComponent<TilemapCollider2D>().enabled = true;
            this.unlocked = false;
            this.open = false;
        }
    }


    public void Open()
    {
        if (!open)
        {
            Tilemap tiles = this.GetComponent<Tilemap>();

            for (int ii = 0; ii < possibleLocations.Length; ii++)
            {
                TileBase tile = tiles.GetTile(possibleLocations[ii]);
                if (tile != null)
                {
                    int tileNum = System.Int32.Parse(tile.name.Substring(7));
                    tile = openDoors[tileNum - 20];
                    tiles.SetTile(possibleLocations[ii], tile);
                }
            }

            this.GetComponent<TilemapCollider2D>().enabled = false;
            this.open = true;
        }  
    }
}
