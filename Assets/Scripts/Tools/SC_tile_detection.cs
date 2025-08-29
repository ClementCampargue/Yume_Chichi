using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class SC_tile_detection : MonoBehaviour
{

    public List<Tile> tiles;
    public AudioSource footsteps;
    public ParticleSystem FX;
    public List<Tilemap> maps;

    public Tile tile;
    void Start()
    {
        var myItems = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
        foreach (Tilemap item in myItems)
        {
            if(item.tag == "Ground")
            {
                maps.Add(item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < maps.Count; i++) 
        {
            Vector3Int coordinate = maps[i].WorldToCell(transform.position);
            if(maps[i].HasTile(coordinate))
            {
                tile = maps[i].GetTile<Tile>(coordinate);
            }
            else
            {
                Debug.Log("null"); 
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
            _out();
    
        }
    }


    void _in()
    {
        if (!footsteps.isPlaying)
        {
            footsteps.Play();
        }
        if (!FX.isPlaying)
        {
            FX.Play();
        }
    }

    void _out()
    {
        footsteps.Stop();
        FX.Stop();
    }
}
