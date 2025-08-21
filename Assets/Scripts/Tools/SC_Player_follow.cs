using UnityEngine;

public class SC_Player_follow : MonoBehaviour
{
    private Transform player;
    public Vector2 offset;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(offset.x + player.position.x, offset.y + player.position.y) ;
    }
}
