using UnityEngine;

public class SC_Player_follow : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    public bool Horizontal_follow = true;
    public bool Vertical_follow = true;
    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Horizontal_follow)
        {
            transform.position = new Vector3(offset.x + target.position.x, transform.position.y, transform.position.z);
        }
        if (Vertical_follow)
        {
            transform.position = new Vector3(transform.position.x, offset.y + target.position.y, transform.position.z);
        }
    }
}
