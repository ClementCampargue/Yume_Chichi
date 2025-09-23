using UnityEngine;

public class SC_clone_sprite : MonoBehaviour
{
    public SpriteRenderer sprite_reference;
    
    SpriteRenderer spr;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spr.sprite = sprite_reference.sprite;
    }
}
