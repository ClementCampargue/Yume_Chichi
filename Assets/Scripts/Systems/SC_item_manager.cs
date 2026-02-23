using System.Collections.Generic;
using UnityEngine;

public class SC_item_manager : MonoBehaviour
{

    public static SC_item_manager instance;

    public List<SO_Item> items;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void add_item(SO_Item item)
    {
        items.Add(item);
    }
}
