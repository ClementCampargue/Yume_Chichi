using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_scene_master : MonoBehaviour
{
    public List<AudioClip> Music_and_ambiances;
    public bool Cam_horizontal_movement = true;
    public bool Cam_vertical_movement = true;
    private SC_Player_follow cam_follow;
    public Transform Camera_target;

    private Transform player;
    public GameObject vignette_start;
    public GameObject game_master;
    public float camera_size;
    void Start()
    {
        if (GameObject.Find("Player") == null)
        {
            Instantiate(game_master);
        }
        PlayerPrefs.SetString("Scene" + PlayerPrefs.GetInt("Save"), SceneManager.GetActiveScene().name );

        Instantiate(vignette_start);
        for (int i = 0; i < Music_and_ambiances.Count; i++)
        {
            GameObject.Find("AUDIO_MASTER").GetComponent<SC_Audio_master>().add_clip(Music_and_ambiances[i], i);
        }
        cam_follow =GameObject.Find("MAIN_CAMERA").GetComponent<SC_Player_follow>();
        player = GameObject.Find("Player").transform;
        
    }

    private void Update()
    {
        if(cam_follow == null)
        {
            cam_follow = GameObject.Find("MAIN_CAMERA").GetComponent<SC_Player_follow>();

        }
        cam_follow.target = Camera_target;


        if (Cam_vertical_movement)
        {
            Camera_target.position = new Vector2(Camera_target.position.x, player.position.y);
        }
        else
        {
            Camera_target.position = new Vector2(Camera_target.position.x, Camera_target.position.y);
        }
        if (Cam_horizontal_movement)
        {
            Camera_target.position = new Vector2(player.position.x, Camera_target.position.y);
        }
        else
        {
            Camera_target.position = new Vector2(Camera_target.position.x, Camera_target.position.y);
        }
    }
}
