using UnityEngine;

public class SC_loop_tilemap : MonoBehaviour
{
    public float tileWidth = 100f;
    public float offset;
    void Update()
    {

            float distance = transform.position.x - GameObject.Find("Player").transform.position.x + offset;
            Debug.Log(distance);
            if (distance > 0)
            {
                transform.position -= new Vector3(tileWidth , 0f, 0f);
            }
            else if (distance < -tileWidth)
            {
                transform.position += new Vector3(tileWidth, 0f, 0f);
            }
    }
}
