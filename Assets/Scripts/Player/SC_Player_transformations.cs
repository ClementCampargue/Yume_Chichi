
using System.Collections.Generic;
using UnityEngine;

public class SC_Player_transformations : MonoBehaviour
{
    public SC_Player_controller controller;
    public List<GameObject> transformations;

    private int currentIndex = 0;

    public void transform_(int index)
    {
        if (transformations == null || transformations.Count == 0)
            return;

        if (index < 0 || index >= transformations.Count)
            return;

        foreach (GameObject gb in transformations)
        {
            gb.SetActive(false);
        }

        transformations[index].SetActive(true);
        controller.animator = transformations[index].GetComponent<Animator>();
        currentIndex = index;

        Debug.Log("Transformation active : " + transformations[index].name);
    }


    void Start()
    {
        
    }

    void Update()
    {
        if (transformations == null || transformations.Count == 0)
            return;

        // Transformation précédente
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int newIndex = currentIndex - 1;
            if (newIndex < 0)
                newIndex = transformations.Count - 1;

            transform_(newIndex);
        }

        // Transformation suivante
        if (Input.GetKeyDown(KeyCode.E))
        {
            int newIndex = currentIndex + 1;
            if (newIndex >= transformations.Count)
                newIndex = 0;

            transform_(newIndex);
        }
    }
}
