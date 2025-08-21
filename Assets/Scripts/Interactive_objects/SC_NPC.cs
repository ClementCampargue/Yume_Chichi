using Unity.VisualScripting;
using UnityEngine;

public class SC_NPC : MonoBehaviour
{
    public float distance;

    private bool can_talk =true;
    private SC_Player_controller player;
    public GameObject dialogue;
    public SpriteRenderer arrow;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<SC_Player_controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(player.transform.position, transform.position)< distance)
        {
            if (can_talk && Input.GetButtonDown("Fire1"))
            {
                can_talk = false;
                Start_dialogue();
            }
            if (can_talk)
            {
                arrow.enabled =true;
            }
        }
        else
        {
            arrow.enabled = false;
        }
    }

    void Start_dialogue()
    {

        can_talk = false;
        player.rb.linearVelocity = Vector2.zero;
        player_anim();
        arrow.enabled = false;
        GameObject I_dialogue;
        I_dialogue = Instantiate(dialogue);
        I_dialogue.GetComponent<SC_Dialogue_system>().npc = this;
        player.can_act = false;
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

    public void Reset_NPC()
    {
        Invoke("Delay_Reset_NPC", 0.5f);
    }

    public void Delay_Reset_NPC()
    {
        can_talk = true;
        arrow.enabled = true;
    }
}
