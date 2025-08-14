using Unity.VisualScripting;
using UnityEngine;

public class SC_Movement_anim : MonoBehaviour
{
    public Vector2 speed;
    public Vector2 distance;
    Vector3 initpos;
    public Vector2 Random_multiplier;

    private void Start()
    {
        if(Random_multiplier != Vector2.zero)
        {
            distance = new Vector2(distance.x + Random.Range(-Random_multiplier.x, Random_multiplier.x), distance.y + Random.Range(-Random_multiplier.y, Random_multiplier.y));
         }
        initpos = transform.localPosition;
    }
    public void Update()
    {
        transform.localPosition = new Vector3(Mathf.Sin(Time.time * speed.x) * distance.x + initpos.x + Random_multiplier.x, Mathf.Sin(Time.time * speed.y) * distance.y + initpos.y + Random_multiplier.y, 0);
    }

    public void OnDisable()
    {
        transform.localPosition = new Vector3(initpos.x, initpos.y);
    }
}
