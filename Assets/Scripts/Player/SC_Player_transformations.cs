
using System.Collections.Generic;
using UnityEngine;

public class SC_Player_transformations : MonoBehaviour
{
    public SC_Player_controller controller;
    public List<GameObject> transformations;


    public void transform_(int index)
    {
        foreach (GameObject gb in transformations) 
        { 
            gb.SetActive(false);
        }
        transformations[index].SetActive(true);
        controller.animator = transformations[index].GetComponent<Animator>();
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
