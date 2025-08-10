using UnityEngine;

public class S_Player_controller : MonoBehaviour
{
    public float Move_speed;
    private Animator animator;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Move_speed, Input.GetAxis("Vertical") * Move_speed);
    }
}
