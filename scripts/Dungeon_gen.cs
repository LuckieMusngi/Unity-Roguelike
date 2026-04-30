using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dungeon_gen : MonoBehaviour
{
    [SerializeField] private Object p_room;
    [SerializeField] private Tilemap ghostTmap;
    //we will multiply the room min/maxes by 2 and add one later.
    public int roomXmin, roomXmax;
    public int roomYmin, roomYmax;
    private Tilemap tmap;
    [SerializeField] private Tile cube, square;
    public int distanceX, distanceY;
    private Object mapSquare;
    private Transform childTransform;

    public struct Room
    {
        public readonly GameObject go;
        public readonly Roomgen script;
        public readonly Vector2Int cr;
        public readonly Vector2Int size;
        public readonly Tilemap tmap;
        public List<Vector3Int> gates;
        public bool finished;
        public Room(Object p_room, Transform tr, Vector2Int center, Vector2Int roomSize, int roomID)
        {
            finished = false;
            gates = new List<Vector3Int>();
            cr = center;
            size = roomSize;
            go = (GameObject)Instantiate(p_room, (Vector2)center, Quaternion.identity);
            go.transform.parent = tr;
            tmap = go.GetComponent<Tilemap>();
            script = go.GetComponent<Roomgen>();
            script.roomID = roomID;
            script.MakeRoom(size.x, size.y);
        }
    }

    public int roomAmount = 50;
    public Room[] rooms;

    // Start is called before the first frame update
    private void Awake()
    {
        mapSquare = Resources.Load("prefabs/MinimapSquare");
        childTransform = transform.GetChild(0);
        ghostTmap = childTransform.GetComponent<Tilemap>();
        tmap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        rooms = new Room[roomAmount];
        _ = StartCoroutine(GenerateRooms());
    }

    private IEnumerator GenerateRooms()
    {
        List<int> path = new List<int>() { 0 };
        Vector2Int[] blocks = new Vector2Int[roomAmount];
        blocks[0] = Vector2Int.zero; Vector2Int currentblock = Vector2Int.zero;
        // plans where the rooms will be
        for (int i = 1; i < roomAmount; i++)
        {
            int rng = Random.Range(1, 5);
            int change = rng % 2 == 1 ? -1 : 1;
            currentblock += rng <= 2 ? Vector2Int.right * change : Vector2Int.up * change;
            blocks[i] = currentblock;
            if (InArray(blocks, i) == i + 1)
            {
                path.Add(i);
            }
            else
            {
                path.Add(InArray(blocks, i));
                i--;
            }
            yield return new WaitForEndOfFrame();
        }
        rooms[0] = new Room(p_room, transform, blocks[0] * new Vector2Int(distanceX, distanceY), new Vector2Int(21, 11), 0);
        rooms[0].tmap.color = Color.grey;
        // makes rooms
        for (int i = 1; i < blocks.Length; i++)
        {
            Vector2Int rsize = new Vector2Int(Random.Range(roomXmin, roomXmax), Random.Range(roomYmin, roomYmax));
            rooms[i] = new Room(p_room, transform, blocks[i] * new Vector2Int(distanceX, distanceY), (rsize * 2) + Vector2Int.one, i);
            yield return new WaitForEndOfFrame();
        }
        // makes paths
        for (int i = 1; i < path.Count; i++)
        {
            MakePath(rooms[path[i - 1]], rooms[path[i]]);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        // adds scripts (tells what kind of room it will be)
        for (int i = 1; i < rooms.Length; i++)
        {
            room_script room_Script = rooms[i].go.AddComponent<room_script>();
            room_Script.enemy_amount = Random.Range(4, 6);
        }
        //first level should be finished because it's given free;
        Finish(0);
    }

    private int InArray(Vector2Int[] array, int current)
    {
        for (int i = 0; i < current; i++)
        {
            if (array[current] == array[i])
            {
                return i;
            }
        }
        return current + 1;
    }

    private readonly Vector2 tileToWorld = Vector2.one * 0.5f;
    //only works if the rooms have centers that can sit on a line that's parallel to an axis.
    private void MakePath(Room from, Room to)
    {
        Vector2 center;
        Vector2Int totaldistance = to.cr - from.cr;
        bool vertical = totaldistance.x == 0;
        if (vertical)
        {
            bool up = totaldistance.y > 0;
            int distance = Mathf.Abs(totaldistance.y) - ((from.size.y + to.size.y) / 2);
            Vector2Int start = up ? from.cr + (Vector2Int.up * ((from.size.y / 2) + 1)) : to.cr + (Vector2Int.up * ((to.size.y / 2) + 1));
            center = start + (Vector2.up * distance / 2);
            for (int i = 0; i < distance; i++)
            {
                tmap.SetTile((Vector3Int)(start + new Vector2Int(-2, i)), cube);
                tmap.SetTile((Vector3Int)(start + new Vector2Int(2, i)), cube);
            }
            for (int i = 0; i < 3; i++)
            {
                Vector3Int fromgate = (Vector3Int.up * ((from.size.y / 2) + (up ? 1 : 0)) * (up ? 1 : -1)) + (Vector3Int.right * (-1 + i));
                Vector3Int togate = (Vector3Int.up * ((to.size.y / 2) + (up ? 0 : 1)) * (up ? -1 : 1)) + (Vector3Int.right * (-1 + i));

                if (!from.gates.Contains(fromgate)) { from.gates.Add(fromgate); }
                if (!to.gates.Contains(togate)) { to.gates.Add(togate); }
            }
            GameObject mapgo = Instantiate(mapSquare, center + tileToWorld, Quaternion.identity, childTransform) as GameObject;
            mapgo.transform.localScale = new Vector2(3, distance - 2);
            //-2 here is just bc it works. i have no idea why it's -2 here and nothing on horizontal;
        }
        else
        {
            bool right = totaldistance.x > 0;
            int distance = Mathf.Abs(totaldistance.x) - ((from.size.x + to.size.x) / 2);
            Vector2Int start = right ? from.cr + (Vector2Int.right * ((from.size.x / 2) + 1)) : to.cr + (Vector2Int.right * ((to.size.x / 2) + 1));
            center = start + (Vector2.right * distance / 2);
            for (int i = 0; i < distance; i++)
            {
                tmap.SetTile((Vector3Int)(start + new Vector2Int(i, -2)), cube);
                tmap.SetTile((Vector3Int)(start + new Vector2Int(i, 2)), cube);
            }
            for (int i = 0; i < 3; i++)
            {
                Vector3Int fromgate = (Vector3Int.right * ((from.size.x / 2) + (right ? 1 : 0)) * (right ? 1 : -1)) + (Vector3Int.up * (-1 + i));
                Vector3Int togate = (Vector3Int.right * ((to.size.x / 2) + (right ? 0 : 1)) * (right ? -1 : 1)) + (Vector3Int.up * (-1 + i));

                if (!from.gates.Contains(fromgate)) { from.gates.Add(fromgate); }
                if (!to.gates.Contains(togate)) { to.gates.Add(togate); }
            }
            GameObject mapgo = Instantiate(mapSquare, center + tileToWorld, Quaternion.identity, childTransform) as GameObject;
            mapgo.transform.localScale = new Vector2(distance, 3);
        }

    }
    public void Finish(int roomID)
    {
        rooms[roomID].finished = true;
        rooms[roomID].go.transform.GetChild(0).GetComponent<SpriteRenderer>().color -= Color.black * 0.5f;
        OpenRooms();
    }

    public void OpenRooms()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            OpenGates(i);
        }
    }
    public void CloseRooms()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (!rooms[i].finished)
            {
                CloseGates(i);
            }
        }
    }

    public void OpenGates(int roomID)
    {
        Room room = rooms[roomID];
        for (int i = 0; i < room.gates.Count; i++)
        {
            room.tmap.SetTile(room.gates[i], null);
            ghostTmap.SetTile(room.gates[i] + (Vector3Int)room.cr, cube);
        }
    }

    public void CloseGates(int roomID)
    {
        Room room = rooms[roomID];
        for (int i = 0; i < room.gates.Count; i++)
        {
            ghostTmap.SetTile(room.gates[i], null);
            room.tmap.SetTile(room.gates[i], cube);
        }
    }
}