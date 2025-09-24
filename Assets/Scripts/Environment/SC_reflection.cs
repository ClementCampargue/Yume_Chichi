using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_reflection : MonoBehaviour
{
    public Vector2 decalage;
    private Transform _camera;
    private Material material;
    void Start()
    {
        _camera = Camera.main.transform;
        material = GetComponent<SpriteRenderer>().material;
    }


    void Update()
    {
        material.SetVector("_Offset",new Vector3(-_camera.transform.position.x  + transform.position.x * decalage.x, -_camera.transform.position.y + transform.position.y * decalage.y));

    }
}
