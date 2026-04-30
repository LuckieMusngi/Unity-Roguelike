using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Roomgen : MonoBehaviour
{
    public int roomID;
    [SerializeField] private Tile cube;
    private Tilemap tmap;
    public int roomx, roomy;

    private void Awake()
    {
        tmap = GetComponent<Tilemap>();
    }

    private Vector2Int bl;
    //how far away from the wall the blocks should be. prevents blocks from spawning on the wall
    private const int away = 3;
    //cells is how big the grid will be. grid = (cells, cells);
    private const int cells = 3;
    public void MakeRoom(int roomX, int roomY)
    {
        roomx = roomX;
        roomy = roomY;
        transform.GetChild(0).localScale = new Vector3(roomX, roomY, 0);
        GetComponent<BoxCollider2D>().size = new Vector2(roomX - 1.5f, roomY - 1.5f);
        //makes rectangle
        bl = new Vector2Int(-roomX / 2, -roomY / 2);
        MakeRectangle(roomX, roomY, bl);

        //there was previously MakeBlocks() in here but now this entire function just looks sad
    }
    //MakeRectangle starts at the bottom left and goes outwards width and height amount. The perimeter is (width + 1) x (height + 1) and only creates perimeter.
    private void MakeRectangle(int width, int height, Vector2Int bottomleft)
    {
        Vector3Int bl = (Vector3Int)bottomleft;
        for (int x = 0; x != width + 1; x++)
        {
            tmap.SetTile(new Vector3Int(x, 0, 0) + bl, cube);
            tmap.SetTile(new Vector3Int(x, height, 0) + bl, cube);
        }
        for (int y = 0; y != height; y++)
        {
            tmap.SetTile(new Vector3Int(0, y, 0) + bl, cube);
            tmap.SetTile(new Vector3Int(width, y, 0) + bl, cube);
        }
    }

    public void MakeBlocks()
    {
        int amount = Random.Range(5, 9);
        int size = Random.Range(3, 7);
        int gsizeX = roomx / cells;
        int gsizeY = roomy / cells;
        Vector2Int gsize = new Vector2Int(gsizeX, gsizeY);
        Vector2Int[] current = new Vector2Int[amount];
        for (int i = 0; i < amount; i++)
        {
            current[i] = new Vector2Int(Random.Range(0, cells), Random.Range(0, cells));
            while (Utils.InArray(current, i))
            {
                current[i] = new Vector2Int(Random.Range(0, cells), Random.Range(0, cells));
            }
            //basically away will only happen if the block is gonna be near the wall.
            int awayX = current[i].x == 0 ? away : 0;
            int awayY = current[i].y == 0 ? away : 0;
            int intoX = current[i].x == cells - 1 ? away : 0;
            int intoY = current[i].y == cells - 1 ? away : 0;
            MakeBlock(size, bl
                + (current[i] * gsize)
                + new Vector2Int(Random.Range(awayX, gsizeX - intoX), Random.Range(awayY, gsizeY - intoY)));
        }
    }

    //snake-like algorithm
    private void MakeBlock(int size, Vector2Int center)
    {
        Vector3Int[] blocks = new Vector3Int[size];
        Vector3Int cr = (Vector3Int)center;
        Vector3Int currentblock = cr;
        Vector3Int avg = Vector3Int.zero;
        for (int i = 0; i < size; i++)
        {
            blocks[i] = currentblock;
            avg += currentblock;
            int rng = Random.Range(1, 5);
            int change = rng % 2 == 1 ? -1 : 1;
            currentblock += rng < 3 ? Vector3Int.right * change : Vector3Int.up * change;
        }
        avg /= blocks.Length;
        cr = avg - cr;
        for (int i = 0; i < blocks.Length; i++)
        {
            tmap.SetTile(blocks[i] - cr, cube);
        }
    }

    public void Open()
    {
        GetComponentInParent<Dungeon_gen>().Finish(roomID);
    }
    public void Close()
    {
        GetComponentInParent<Dungeon_gen>().CloseRooms();
    }

}