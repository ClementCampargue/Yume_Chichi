using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SC_item_required_trigger : MonoBehaviour
{
    [Header("Requirements")]
    public List<SO_Item> requiredItems = new List<SO_Item>();
    public List<int> requiredQuantities = new List<int>();

    [Header("Dialogues")]
    public GameObject base_dialogue;
    public GameObject unlocked_dialogue;

    private SC_item_manager manager;

    private bool unlocked;

    void Start()
    {
        manager = SC_item_manager.instance;
    }

    void Update()
    {
        CheckRequirements();
    }

    public void CheckRequirements()
    {
        unlocked = true; // use the class-level variable, not a new local one

        for (int i = 0; i < requiredItems.Count; i++)
        {
            bool hasRequiredItem = false;

            for (int y = 0; y < manager.items.Count; y++)
            {
                if (manager.items[y] == requiredItems[i])
                {
                    hasRequiredItem = true;

                    // If player has less than required, unlock should be false
                    if (manager.items_amount[y] < requiredQuantities[i])
                    {
                        unlocked = false;
                        break;
                    }
                }
            }

            if (!hasRequiredItem)
            {
                unlocked = false; // player doesn't even have the item
                break;
            }
        }

        // Update dialogues based on unlocked state
        base_dialogue.SetActive(!unlocked);
        unlocked_dialogue.SetActive(unlocked);
    }
}