using UnityEngine;

public class SC_animate_distance : MonoBehaviour
{
    private Transform trs;
    public float distance;
    public Animator animator_;
    void Start()
    {
        trs = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(trs.position, transform.position) < distance)
        {
            animator_.enabled = true;
            this.enabled = false;
        }
    }
}
