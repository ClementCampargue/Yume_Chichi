using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SC_Scene_music : MonoBehaviour
{
    public List<AudioClip> Music_and_ambiances;

    void Start()
    {
        for (int i = 0; i < Music_and_ambiances.Count; i++)
        {
            GameObject.Find("AUDIO_MASTER").GetComponent<SC_Audio_master>().add_clip(Music_and_ambiances[i], i);
        }
    }

}
