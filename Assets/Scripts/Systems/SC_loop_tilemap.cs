using UnityEngine;

public class SC_loop_tilemap : MonoBehaviour
{
    public Transform[] backgroundTiles;
    public float tileWidth = 100f;

    void Update()
    {
        foreach (Transform tile in backgroundTiles)
        {
            float distance = tile.position.x - GameObject.Find("Player").transform.position.x;
            Debug.Log(distance);
            if (distance > 0)
            {
                tile.position -= new Vector3(tileWidth * backgroundTiles.Length, 0f, 0f);
            }
            else if (distance < -tileWidth)
            {
                tile.position += new Vector3(tileWidth * backgroundTiles.Length, 0f, 0f);
            }
        }
    }
}
