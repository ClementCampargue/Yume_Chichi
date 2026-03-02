using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_scene_loader : MonoBehaviour
{
    public string scene_name;
    public string teleporter_tag;
    public Transform spawn_point;
    [HideInInspector] public string current_scene;
    public GameObject fade_obj;
    public bool teleporter;
    public SC_scene_loader teleporter_point;
    private bool cancollide =true;
    private SC_Player_controller player;
    private SC_game_master master;
    void Start()
    {
        player = SC_Player_controller.instance;
        master = SC_game_master.instance;
        if (master.previous_scene == scene_name && PlayerPrefs.GetString("Scene_teleport") == teleporter_tag)
        {
            spawn_player();
        }
        current_scene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("Scene_teleport", teleporter_tag);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && cancollide)
        {
            Invoke("delay_load", 1f);
            cancollide = false;
            player.can_act =false;
            Instantiate(fade_obj);
        }
    }

    public void spawn_player()
    {
        Invoke("delay_collide", 1f);
        cancollide = false;
        player.transform.position = spawn_point.position;
        player.can_act = false;
    }

    void delay_collide()
    {
        cancollide = true;
        player.can_act = true;

    }

    void delay_load()
    {
        player.can_act = true;
        master.previous_scene = current_scene;
        if (teleporter)
        {
            teleporter_point.spawn_player();
        }
        else
        {
            SceneManager.LoadScene(scene_name);
        }

    }
}
