using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorController : MonoBehaviour
{
    public Tile[] openDoors;
    public int ROOM_WIDTH;
    public int ROOM_HEIGHT;
    public int direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            Tilemap tiles = this.GetComponent<Tilemap>();

            Vector3Int cell1;
            Vector3Int cell2;
            if (direction == 0)
            {
                cell1 = new Vector3Int(ROOM_WIDTH / 2 - 1, ROOM_HEIGHT - 1, 0);
                cell2 = new Vector3Int(ROOM_WIDTH / 2, ROOM_HEIGHT - 1, 0);
            }
            else if (direction == 1)
            {
                cell1 = new Vector3Int(ROOM_WIDTH - 1, ROOM_HEIGHT / 2 - 1, 0);
                cell2 = new Vector3Int(ROOM_WIDTH - 1, ROOM_HEIGHT / 2, 0);
            }
            else if (direction == 2)
            {
                cell1 = new Vector3Int(ROOM_WIDTH / 2, 0, 0);
                cell2 = new Vector3Int(ROOM_WIDTH / 2 - 1, 0, 0);
            }
            else
            {
                cell1 = new Vector3Int(0, ROOM_HEIGHT / 2, 0);
                cell2 = new Vector3Int(0, ROOM_HEIGHT / 2 - 1, 0);
            }


            TileBase tile1 = tiles.GetTile(cell1);
            TileBase tile2 = tiles.GetTile(cell2);
            if (tile1 != null && tile2 != null)
            { 
                int tileNum1 = System.Int32.Parse(tile1.name.Substring(7));
                tile1 = openDoors[tileNum1 - 20];
                int tileNum2 = System.Int32.Parse(tile2.name.Substring(7));
                tile2 = openDoors[tileNum2 - 20];
                tiles.SetTile(cell1, tile1);
                tiles.SetTile(cell2, tile2);
            }
                    
            this.GetComponent<TilemapCollider2D>().enabled = false;
        }
    }
}
