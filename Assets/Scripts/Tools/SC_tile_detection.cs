using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SC_tile_detection : MonoBehaviour
{
    public string tag;
    public AudioSource footsteps;
    public ParticleSystem FX;

    public SC_footstep_master master;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (master.currenttag == tag && rb.linearVelocity != Vector2.zero)
        { 
            in_();
        }
        else
        {
            out_();
        }

        if(rb.linearVelocity == Vector2.zero)
        {
            out_();
        }


    }

    void in_()
    {
        if (!footsteps.isPlaying)
        {
            footsteps.Play();
        }
        if (!FX.isPlaying)
        {
            FX.Play();
        }
    }

    void out_()
    {
        footsteps.Stop();
        FX.Stop();
    }


}
