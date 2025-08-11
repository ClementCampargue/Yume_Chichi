using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Dialogue_system : MonoBehaviour
{
    public SC_NPC npc;
    public TextMeshProUGUI text_component;
    private Transform camtarget;
    public string[] lines;
    public GameObject[] portraits;
    public float speed;
    public float speed_pause;
    public int index;
    private S_Player_controller player;
    public Animator anim;

    public GameObject cursor;

    private string check;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<S_Player_controller>();
        text_component.text = string.Empty;
        camtarget = GameObject.Find("Cam_target").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (text_component.text == check)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text_component.text = check;
            }
        }

        if (text_component.text == check)
        {
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
            if (index > 0)
            {
                portraits[index - 1].SetActive(false);
            }
            portraits[index].SetActive(true);

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
        check =lines[index];

        check = check.Replace("§", string.Empty);


        Debug.Log(check);
        foreach (char c in lines[index].ToCharArray())
        {
            
            text_component.text += c;
            if (text_component.text.EndsWith("§"))
            {
                text_component.text = text_component.text.Substring(0, text_component.text.Length - 1);
                yield return new WaitForSeconds(speed_pause);
            }
            else
            {
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
        npc.Reset_NPC();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
