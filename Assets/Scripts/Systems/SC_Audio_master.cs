using System;
using System.Collections.Generic;
using UnityEngine;


public class SC_Audio_master : MonoBehaviour
{

    public List<AudioSource> music_player;
    void Start()
    {
        for (int i = 0; i < music_player.Count; i++) 
        {
            if (music_player[i].volume < 1)
            {
                Debug.Log(music_player[i].volume);
                music_player[i].volume += Time.deltaTime;
            }
        }
    }
    public void add_clip(AudioClip audio , int index)
    {
        if(music_player[index].clip != audio)
        {
            music_player[index].clip = audio;
            music_player[index].Play();
            music_player[index].volume = 0;

        }
    }
}
