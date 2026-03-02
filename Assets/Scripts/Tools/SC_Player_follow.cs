using UnityEngine;

public class SC_Player_follow : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    public bool Horizontal_follow = true;
    public bool Vertical_follow = true;


    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector2 newPosition = transform.position;

        if (Horizontal_follow)
        {
            newPosition.x = target.position.x + offset.x;
        }

        if (Vertical_follow)
        {
            newPosition.y = target.position.y + offset.y;
        }

        transform.position = newPosition;
    }
}
