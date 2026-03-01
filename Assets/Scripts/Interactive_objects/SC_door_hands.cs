using System.Collections.Generic;
using UnityEngine;

public class SC_door_hands : MonoBehaviour
{

    public List<SC_trigger_anim> leviers;
    private SC_trigger_cutscene cutscene;
    private bool open;
    public Animator anim;
    void Start()
    {
        cutscene = GetComponent<SC_trigger_cutscene>();
    }

    // Update is called once per frame
    void Update()
    {
        open = true;
        for (int i = 0; i < leviers.Count; i++)
        {
            if (!leviers[i].interacted)
            {
                open = false;
            }
        }
        if (open)
        {
            anim.enabled = true;
            cutscene.start_cutscene();
            this.enabled = false;
        }
    }
}
