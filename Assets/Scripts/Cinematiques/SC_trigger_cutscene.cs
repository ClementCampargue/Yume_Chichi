using UnityEngine;

public class SC_trigger_cutscene : MonoBehaviour
{
    private SC_Player_controller controller;
    private SC_camera_controller cam_controller;
    public float time_before_can_act;

    private bool cutscene_on;
    public bool trigger_multiple_times;
    public Transform target_cam;
    void Start()
    {
        controller = SC_Player_controller.instance;
        cam_controller = SC_camera_controller.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !cutscene_on)
        {
            start_cutscene();
        }
    }



    public void start_cutscene()
    {
        cutscene_on = true;
        controller.disable_controller();
        if(time_before_can_act != 0)
        {
            Invoke("end_cutscene", time_before_can_act);
        }
        cam_controller.start_cutscene(target_cam);
    }

    public void end_cutscene()
    {
        controller.enable_controller();
        cam_controller.end_cutscene();

        if (trigger_multiple_times)
        {
            cutscene_on = false;
        }
        else
        {
            this.enabled = false;
        }
    }
}
