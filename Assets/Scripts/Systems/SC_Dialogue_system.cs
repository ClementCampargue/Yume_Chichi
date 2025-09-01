using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SC_Dialogue_system : MonoBehaviour
{
     public int choice_index;
    public bool choice;
    [HideInInspector]  public SC_NPC npc;
    [HideInInspector]  public Animator npc_anim;
    [HideInInspector]  public SC_interactive_object interactive;
    [HideInInspector] public TextMeshProUGUI text_component;
    [HideInInspector] public TextMeshProUGUI character_component;
    [HideInInspector] public Transform camtarget;
    public string[] character_names;
    public string[] lines;
    public Sprite[] portraits;
    public AudioClip[] talk_sound;
    public AnimationClip[] anims;
    public float speed;
    public float speed_pause;
    public float virgule_time;
    public float point_time;
    public float exclamation_point_time;
    public float interrogation_point_time;
    private int index;
    [HideInInspector] public SC_Player_controller player;
    [HideInInspector] public Animator anim;

    [HideInInspector] public GameObject cursor;

    private string check; 
    private Image prt_spr;

    private AudioSource talk_sfx;
    private AudioSource confirm_sfx;

    private List<GameObject> choices;
    private GameObject choices_obj;

    private bool child_dialogue;
    private bool typing;

    private bool last_dialoguie;

    void Start()
    {
        Start_dialogue();

        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("Choice"))
            {
                choices.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !typing)
        {

            if (index < lines.Length - 1)
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
                    Invoke("delay_input", 0.5f);

                }
            }
            else
            {
                if (!choice)
                {
                    End_dialogue();
                }
            }
        }

        if (text_component.text == check)
        {
            if (choice && index == lines.Length - 1)
            {
                choices_obj.SetActive(true);
            }
            typing = false;

            talk_sfx.Stop();
            cursor.SetActive(true);
        }
        else
        {
            cursor.SetActive(false);
        }

    }

    void delay_input()
    {
        typing = false;
    }

    void NextLine()
    {
        cursor.SetActive(false);
        if (index < portraits.Length)
        {
            Debug.Log(portraits[index]);
            if (portraits[index].name == string.Empty)
            {
                anim.SetTrigger("Next");
            }
            else
            {
                anim.SetTrigger("Next");
                // anim.SetTrigger("Next_nocharacter");
            }
        }
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
        typing = true;
        if (anims[index] != null)
        {
            npc_anim.Play(anims[index].name);
        }
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
            else if (text_component.text.EndsWith(","))
            {
                talk_sfx.Stop();
                text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                yield return new WaitForSeconds(virgule_time);
            }
            else if (text_component.text.EndsWith("."))
            {
                talk_sfx.Stop();
                text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                yield return new WaitForSeconds(point_time);
            }
            else if (text_component.text.EndsWith("?"))
            {
                talk_sfx.Stop();
                text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                yield return new WaitForSeconds(interrogation_point_time);
            }
            else if (text_component.text.EndsWith("!"))
            {
                talk_sfx.Stop();
                text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                yield return new WaitForSeconds(exclamation_point_time);
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
        index = 0;

        player = GameObject.Find("Player").GetComponent<SC_Player_controller>();
        camtarget = GameObject.Find("Cam_target").GetComponent<Transform>();

        if (transform.parent.name.Contains("Dialogue"))
        {
            if (!child_dialogue)
            {
                anim = transform.parent.GetComponent<Animator>();
            }
        }
        else
        {
            if (!child_dialogue)
            {
                anim = GetComponent<Animator>();
            }
            prt_spr = transform.Find("Pivot").transform.Find("Portrait").GetComponent<Image>();
            text_component = transform.Find("Pivot").transform.Find("Text").GetComponent<TextMeshProUGUI>();
            character_component = transform.Find("Pivot").transform.Find("Character_name").GetComponent<TextMeshProUGUI>();
            confirm_sfx = transform.Find("Audio").transform.Find("Confirm_sound").GetComponent<AudioSource>();
            talk_sfx = transform.Find("Audio").transform.Find("Talk_sound").GetComponent<AudioSource>();
            cursor = transform.Find("Pivot").transform.Find("Cursor").gameObject;
            choices_obj = transform.Find("Pivot").transform.Find("Buttons").gameObject;
        }




        if (anims[index] != null)
        { 
            npc_anim.Play(anims[index].name);
        }
        if (portraits.Length > 1)
        {
            prt_spr.sprite = portraits[index];
        }
        talk_sfx.clip = talk_sound[index];
        character_component.text = character_names[index];

        camtarget.position = transform.position;
    }

    public void start_typing()
    {
        text_component.text = string.Empty;
        StartCoroutine(typeline());

    }
    public void End_dialogue()
    {
        last_dialoguie = true;

        foreach (Transform child in transform)
        {
            if (child.name.Contains("Dialogue"))
            {
                last_dialoguie = false;
                text_component.text = string.Empty;
                child.GetComponent<SC_Dialogue_system>().child_dialogue = true;
                child.GetComponent<SC_Dialogue_system>().npc_anim = npc_anim;
                child.GetComponent<SC_Dialogue_system>().anim = anim;
                child.GetComponent<SC_Dialogue_system>().prt_spr = prt_spr;
                child.GetComponent<SC_Dialogue_system>().text_component = text_component;
                child.GetComponent<SC_Dialogue_system>().character_component = character_component;
                child.GetComponent<SC_Dialogue_system>().confirm_sfx = confirm_sfx;
                child.GetComponent<SC_Dialogue_system>().talk_sfx = talk_sfx;
                child.GetComponent<SC_Dialogue_system>().cursor = cursor;
                child.gameObject.SetActive(true);
                if (child.GetComponent<SC_Dialogue_system>().portraits[0] != null)
                {
                    anim.SetTrigger("Next");
                }
                else
                {
                    anim.SetTrigger("Next_nocharacter");
                }
                this.enabled = false;

            }
        }
        if (last_dialoguie)
        {
            index = 0;
            player.can_act = true;
            anim.SetTrigger("disable");

        }

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
        Destroy(anim.gameObject);
    }


    public void choosen()
    {
        anim.SetTrigger("Next");

        Destroy(choices_obj);
        Invoke("choosen_delay", 0.5f);
        text_component.text = string.Empty;
    }

    void choosen_delay()
    {
        choices[choice_index].GetComponent<SC_Dialogue_system>().child_dialogue = true;
        choices[choice_index].GetComponent<SC_Dialogue_system>().npc_anim = npc_anim;
        choices[choice_index].GetComponent<SC_Dialogue_system>().anim = anim;
        choices[choice_index].GetComponent<SC_Dialogue_system>().prt_spr = prt_spr;
        choices[choice_index].GetComponent<SC_Dialogue_system>().text_component = text_component;
        choices[choice_index].GetComponent<SC_Dialogue_system>().character_component = character_component;
        choices[choice_index].GetComponent<SC_Dialogue_system>().confirm_sfx = confirm_sfx;
        choices[choice_index].GetComponent<SC_Dialogue_system>().talk_sfx = talk_sfx;
        choices[choice_index].GetComponent<SC_Dialogue_system>().cursor = cursor;
        choices[choice_index].SetActive(true);
        this.enabled = false;
    }
}
