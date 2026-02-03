using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SC_footstep_master : MonoBehaviour
{
    public List<Tile> tiles;
    public List<Tilemap> maps;

    public Tile tile;
    public Sprite sprite;
    public List<SpriteRenderer> sprites;

    [HideInInspector] public string currenttag;

    void Start()
    {
        var sprits = FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);
        foreach (SpriteRenderer item in sprits)
        {
            if (item.gameObject.name != "Ground_Shadow")
            {
                sprites.Add(item);
            }
        }

        var myItems = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
        foreach (Tilemap item in myItems)
        {
            if (item.tag == "Ground")
            {
                maps.Add(item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        var spritesUnderPlayer = sprites
    .Where(sr => sr.bounds.Contains(transform.position))
    .OrderByDescending(sr => sr.sortingLayerID * 1000 + sr.sortingOrder);

        if (spritesUnderPlayer.Any())
        {
            SpriteRenderer topSprite = spritesUnderPlayer.First();
            //Debug.Log("Sprite au-dessus du joueur : " + topSprite.tag);

            currenttag = topSprite.tag;

            return;

        }
        else
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
                    Debug.Log("null");
                    //  _out();
                    return;
                }
            }

            if (tiles.Contains(tile))
            {
                //  _in();

            }
            else
            {
              //  _out();

            }
        }

    }

}
