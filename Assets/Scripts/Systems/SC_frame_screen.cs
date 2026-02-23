using UnityEngine;

public class SC_frame_screen : MonoBehaviour
{
    public GameObject frame;
    private Animator frame_anim;

    public bool open_only_once;
    void Start()
    {
        frame_anim = frame.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void open_frame()
    {
        if (frame_anim.enabled)
        {
            frame_anim.SetTrigger("open");
        }
        else
        {
            frame_anim.enabled = true;
        }
    }

    public void Close_frame()
    {
        frame_anim.SetTrigger("close");
        if (open_only_once)
        {
            Destroy(this);
        }
    }

}
