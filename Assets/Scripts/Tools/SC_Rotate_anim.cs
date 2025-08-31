using UnityEngine;
using UnityEngine.UI;

public class SC_Rotate_anim : MonoBehaviour
{

    public float rotate_speed;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotate_speed * Time.deltaTime);
    }
}
