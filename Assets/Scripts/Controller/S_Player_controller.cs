using UnityEditor.Tilemaps;
using UnityEngine;

public class S_Player_controller : MonoBehaviour
{
    public bool can_act;
    public float Move_speed;
    public Animator animator;
    private Rigidbody2D rb;

    public AudioSource footstep;

    private bool facing_right;

    public GameObject arrow;
    void Start()
    {
        can_act = true;
        facing_right = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (can_act)
        {
            check_movement();
            rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Move_speed, Input.GetAxis("Vertical") * Move_speed);
        }
    }

    private void check_movement()
    {

        if (rb.linearVelocity != Vector2.zero)
        {
            if (!footstep.isPlaying)
            {
                footstep.Play();
            }


            if (Mathf.Abs(rb.linearVelocity.x) < Mathf.Abs(rb.linearVelocity.y) && rb.linearVelocity.y > 0)
            {
                animator.SetBool("Up", true);
                animator.SetBool("Side", false);
                animator.SetBool("Down", false);
                if (!facing_right)
                {
                    facing_right = true;
                    Flip();
                }


            }
            else if (Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(rb.linearVelocity.y) && rb.linearVelocity.x > 0)
            {
                animator.SetBool("Side", true);
                animator.SetBool("Down", false);
                animator.SetBool("Up", false);

                if (!facing_right)
                {
                    facing_right = true;
                    Flip();
                }
            }
            else if (Mathf.Abs(rb.linearVelocity.x) < Mathf.Abs(rb.linearVelocity.y) && rb.linearVelocity.y < 0)
            {
                animator.SetBool("Down", true);
                animator.SetBool("Side", false);
                animator.SetBool("Up", false);
                if (!facing_right)
                {
                    facing_right = true;
                    Flip();
                }
            }
            else if(Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(rb.linearVelocity.y) && rb.linearVelocity.x < 0)
            {
                animator.SetBool("Side", true);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);

                if (facing_right)
                {
                    facing_right = false;
                    Flip();
                }
            }

            animator.SetTrigger("Moving");

        }
        else
        {
            animator.SetTrigger("Idle");
            footstep.Stop();
        }
        


    }


    private void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    public void Display_arrow()
    {
        arrow.SetActive(true);
    }

    public void Hide_arrow()
    {
        arrow.SetActive(false);
    }

}
