using UnityEngine;

public class SC_trigger_anim : MonoBehaviour
{
    public bool interacted;
    private Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void trigger()
    {
        interacted = true;
       if(anim!=null) anim.enabled = true;
    }
}
