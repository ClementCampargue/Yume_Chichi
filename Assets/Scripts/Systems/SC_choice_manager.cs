using UnityEngine;

public class SC_choice_manager : MonoBehaviour
{
    public SC_Dialogue_system dialogue;
    public int index;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void choose()
    {
        dialogue.choice_index = index;
        dialogue.choosen();
    }
}
