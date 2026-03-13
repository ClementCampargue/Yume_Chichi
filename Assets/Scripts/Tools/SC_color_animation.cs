using UnityEngine;

public class SC_color_animation : MonoBehaviour
{
    public float speed = 0.2f; // vitesse de rotation de la teinte

    private SpriteRenderer spriteRenderer;
    private float hue = 0f;
    private float saturation = 1f;
    private float value = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // récupérer la couleur initiale
        Color.RGBToHSV(spriteRenderer.color, out hue, out saturation, out value);
    }

    void Update()
    {
        // faire tourner la teinte en continu
        hue += Time.deltaTime * speed;

        if (hue > 1f)
            hue -= 1f;

        // convertir HSV -> RGB
        spriteRenderer.color = Color.HSVToRGB(hue, saturation, value);
    }
}