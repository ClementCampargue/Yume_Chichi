using UnityEngine;

public class SC_lookat : MonoBehaviour
{
    public Transform aim;
    public Transform pupil;
    public Transform center;

    public float radius = 0.2f; 


    void Start()
    {
        if(aim == null)
        {
            aim = GameObject.Find("Player").transform;
        }
    }
    void Update()
    {
        if (aim == null || pupil == null || center == null) return;

        Vector3 dir = (aim.position - center.position).normalized;

        Vector3 offset = dir * radius;

        pupil.localPosition = offset;
    }
}
