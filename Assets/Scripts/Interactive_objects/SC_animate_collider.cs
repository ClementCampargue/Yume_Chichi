using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_animate_collider : MonoBehaviour
{
    public string tag_;
    public Animator animator_;
    public float random_range_amount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == tag_)
        {
            if(random_range_amount == 0)
            {
                animator_.enabled = true;
                this.enabled = false;
            }
            else
            {
                Invoke("delay", Random.Range(0, random_range_amount));
            }
        }
    }

    void delay()
    {
        animator_.enabled = true;
        this.enabled = false;

    }
}
