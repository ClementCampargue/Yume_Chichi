using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SC_Dialogue_system : MonoBehaviour
{
    public int choice_index;
    public bool choice;
    [HideInInspector] public SC_NPC npc;
    [HideInInspector] public Animator npc_anim;
    [HideInInspector] public SC_interactive_object interactive;
    [HideInInspector] public TextMeshProUGUI text_component;
    [HideInInspector] public TextMeshProUGUI character_component;
    [HideInInspector] public Transform camtarget;
    public string[] character_names;
    public string[] lines;
    public List<Sprite> portraits = new List<Sprite>();
    public AudioClip[] talk_sound;
    public AnimationClip[] anims;
    public float speed;
    public float speed_pause_;
    public float speed_pause__;
    public float speed_pause___;

    public int index;
    [HideInInspector] public SC_Player_controller player;
    [HideInInspector] public Animator anim;

    [HideInInspector] public Animator cursor;

    private string check;
    private Image prt_spr;

    private AudioSource talk_sfx;
    private AudioSource confirm_sfx;

    private List<GameObject> choices = new List<GameObject>();
    private GameObject choices_obj;

    private bool child_dialogue;
    private bool typing;

    private bool last_dialoguie;
    private bool end;

    public SC_text_effect_master test;

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

    void Update()
    {
        if(text_component.text == string.Empty)
        {
            test.reset_();
        }

        string text2 = lines[index].Replace("\n", "").Replace("\r", "").Replace(test.color_character_effect_start, "").Replace(test.color_character_effect_end, "").Replace(test.scale_character_effect_start, "").Replace(test.scale_character_effect_end, "").Replace(test.ondulation_character_effect_start, "").Replace(test.ondulation_character_effect_end, "").Replace(test.movement_character_effect_start, "").Replace(test.movement_character_effect_end, "").Replace("µ", "").Replace("£", "").Replace("%", "");
        string text = text_component.text.Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace(test.color_character_effect_start, "").Replace(test.color_character_effect_end, "").Replace(test.scale_character_effect_start, "").Replace(test.scale_character_effect_end, "").Replace(test.ondulation_character_effect_start, "").Replace(test.ondulation_character_effect_end, "").Replace(test.movement_character_effect_start, "").Replace(test.movement_character_effect_end, "").Replace("µ", "").Replace("£", "").Replace("%", "");
        check = check.Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace(test.color_character_effect_start, "").Replace(test.color_character_effect_end, "").Replace(test.scale_character_effect_start, "").Replace(test.scale_character_effect_end, "").Replace(test.ondulation_character_effect_start, "").Replace(test.ondulation_character_effect_end, "").Replace(test.movement_character_effect_start, "").Replace(test.movement_character_effect_end, "").Replace("µ", "").Replace("£", "").Replace("%", "");


        if (Input.GetButtonDown("Fire1"))
        {
            if (index < lines.Length - 1)
            {

                if (text == check)
                {
                    Invoke("update_visuals", 0.1f);
                    confirm_sfx.Play();
                    index++;
                    if (index < lines.Length)
                    {
                        if (character_names[index - 1] != character_names[index])
                        {
                            anim.ResetTrigger("no_character_start");
                            if (portraits[index].ToString() != "null")
                            {
                                anim.SetTrigger("Next");
                            }
                            else
                            {
                                anim.SetTrigger("Next_nocharacter");
                            }
                        }
                        test.reset_();

                    }
                    else
                    {

                        if (portraits[index].name != string.Empty)
                        {
                            anim.SetTrigger("Next");
                        }
                        else
                        {
                            anim.SetTrigger("Next_nocharacter");
                        }
                    }
                    cursor.SetBool("On", false);

                }
                else if (typing)
                {
                    if (cursor.enabled)
                    {
                        cursor.SetBool("On", true);
                    }
                    else
                    {
                        cursor.SetBool("On", true);
                        cursor.enabled = true;
                    }

                    StopAllCoroutines();

                    text_component.text = text2;
                    check_effect();

                    Invoke("delay_input", 0.2f);
                }
            }
            else if (!end)
            {
                if (text == check)
                {

                    end = true;
                    if (!choice)
                    {
                        End_dialogue();
                    }
                    cursor.SetBool("On", false);
                    test.reset_();

                }
                else if (typing)
                {
                    if (cursor.enabled)
                    {
                        cursor.SetBool("On", true);
                    }
                    else
                    {
                        cursor.SetBool("On", true);
                        cursor.enabled = true;
                    }
                    check_effect();

                    StopAllCoroutines();
                    text_component.text = text2;
                    Invoke("delay_input", 0.2f);

                }
            }
        }

        if (text == check)
        {

            if (choice && index == lines.Length - 1)
            {
                choices_obj.SetActive(true);
            }

            typing = false;
            talk_sfx.Stop();

        }

    }



    void delay_input()
    {
        typing = false;
    }

    void NextLine()
    {
        typing = true;

        if (index < lines.Length)
        {
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

        if (anims[index] != null)
        {
            npc_anim.Play(anims[index].name);
        }
        check = lines[index];
        check = check.Replace("%", "");
        check = check.Replace("µ", "");
        check = check.Replace("£", "");
        check = check.Replace(test.color_character_effect_start, "");
        check = check.Replace(test.color_character_effect_end, "");
        check = check.Replace(test.movement_character_effect_start, "");
        check = check.Replace(test.movement_character_effect_end, "");
        check = check.Replace(test.ondulation_character_effect_start, "");
        check = check.Replace(test.ondulation_character_effect_end, "");
        check = check.Replace(test.scale_character_effect_start, "");
        check = check.Replace(test.scale_character_effect_start, "");

        text_component.text = string.Empty;
        string[] words = lines[index].Split(' ');
        foreach (string word in words)
        {
            string testText = text_component.text + (text_component.text.EndsWith("\n") || text_component.text == "" ? "" : " ") + word;
            text_component.ForceMeshUpdate();
            float currentWidth = text_component.GetPreferredValues(testText).x;

            if (currentWidth > text_component.rectTransform.rect.width)
            {
                text_component.text += "\n";
            }
            else if (text_component.text != "" && !text_component.text.EndsWith("\n"))
            {
                text_component.text += " ";
            }
            foreach (char c in word)
            {
                text_component.text += c;
                if (text_component.text.EndsWith("µ"))
                {
                    talk_sfx.Stop();
                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                    yield return new WaitForSeconds(speed_pause_);
                }
                else if (text_component.text.EndsWith("£"))
                {
                    talk_sfx.Stop();
                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                    yield return new WaitForSeconds(speed_pause__);
                }
                else if (text_component.text.EndsWith("%"))
                {
                    talk_sfx.Stop();
                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                    yield return new WaitForSeconds(speed_pause___);
                }
                else if (text_component.text.EndsWith(test.movement_character_effect_start))
                {
                    test.movement = true;

                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                }
                else if (text_component.text.EndsWith(test.movement_character_effect_end))
                {
                    test.movement = false;

                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                }
                else if (text_component.text.EndsWith(test.ondulation_character_effect_start))
                {
                    test.ondulation = true;

                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                }
                else if (text_component.text.EndsWith(test.ondulation_character_effect_end))
                {
                    test.ondulation = false;

                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                }
                else if (text_component.text.EndsWith(test.scale_character_effect_start))
                {
                    test.Scale = true;

                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                }
                else if (text_component.text.EndsWith(test.scale_character_effect_end))
                {
                    test.Scale = false;

                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                }
                else if (text_component.text.EndsWith(test.color_character_effect_start))
                {
                    test.Color_ = true;

                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                }
                else if (text_component.text.EndsWith(test.color_character_effect_end))
                {
                    test.Color_ = false;

                    text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                }


                else
                {

                    if (!talk_sfx.isPlaying)
                    {
                        talk_sfx.Play();
                    }
                }
                if (test.movement)
                {
                    if (c.ToString() != "")
                    {
                        int charIndex = text_component.text.Length;

                        test.targetLetters_movement.Add(charIndex);

                        test.RefreshText();
                    }
                }
                else if (test.ondulation)
                {
                    if (c.ToString() != "")
                    {
                        int charIndex = text_component.text.Length;

                        test.targetLetters_ondulation.Add(charIndex);

                        test.RefreshText();
                    }
                }
                else if (test.Scale)
                {
                    if (c.ToString() != "")
                    {
                        int charIndex = text_component.text.Length;

                        test.targetLetters_scale.Add(charIndex);

                        test.RefreshText();
                    }
                }
                else if (test.Color_)
                {
                    if (c.ToString() != "")
                    {
                        int charIndex = text_component.text.Length;

                        test.targetLetters_color.Add(charIndex);

                        test.RefreshText();
                    }
                }
                yield return new WaitForSeconds(speed);


            }

            if (cursor.enabled)
            {
                cursor.SetBool("On", true);
            }
            else
            {
                cursor.SetBool("On", true);
                cursor.enabled = true;
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
            prt_spr = transform.Find("Portrait").GetComponent<Image>();
            text_component = transform.Find("Pivot").transform.Find("Text").GetComponent<TextMeshProUGUI>();
            character_component = transform.Find("Pivot").transform.Find("Character_name").GetComponent<TextMeshProUGUI>();
            confirm_sfx = transform.Find("Audio").transform.Find("Confirm_sound").GetComponent<AudioSource>();
            talk_sfx = transform.Find("Audio").transform.Find("Talk_sound").GetComponent<AudioSource>();
            cursor = transform.Find("Cursor_pivot").transform.Find("Cursor").GetComponent<Animator>();
            choices_obj = transform.Find("Pivot").transform.Find("Buttons").gameObject;
        }

        if (anims[index] != null)
        {
            npc_anim.Play(anims[index].name);
        }
        if (portraits.Count > 0)
        {
            prt_spr.sprite = portraits[index];
        }
        talk_sfx.clip = talk_sound[index];
        character_component.text = character_names[index];
        camtarget.position = transform.position;
        if (portraits[0].ToString() == "null")
        {
            anim.SetTrigger("no_character_start");
        }
    }
    public void start_typing()
    {
        typing = true;
        text_component.text = string.Empty;
        StartCoroutine(typeline());
    }

    public void End_dialogue()
    {

        last_dialoguie = true;
        typing = false;

        foreach (Transform child in transform)
        {
            if (child.name.Contains("Dialogue"))
            {
                last_dialoguie = false;
                text_component.text = string.Empty;
                child.GetComponent<SC_Dialogue_system>().child_dialogue = true;
                child.GetComponent<SC_text_effect_master>().textMeshPro = test.textMeshPro;
                child.GetComponent<SC_Dialogue_system>().npc_anim = npc_anim;
                child.GetComponent<SC_Dialogue_system>().anim = anim;
                child.GetComponent<SC_Dialogue_system>().prt_spr = prt_spr;
                child.GetComponent<SC_Dialogue_system>().text_component = text_component;
                child.GetComponent<SC_Dialogue_system>().character_component = character_component;
                child.GetComponent<SC_Dialogue_system>().confirm_sfx = confirm_sfx;
                child.GetComponent<SC_Dialogue_system>().talk_sfx = talk_sfx;
                child.GetComponent<SC_Dialogue_system>().cursor = cursor;
                child.gameObject.SetActive(true);
                anim.ResetTrigger("no_character_start");

                if (portraits[index].ToString() != "null")
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



            anim.ResetTrigger("no_character_start");
            player.can_act = true;
            if (portraits[index].ToString() == "null")
            {
                anim.SetTrigger("disable_no_character");
            }
            else
            {
                anim.SetTrigger("disable");
            }


        }
        if (transform.parent != null)
        {


            SC_item item = transform.parent.GetComponent<SC_item>();

            if (item != null)
            {
                item.get();
            }

        }

        if (transform.parent != null)
        {
            SC_frame_screen frame = transform.parent.GetComponent<SC_frame_screen>();

            if (frame != null)
            {
                frame.Close_frame();
            }
        }

        Destroy(test);

    }

    public void Destroy_dialogue()
    {
        if (npc != null)
        {
            npc.Reset_NPC();
        }
        if (interactive != null)
        {
            interactive.Reset();
        }
        index = 0;
        Destroy(anim.gameObject);
    }
    public void choosen()
    {
        anim.ResetTrigger("no_character_start");
        anim.SetTrigger("Next");
        Destroy(choices_obj);
        Invoke("choosen_delay", 0.5f);
        text_component.text = string.Empty;
    }
    void choosen_delay()
    {
        choices[choice_index].GetComponent<SC_text_effect_master>().textMeshPro = test.textMeshPro;
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
    public void update_visuals()
    {
        NextLine();
        if (anims[index] != null)
        {
            npc_anim.Play(anims[index].name);
        }
        if (portraits.Count > 1)
        {
            prt_spr.sprite = portraits[index];
        }
        talk_sfx.clip = talk_sound[index];
        character_component.text = character_names[index];
        camtarget.position = transform.position;
    }



    public void check_effect()
    {
        string testt = lines[index];
        test.movement = false;
        test.Scale = false;
        test.ondulation = false;
        test.Color_ = false;

        for (int i = 0; i < text_component.text.Length; i++)
        {
            if (testt[i+1].ToString() == test.movement_character_effect_start)
            {
                test.movement = true;
                testt = testt.Remove(i+1,1);

            }
            else if (testt[i].ToString() == test.movement_character_effect_end)
            {
                test.movement = false;
                testt = testt.Remove(i, 1);

            }
            else if (testt[i].ToString() == test.scale_character_effect_start)
            {
                test.Scale = true;
                testt = testt.Remove(i, 1);

            }
            else if (testt[i].ToString() == test.scale_character_effect_end)
            {
                test.Scale = false;
                testt = testt.Remove(i, 1);

            }
            else if (testt[i].ToString() == test.ondulation_character_effect_start)
            {
                test.ondulation = true;
                testt = testt.Remove(i, 1);

            }
            else if (testt[i].ToString() == test.ondulation_character_effect_end)
            {
                test.ondulation = false;
                testt = testt.Remove(i, 1);

            }
            else if (testt[i].ToString() == test.color_character_effect_start)
            {
                test.Color_ = true;
                testt = testt.Remove(i, 1);

            }
            else if (testt[i].ToString() == test.color_character_effect_end)
            {
                test.Color_ = false;
                testt = testt.Remove(i, 1);

            }
            else
            {
                Debug.Log(testt);
                if (test.movement)
                {
                    if (testt[i].ToString() != "")
                    {

                        int charIndex = i;

                        test.targetLetters_movement.Add(charIndex);

                        test.RefreshText();
                    }
                }
                else if (test.ondulation)
                {
                    if (testt[i].ToString() != "")
                    {

                        int charIndex = i;

                        test.targetLetters_ondulation.Add(charIndex);

                        test.RefreshText();
                    }
                }
                else if (test.Scale)
                {
                    if (testt[i].ToString() != "")
                    {

                        int charIndex = i;

                        test.targetLetters_scale.Add(charIndex);

                        test.RefreshText();
                    }
                }
                else if (test.Color_)
                {
                    if (testt[i].ToString() != "")
                    {

                        int charIndex = i;

                        test.targetLetters_color.Add(charIndex);

                        test.RefreshText();
                    }
                }
            }

        }
    }
}