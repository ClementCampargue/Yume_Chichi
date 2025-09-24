using Unity.Cinemachine;
using UnityEngine;

public class SC_Player_controller : MonoBehaviour
{
    public bool can_act;
    public float Move_speed;
    public Animator animator;
    public Rigidbody2D rb;
    public bool facing_right;
    [HideInInspector] public float stair_speed;
    private Vector2 respawn_coordinates;

    public bool sleeping;
    void Start()
    {
        respawn_coordinates = new Vector2(PlayerPrefs.GetFloat("Respawn_x" + PlayerPrefs.GetInt("Save")), PlayerPrefs.GetFloat("Respawn_y" + PlayerPrefs.GetInt("Save")));
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
            check_input();
        }
    }
    private void check_input()
    {
        if (Input.GetButtonDown("wake_up"))
        {
            if (sleeping)
            {
                wake_up();
            }
            else
            {
                go_back_to_sleep();
            }
        }
        if (Input.GetButtonDown("Action"))
        {
            perform_action();
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


        }
        else if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            animator.SetTrigger("Idle");
        }
    }
    private void apply_movement()
    {
        if (Input.GetAxis("Horizontal") > 0 && stair_speed !=0)
        {
            rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Move_speed, + stair_speed * Move_speed);
        }
        else if(Input.GetAxis("Horizontal") < 0 && stair_speed != 0)
        {
            rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Move_speed, -stair_speed * Move_speed);
        }
        else
        {
            rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * Move_speed, Input.GetAxis("Vertical") * Move_speed);
        }

    }

    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void perform_action()
    {

    }

    void wake_up()
    {
        PlayerPrefs.SetFloat("Respawn_x" + PlayerPrefs.GetInt("Save"), transform.position.x);
        PlayerPrefs.SetFloat("Respawn_y" + PlayerPrefs.GetInt("Save"), transform.position.y);
        respawn_coordinates = transform.position;
    }

    void go_back_to_sleep()
    {

    }

}
