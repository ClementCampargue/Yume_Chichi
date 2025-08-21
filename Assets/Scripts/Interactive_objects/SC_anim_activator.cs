using Unity.VisualScripting;
using UnityEngine;

public class SC_anim_activator : MonoBehaviour
{
    private SC_Player_controller player;
    public float distance;
    private bool canactivate = true;

    private SC_Sprite_animator spr_anim;
    private SC_Movement_anim mov_anim;
    private SC_Color_anim color_anim;

    public SpriteRenderer arrow;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<SC_Player_controller>();

        spr_anim =GetComponent<SC_Sprite_animator>();
        mov_anim = GetComponent<SC_Movement_anim>();
        color_anim = GetComponent<SC_Color_anim>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < distance)
        {
            if (canactivate && Input.GetButtonDown("Fire1"))
            {
                activate();
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
    void activate()
    {
        Destroy(arrow);
        if (spr_anim != null)
        {
            spr_anim.enabled = true;
        }
        if(mov_anim != null)
        {
            mov_anim.enabled = true;
        }
        if(color_anim != null)
        {
            color_anim.enabled = true;
        }
        Destroy(this);
    }
}
