using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_Title_screen : MonoBehaviour
{

    public GameObject game_master;
    public Animator anim;
    [HideInInspector]public int saveslot;
    void Start()
    {
        PlayerPrefs.SetInt("Respawn", 1);
    }
    public void show_settings()
    {
        anim.SetTrigger("Settings_on");
    }
    public void hide_settings()
    {
        anim.SetTrigger("Settings_off");
    }

    public void show_load()
    {
        anim.SetTrigger("Load_on");
    }
    public void hide_load()
    {
        anim.SetTrigger("Load_off");
    }

    public void load_scene()
    {
        anim.SetTrigger("Fade_load");
        Invoke("Delay_load", 1f);
    }
    void Delay_load()
    {
        Instantiate(game_master); 
        PlayerPrefs.SetInt("Save", saveslot);
        if(!PlayerPrefs.HasKey("Scene" + PlayerPrefs.GetInt("Save")))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("Scene" + PlayerPrefs.GetInt("Save")));
        }
    }
    public void Quit_game()
    {
        anim.SetTrigger("Fade_quit");
        Invoke("Delay_Quit_game", 1f);

    }
    public void Delay_Quit_game()
    {
        Application.Quit();
    }
}
