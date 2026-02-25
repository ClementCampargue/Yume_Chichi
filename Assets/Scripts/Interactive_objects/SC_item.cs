using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class SC_item : MonoBehaviour
{
    public GameObject visual_to_disable_on_pickup;
    private SC_item_manager sC_Item_Manager;
    public SO_Item item;
    private bool got;
    void Start()
    {
        sC_Item_Manager = SC_item_manager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void get()
    {
        if (!got)
        {
            visual_to_disable_on_pickup.SetActive(false);
            sC_Item_Manager.add_item(item);

            got = true;
        }
    }
}
