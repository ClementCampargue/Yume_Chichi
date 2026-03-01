using UnityEngine;
using UnityEngine.UI;
public class SC_Color_anim : MonoBehaviour
{
    private Image image;
    private SpriteRenderer sprite;

    public float fadeSpeed = 1f;
    public float maxAlpha = 1f;
    public float minAlpha = 0f;
    public float holdTime = 0.5f; // temps d'attente au max

    private float holdTimer = 0f;

    private enum FadeState { FadingUp, Holding, FadingDown }
    private FadeState state = FadeState.FadingUp;

    private void Start()
    {
        image = GetComponent<Image>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float delta = Time.deltaTime * fadeSpeed;

        Color currentColor = GetCurrentColor();

        switch (state)
        {
            case FadeState.FadingUp:
                currentColor.a += delta;
                if (currentColor.a >= maxAlpha)
                {
                    currentColor.a = maxAlpha;
                    state = FadeState.Holding;
                    holdTimer = holdTime;
                }
                break;

            case FadeState.Holding:
                holdTimer -= Time.deltaTime;
                if (holdTimer <= 0f)
                {
                    state = FadeState.FadingDown;
                }
                break;

            case FadeState.FadingDown:
                currentColor.a -= delta;
                if (currentColor.a <= minAlpha)
                {
                    currentColor.a = minAlpha;
                    Destroy(gameObject); // optionnel
                }
                break;
        }

        SetColor(currentColor);
    }

    Color GetCurrentColor()
    {
        if (image != null)
            return image.color;
        else
            return sprite.color;
    }

    void SetColor(Color c)
    {
        if (image != null)
            image.color = c;
        else
            sprite.color = c;
    }
}