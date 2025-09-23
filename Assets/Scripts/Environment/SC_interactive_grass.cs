using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
public class SC_interactive_grass : MonoBehaviour
{
    Material material;
    SpriteRenderer spriteRenderer;
    MeshRenderer meshrenderer;
    List<Transform> entities = new List<Transform>();
    bool emptylist = true;
    float pos = 0;

    private void Awake()
    {
        meshrenderer = GetComponent<MeshRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (meshrenderer != null)
        {
            material = meshrenderer.material;
        }
        else if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (entities.Count <= 0)
        {
            if (!emptylist)
            {
                Vector3 currentMatPos = material.GetVector("_Pos");
                material.SetVector("_Pos", currentMatPos + new Vector3(0f, pos, 0f));
                if (pos >= 50f) emptylist = true;
                pos += 0.01f;
            }
            return;

        }
        if (entities[entities.Count - 1] == null) return;
        material.SetVector("_Pos", entities[entities.Count - 1].position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        entities.Add(collision.transform);
        emptylist = false;
        pos = 0f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        entities.Remove(collision.transform);
    }

    private void OnTriggerEnter(Collider collision)
    {
        entities.Add(collision.transform);
        emptylist = false;
        pos = 0f;
    }

    private void OnTriggerExit(Collider collision)
    {
        entities.Remove(collision.transform);
    }

}
