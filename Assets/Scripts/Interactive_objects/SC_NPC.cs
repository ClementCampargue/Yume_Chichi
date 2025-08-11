using Unity.VisualScripting;
using UnityEngine;

public class SC_NPC : MonoBehaviour
{
    public float distance;

    private bool can_talk =true;
    private S_Player_controller player;
    public GameObject dialogue;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<S_Player_controller>();
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
                player.Display_arrow();
            }
        }
        else
        {
            player.Hide_arrow();
        }
    }

    void Start_dialogue()
    {
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player.Hide_arrow();
        GameObject I_dialogue;
        I_dialogue = Instantiate(dialogue);
        I_dialogue.GetComponent<Dialogue_system>().npc = this;
        player.can_act = false;
    }

    public void Reset_NPC()
    {
        can_talk = true;

        player.Display_arrow();
    }
}
