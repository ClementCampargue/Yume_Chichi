using UnityEngine;

public class SC_Player_follow : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    public bool Horizontal_follow = true;
    public bool Vertical_follow = true;

    private Rigidbody2D rb;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position;

        if (Horizontal_follow)
        {
            newPosition.x = target.position.x + offset.x;
        }

        if (Vertical_follow)
        {
            newPosition.y = target.position.y + offset.y;
        }

        rb.MovePosition(newPosition);
    }
}
