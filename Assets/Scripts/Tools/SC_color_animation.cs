using UnityEngine;

public class SC_color_animation : MonoBehaviour
{
    public Color[] colors;    
    public float speed = 2f;   

    private SpriteRenderer spriteRenderer;
    private int currentColor = 0;
    private int nextColor = 1;
    private float t = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (colors == null || colors.Length == 0)
        {
            colors = new Color[]
            {
                Color.red,
                new Color(1f, 0.5f, 0f),
                Color.yellow,
                Color.green,
                Color.cyan,
                Color.blue,
                new Color(0.6f, 0f, 1f) 
            };
        }

        if (colors.Length == 1)
        {
            colors = new Color[] { colors[0], colors[0] };
        }
    }

    void Update()
    {
        t += Time.deltaTime * speed;

        spriteRenderer.color = Color.Lerp(colors[currentColor], colors[nextColor], t);

        if (t >= 1f)
        {
            t = 0f;
            currentColor = nextColor;
            nextColor = (nextColor + 1) % colors.Length;
        }
    }
}