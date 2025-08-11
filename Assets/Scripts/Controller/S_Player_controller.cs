using UnityEditor.Tilemaps;
using UnityEngine;

public class S_Player_controller : MonoBehaviour
{
    public float Move_speed;
    public Animator animator;
    private Rigidbody rb;

    public AudioSource footstep;

    private bool facing_right;
    public int velocity_to_anim;
    void Start()
    {
        facing_right = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        chack_movement();
  




        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Move_speed, Input.GetAxis("Vertical") * Move_speed);
    }

    private void chack_movement()
    {
        if (rb.linearVelocity.x < 0 && facing_right)
        {
            facing_right = false;
            Flip();

        }
        if (rb.linearVelocity.x > 0 && !facing_right)
        {
            facing_right = true;
            Flip();
        }

        if (rb.linearVelocity != Vector3.zero)
        {
            if (!footstep.isPlaying)
            {
                footstep.Play();
            }


            if (rb.linearVelocity.x < velocity_to_anim && rb.linearVelocity.x > -velocity_to_anim && rb.linearVelocity.y > 0)
            {
                animator.SetBool("Up", true);
                animator.SetBool("Side", false);
                animator.SetBool("Down", false);

            }
            else if (rb.linearVelocity.y < velocity_to_anim && rb.linearVelocity.y > -velocity_to_anim && rb.linearVelocity.x > 0)
            {
                animator.SetBool("Side", true);
                animator.SetBool("Down", false);
                animator.SetBool("Up", false);

            }
            else if (rb.linearVelocity.x < velocity_to_anim && rb.linearVelocity.x > -velocity_to_anim && rb.linearVelocity.y < 0)
            {
                animator.SetBool("Down", true);
                animator.SetBool("Side", false);
                animator.SetBool("Up", false);

            }
            else if(rb.linearVelocity.y < velocity_to_anim && rb.linearVelocity.y > -velocity_to_anim && rb.linearVelocity.x < 0)
            {
                animator.SetBool("Side", true);
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);

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

}
