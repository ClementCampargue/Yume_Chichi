using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SC_tile_stair_detection : MonoBehaviour
{

    public List<Tile> tiles;
    public List<Tilemap> maps;

    public Tile tile;
    public float speed;
    private SC_Player_controller player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Player_controller>();
        var myItems = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
        foreach (Tilemap item in myItems)
        {
            maps.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < maps.Count; i++)
        {
            Vector3Int coordinate = maps[i].WorldToCell(transform.position);
            if (maps[i].HasTile(coordinate))
            {
                tile = maps[i].GetTile<Tile>(coordinate);
            }
            else
            {
                _out();
                return;
            }
        }

        if (tiles.Contains(tile))
        {
            _in();

        }
        else
        {

        }
    }


    void _in()
    {
        player.stair_speed = speed;
    }

    void _out()
    {
        player.stair_speed = 0;

    }
}
