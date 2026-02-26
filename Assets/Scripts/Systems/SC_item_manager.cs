using System.Collections.Generic;
using UnityEngine;

public class SC_item_manager : MonoBehaviour
{

    public static SC_item_manager instance;

    public List<SO_Item> items = new List<SO_Item>();
    public List<int> items_amount = new List<int>();

    void Awake()
    {
        instance = this;
    }

    // Adds an item to the inventory, stacking if already present
    public void AddItem(SO_Item item, int amount)
    {
        int index = items.IndexOf(item);

        if (index >= 0)
        {
            // Item already exists, just increase the amount
            items_amount[index] += amount;
        }
        else
        {
            // New item, add to the lists
            items.Add(item);
            items_amount.Add(amount);
        }
    }
}