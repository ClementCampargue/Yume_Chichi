using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_scene_loader : MonoBehaviour
{
    public string scene_name;
    public Transform spawn_point;
    [HideInInspector] public string current_scene;
    public SC_Color_anim fade;
    void Start()
    {
        if(GameObject.Find("GAME_MASTER").GetComponent<SC_game_master>().previous_scene == scene_name)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = spawn_point.position;
        }
        current_scene = SceneManager.GetActiveScene().name;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Invoke("delay_load", 1f);

            GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Player_controller>().can_act =false;
            fade.enabled = true;
        }
    }

    void delay_load()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Player_controller>().can_act = true;
        GameObject.Find("GAME_MASTER").GetComponent<SC_game_master>().previous_scene = current_scene;
        SceneManager.LoadScene(scene_name);

    }
}
