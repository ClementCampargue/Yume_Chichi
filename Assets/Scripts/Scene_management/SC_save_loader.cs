using UnityEngine;

public class SC_save_loader : MonoBehaviour
{
    public SC_Title_screen title;
    public int slot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void assign_save()
    {
        title.saveslot = slot;
    }
}
