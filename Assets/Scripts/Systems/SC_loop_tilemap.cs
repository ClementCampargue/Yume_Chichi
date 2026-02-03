using UnityEngine;

public class SC_loop_tilemap : MonoBehaviour
{
    public Vector3 Chunk_count;
    private Transform trs;
    private void Start()
    {
        trs = GameObject.Find("Player").transform;
        Chunk_count = Chunk_count * transform.localScale.x;
    }
    void Update()
    {
        float distancex = transform.position.x - trs.position.x;
        float distancey = transform.position.y - trs.position.y;

        if (Mathf.Abs(distancex) > 0)
        {
            int offset = Mathf.RoundToInt(distancex / Chunk_count.x);
            transform.position -= new Vector3(Chunk_count.x* offset, 0, 0);
        }

        if (Mathf.Abs(distancex) >0)
        {
            int offset = Mathf.RoundToInt(distancey / Chunk_count.y);
            transform.position -= new Vector3(0, Chunk_count.y  * offset, 0);
        }
    }
}
