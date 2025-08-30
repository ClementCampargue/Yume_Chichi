using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SC_light_flicker : MonoBehaviour
{
    private Light2D light;

    public float speed = 1;
    public float amplitude = 1;


    private float base_intensity;
    void Start()
    {
        light = GetComponent<Light2D>();
        base_intensity = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = base_intensity + amplitude/2 * Mathf.Sin(Time.time * speed);
    }
}
