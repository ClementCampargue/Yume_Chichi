using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SC_tile_detection : MonoBehaviour
{

    public List<Tile> tiles;
    public List<AudioSource> sources;
    public List<ParticleSystem> FX;
    [HideInInspector] public List<Tilemap> maps;

    void Start()
    {
        var myItems = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
        foreach (Tilemap item in myItems)
        {
            maps.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < maps.Count -1; i++) 
        {
            Vector3Int coordinate = maps[i].WorldToCell(transform.position);
            Debug.Log(maps[i].GetTile(coordinate).name);
        }


   
    }
}
