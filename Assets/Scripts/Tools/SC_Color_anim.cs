using UnityEngine;
using UnityEngine.UI;
public class SC_Color_anim : MonoBehaviour
{
    private Image image;
    private SpriteRenderer _sprite;
    public float speed = 0.5f;
    public float amplitude = 0.5f;
    public float Base_alpha = 0.5f;

    private bool can_fade = false;

    private void Start()
    {
        if (gameObject.GetComponent<Image>() != null)
        {
            image = gameObject.GetComponent<Image>();
        }
        else if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            _sprite = gameObject.GetComponent<SpriteRenderer>();
        }
        Invoke("delay_start", 0.5f);
    }
    public void Update()
    {
        if (amplitude == 0)
        {
            if (can_fade)
            {
                if (speed > 0)
                {
                    if (image.color.a > 0f)
                    {
                        apply_effect();
                    }
                    else
                    {
                        Destroy(image);
                    }
                }
                else
                {
                    if (image.color.a < 255)
                    {
                        apply_effect();
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
     
        }
        else
        {
            apply_effect_sin();
        }
    }

    void delay_start()
    {
        can_fade = true;
    }

    void apply_effect_sin()
    {
        if (gameObject.GetComponent<Image>() != null)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Base_alpha + Mathf.Sin(Time.time * speed) * amplitude / 2);
        }
        else if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, Base_alpha + Mathf.Sin(Time.time * speed) * amplitude / 2);
        }
    }
    void apply_effect()
    {
        if (gameObject.GetComponent<Image>() != null)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - Time.time * speed );
        }
        else if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, _sprite.color.a - Time.time * speed );
        }
    }

}
