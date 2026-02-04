using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SC_Pause_menu : MonoBehaviour
{
    public List<GameObject> pages;
    public List<string> pages_name;
    public List<Animator> pages_header_anims;
    public int page_index;
    public bool opened;
    private Animator canvas_anim;
    public TextMeshProUGUI text;
    void Start()
    {
        canvas_anim = GetComponent<Animator>();
        canvas_anim.SetTrigger("close");

    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            check_input(0);
        }
        if (Input.GetButtonDown("Map"))
        {
            check_input(2);
        }
        if (Input.GetButtonDown("LeftTrigger"))
        {
            previous_page();
        }
        if (Input.GetButtonDown("RightTrigger"))
        {
            next_page();
        }
    }

    void check_input(int index)
    {
        page_index = index;
        if (opened)
        {
            Time.timeScale = 1;
            canvas_anim.SetTrigger("close");
        }
        else
        {
            Time.timeScale = 0;

            if (canvas_anim.enabled == false)
            {
                canvas_anim.enabled = true;
            }
            else
            {
                canvas_anim.SetTrigger("open");
            }
            update_page();
        }
        opened = !opened;
    }

    public void previous_page()
    {
        if (page_index == 0)
        {
            page_index = pages.Count - 1;
        }
        else
        {
            page_index--;
        }

        update_page();
    }

    public void next_page()
    {
        if (page_index == pages.Count - 1)
        {
            page_index = 0;
        }
        else
        {
            page_index++;
        }

        update_page(); 
    }
    public void update_page()
    {
        text.text = pages_name[page_index];

        foreach (GameObject page in pages) 
        {
            page.SetActive(false);
        }
        pages[page_index].SetActive(true);

        foreach (Animator page in pages_header_anims) 
        {
            page.SetTrigger("unselected");
        }
        if(pages_header_anims[page_index].enabled == false)
        {
            pages_header_anims[page_index].ResetTrigger("unselected");
            pages_header_anims[page_index].enabled =true;
        }
        else
        {
            pages_header_anims[page_index].ResetTrigger("unselected");
            pages_header_anims[page_index].SetTrigger("selected");
        }
    }
    public void close()
    {
        Time.timeScale = 1;
        canvas_anim.SetTrigger("close");
        opened = !opened;
    }
}
