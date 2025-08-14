using Unity.Cinemachine;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SC_Player_controller : MonoBehaviour
{
    public bool can_act;
    public float Move_speed;
    public Animator animator;
    public Rigidbody2D rb;

    public AudioSource footstep;

    public bool facing_right;

    public GameObject arrow;
    void Start()
    {
        can_act = true;
        facing_right = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (can_act)
        {
            check_movement();
            apply_movement();
        }
    }

    private void check_movement()
    {
        
        if (rb.linearVelocity != Vector2.zero)
        {

            if (Input.GetAxis("Vertical") > 0 && Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(Input.GetAxis("Vertical")))
            {

                animator.SetBool("Down", false);
                animator.SetBool("Side", false);
                animator.SetBool("Up", true);

                if (!facing_right)
                {
                    facing_right = true;
                    Flip();
                }

            }
            else if (Input.GetAxis("Vertical") < 0 && Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(Input.GetAxis("Vertical")))
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
            else if (Input.GetAxis("Horizontal") > 0 && Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Abs(Input.GetAxis("Horizontal")))
            {

                animator.SetBool("Down", false);
                animator.SetBool("Side", true);
                animator.SetBool("Up", false);

                if (!facing_right)
                {
                    facing_right = true;
                    Flip();
                }


            }
            else if (Input.GetAxis("Horizontal") < 0 && Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Abs(Input.GetAxis("Horizontal")))
            {

                animator.SetBool("Down", false);
                animator.SetBool("Side", true);
                animator.SetBool("Up", false);

                if (facing_right)
                {
                    facing_right = false;
                    Flip();
                }

            }

            animator.SetTrigger("Moving");
            if (!footstep.isPlaying)
            {
                footstep.Play();
            }

        }
        else if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            animator.SetTrigger("Idle");
            footstep.Stop();
        }
    }
    private void apply_movement()
    {
        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Move_speed, Input.GetAxis("Vertical") * Move_speed);
    }

    public void Flip()
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
