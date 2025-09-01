using Unity.VisualScripting;
using UnityEngine;

public class SC_interactive_object : MonoBehaviour
{
    private SC_Player_controller player;
    private bool canactivate = true;

    private SpriteRenderer arrow;

    public bool monologue;
    private GameObject monologue_box;
    private bool can_talk;
    private Animator animator;

    public bool collided;

    void Start()
    {
        can_talk = true;
        player = GameObject.FindWithTag("Player").GetComponent<SC_Player_controller>();
        arrow = transform.Find("Arrow").GetComponent<SpriteRenderer>();
        monologue_box = transform.Find("Monologue").gameObject;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collided && can_talk)
        {
            if (canactivate && Input.GetButtonDown("Fire1"))
            {
                if (monologue)
                {
                    Start_dialogue();
                }
                else
                {
                    Activate();
                }
            }
            if (canactivate)
            {
                arrow.enabled = true;
            }
        }
        else
        {
            arrow.enabled = false;
        }
    }


    public void Reset()
    {
        Invoke("Delay_Reset_NPC", 0.5f);
    }

    public void Delay_Reset_NPC()
    {
        can_talk = true;
        arrow.enabled = true;
    }


    void Start_dialogue()
    {
        can_talk = false;
        player.rb.linearVelocity = Vector2.zero;
        player_anim();
        arrow.enabled = false;
        GameObject I_dialogue;
        I_dialogue = Instantiate(monologue_box);
        I_dialogue.transform.parent = transform;
        I_dialogue.SetActive(true);
        I_dialogue.GetComponent<SC_Dialogue_system>().interactive = this;
        player.can_act = false;
        animator.enabled = true;
    }


    void Activate()
    {
        can_talk = false;
        animator.enabled = true;

    }

    void player_anim()
    {
        if (player.transform.position.x > transform.position.x && Mathf.Abs(player.transform.position.x - transform.position.x) > Mathf.Abs(player.transform.position.y - transform.position.y))
        {
            player.animator.SetBool("Side", true);
            player.animator.SetBool("Up", false);
            player.animator.SetBool("Down", false);
            player.animator.SetBool("Side", true);
            if (player.facing_right)
            {
                player.facing_right = false;
                player.Flip();
            }
        }

        if (player.transform.position.x < transform.position.x && Mathf.Abs(player.transform.position.x - transform.position.x) > Mathf.Abs(player.transform.position.y - transform.position.y))
        {
            player.animator.SetBool("Side", true);
            player.animator.SetBool("Up", false);
            player.animator.SetBool("Down", false);
            if (!player.facing_right)
            {
                player.facing_right = true;
                player.Flip();
            }
        }

        if (player.transform.position.y > transform.position.y && Mathf.Abs(player.transform.position.y - transform.position.y) > Mathf.Abs(player.transform.position.x - transform.position.x))
        {
            player.animator.SetBool("Side", false);
            player.animator.SetBool("Up", false);
            player.animator.SetBool("Down", true);
        }

        if (player.transform.position.y < transform.position.y && Mathf.Abs(player.transform.position.y - transform.position.y) > Mathf.Abs(player.transform.position.x - transform.position.x))
        {
            player.animator.SetBool("Side", false);
            player.animator.SetBool("Up", true);
            player.animator.SetBool("Down", false);
        }

        player.animator.ResetTrigger("Moving");
        player.animator.SetTrigger("Idle");

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collided = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collided = false;
        }
    }
}
