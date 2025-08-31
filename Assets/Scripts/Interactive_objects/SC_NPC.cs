using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SC_NPC : MonoBehaviour
{
    public bool collided;

    private bool can_talk =true;
    private SC_Player_controller player;
    private SpriteRenderer arrow;

    private Transform point_to_talk; 
    private Animator animator;

    private int index;
    private List<GameObject> dialogues;

    public List<string> locked_dialogue_name;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<SC_Player_controller>();
        animator = GetComponent<Animator>();


        for (int i = 0; i < locked_dialogue_name.Count; i++) 
        {

            if(PlayerPrefs.GetInt(locked_dialogue_name[i]) == 0)
            {
                Transform trs = transform.Find(locked_dialogue_name[i]);
                foreach (Transform child in trs)
                {
                    if (child.gameObject.name.Contains("Dialogue") && !child.gameObject.name.Contains(locked_dialogue_name[i]))
                    {
                        dialogues.Add(child.gameObject);
                    }
                }
            }
            else
            {

                foreach (Transform child in transform)
                {
                    if (child.gameObject.name.Contains("Dialogue") && !child.gameObject.name.Contains(locked_dialogue_name[i]))
                    {
                        dialogues.Add(child.gameObject);
                    }
                }
            }
      
        }



        point_to_talk = transform.Find("Talk_zone");
        arrow = transform.Find("Arrow").GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if(collided)
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
        I_dialogue = Instantiate(dialogues[index]);
        I_dialogue.SetActive(true);
        I_dialogue.GetComponent<SC_Dialogue_system>().npc = this;
        I_dialogue.GetComponent<SC_Dialogue_system>().npc_anim = animator;
        player.can_act = false; 
        if (index < dialogues.Count - 1)
        {
            index++;
        }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collided = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collided = false;
        }
    }
}
