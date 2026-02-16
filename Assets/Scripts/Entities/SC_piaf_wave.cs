using UnityEngine;

public class SC_piaf_wave : MonoBehaviour
{
 public float radius = 3f;  // Rayon de l'onde
    public LayerMask birdLayer;
    public float speed = 5f;

    private SC_piaf_brain originBird;

    public void Initialize(SC_piaf_brain bird)
    {
        originBird = bird;
        Destroy(gameObject, 1f); // Durée de vie de l'onde
    }

    void Update()
    {
        // Faire grandir l'onde
        transform.localScale += Vector3.one * speed * Time.deltaTime;
        if (transform.localScale.sqrMagnitude > radius) 
        { 
            Destroy(gameObject);
        }
        // Détecter les autres oiseaux dans la zone verte
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2, birdLayer);
        foreach (Collider2D hit in hits)
        {
            SC_piaf_brain bird = hit.GetComponent<SC_piaf_brain>();
            if (bird != null && bird != originBird)
            {
                bird.Invoke_StartFlee();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2);
    }
}