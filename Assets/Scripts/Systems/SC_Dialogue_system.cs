using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SC_Dialogue_system : MonoBehaviour
{
    [HideInInspector]  public SC_NPC npc;
    [HideInInspector]  public Animator npc_anim;
    [HideInInspector]  public SC_interactive_object interactive;
    private TextMeshProUGUI text_component;
    private TextMeshProUGUI character_component;
    private Transform camtarget;
    public string[] character_names;
    public string[] lines;
    public Sprite[] portraits;
    public AudioClip[] talk_sound;
    public AnimationClip[] anims;
    public float speed;
    public float speed_pause;
    private int index;
    private SC_Player_controller player;
    private Animator anim;

    private GameObject cursor;

    private string check;
    private Image prt_spr;

    private AudioSource talk_sfx;
    private AudioSource confirm_sfx;

    void Start()
    {
        index = 0;

        player = GameObject.Find("Player").GetComponent<SC_Player_controller>();
        anim = GetComponent<Animator>();
        camtarget = GameObject.Find("Cam_target").GetComponent<Transform>();
        prt_spr = transform.Find("Pivot").transform.Find("Portrait").GetComponent<Image>();
        text_component = transform.Find("Pivot").transform.Find("Dialogue_text").GetComponent<TextMeshProUGUI>();
        character_component = transform.Find("Pivot").transform.Find("Character_name").GetComponent<TextMeshProUGUI>();

        confirm_sfx = transform.Find("Audio").transform.Find("Confirm_sound").GetComponent<AudioSource>();
        talk_sfx = transform.Find("Audio").transform.Find("Talk_sound").GetComponent<AudioSource>();
        cursor = transform.Find("Pivot").transform.Find("Cursor").gameObject;
        text_component.text = string.Empty;

        if (anims[index] != null)
        {
            npc_anim.Play(anims[index].name);
        }
        prt_spr.sprite = portraits[index];
        talk_sfx.clip = talk_sound[index];
        character_component.text = character_names[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (text_component.text == check)
            {
                NextLine();
                confirm_sfx.Play();

            }
            else
            {
                StopAllCoroutines();
                text_component.text = check;
            }
        }

        if (text_component.text == check)
        {
            talk_sfx.Stop();
            cursor.SetActive(true);
        }
        else
        {
            cursor.SetActive(false);
        }

    }

    void NextLine()
    {
        cursor.SetActive(false);

        if (index < lines.Length - 1)
        {
            index++;
            text_component.text = string.Empty;
            StartCoroutine(typeline());
        }
        else
        {
            End_dialogue();
        }
    }

    IEnumerator typeline()
    {
        if(anims[index] != null)
        {
            npc_anim.Play(anims[index].name);
        }
        prt_spr.sprite = portraits[index];
        talk_sfx.clip = talk_sound[index];
        character_component.text = character_names[index];
        check =lines[index];

        check = check.Replace("§", string.Empty);

        foreach (char c in lines[index].ToCharArray())
        {


            text_component.text += c;
            if (text_component.text.EndsWith("§"))
            {
                talk_sfx.Stop();
                text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                yield return new WaitForSeconds(speed_pause);
            }
            else
            {
                if (!talk_sfx.isPlaying)
                {
                    talk_sfx.Play();
                }
                yield return new WaitForSeconds(speed);
            }

            
        }
    }

    public void Start_dialogue()
    {
        camtarget.position = transform.position;
        StartCoroutine(typeline());
    }

    public void End_dialogue()
    {
        anim.SetTrigger("disable");
        player.can_act = true;

    }

    public void Destroy_dialogue()
    {
        if(npc != null)
        {
            npc.Reset_NPC();
        }
        if(interactive != null)
        {
            interactive.Reset();
        }
        Destroy(gameObject);
    }
}
