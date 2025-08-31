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
    public float virgule_time;
    public float point_time;
    public float exclamation_point_time;
    public float interrogation_point_time;
    private int index;
    private SC_Player_controller player;
    private Animator anim;

    private GameObject cursor;

    private string check;
    private Image prt_spr;
    private Image prt_spr2;

    private AudioSource talk_sfx;
    private AudioSource confirm_sfx;

    private bool portrait_2;

    void Start()
    {
        index = 0;

        player = GameObject.Find("Player").GetComponent<SC_Player_controller>();
        anim = GetComponent<Animator>();
        camtarget = GameObject.Find("Cam_target").GetComponent<Transform>();
        prt_spr = transform.Find("Pivot").transform.Find("Portrait").GetComponent<Image>();
        prt_spr2 = transform.Find("Pivot").transform.Find("Portrait_").GetComponent<Image>();
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
        if (portraits.Length > 1)
        {
            prt_spr.sprite = portraits[index];
        }
        prt_spr2.sprite = portraits[index +1];
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
                if(portraits.Length > 1 && index < lines.Length - 1)
                {
                    if (!portrait_2)
                    {
                        if (prt_spr.sprite == portraits[index + 1])
                        {
                            NextLine();
                        }
                        else
                        {
                            anim.SetTrigger("Next");
                        }

                    }
                    else
                    {
                        if (prt_spr2.sprite == portraits[index + 1])
                        {
                            NextLine();
                        }
                        else
                        {
                            anim.SetTrigger("Next2");
                        }
                        Debug.Log("2");

                    }

                }
                else
                {
                    NextLine();
                }

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
        camtarget.position = transform.position;
        StartCoroutine(typeline());
    }

    public void End_dialogue()
    {
        if (portrait_2)
        {
            anim.SetTrigger("disable2");
        }
        else
        {
            anim.SetTrigger("disable");
        }

        player.can_act = true;
        index = 0;

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

    public void update_portraits()
    {
        if(index < lines.Length - 1)
        {

            text_component.text = string.Empty;
            talk_sfx.clip = talk_sound[index + 1];
            character_component.text = character_names[index + 1];

            portrait_2 = !portrait_2;
            if (!portrait_2)
            {
                prt_spr2.sprite = portraits[index];
                if (index < portraits.Length - 1)
                {
                    prt_spr.sprite = portraits[index + 1];
                }
            }
            else
            {
                prt_spr.sprite = portraits[index];
                if (index < portraits.Length - 1)
                {
                    prt_spr2.sprite = portraits[index + 1];
                }
            }
        }
        

    }
}
