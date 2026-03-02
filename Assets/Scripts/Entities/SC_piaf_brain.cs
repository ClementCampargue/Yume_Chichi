using System.Collections;
using UnityEngine;

public class SC_piaf_brain : MonoBehaviour
{
    [Header("Movement")]
    public float fleeRadiusX = 6f;   // horizontal
    public float fleeRadiusY = 3f;   // vertical
    public float fleeDelay = 0.5f;
    public float fleeSpeed = 10f;

    [Header("Wave")]
    public GameObject wavePrefab;
    public float waveDuration = 1f;

    [Header("Obstacle Avoidance")]
    public float obstacleDetectDistance = 1f; // Distance pour détecter les obstacles
    public LayerMask obstacleLayer;

    [Header("Debug")]
    public bool debugMode = true;

    private Transform player;
    private bool isFleeing = false;
    private Vector3 fleeDirection;
    private bool isCoroutineRunning = false;
    public Animator anim;
    private Vector3 originalScale;
    private bool facingRight;
    private Vector3 startPosition;
    private bool respawning;

    public float respawn_dist = 10;
    void Start()
    {
        originalScale = transform.localScale;
        startPosition = transform.position;
        player = SC_Player_controller.instance.transform;
    }

    void Update()
    {

        if ((respawning))
        {
            if (Vector2.Distance(startPosition, player.position) > respawn_dist)
            {
                reset();
            }

                return;
        }

        if (fleeDirection.x != 0) // éviter la division par zéro ou jitter quand x ~ 0
        {
            bool movingRight = fleeDirection.x > 0;
            if (movingRight && !facingRight)
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                facingRight = true;
            }
            else if (!movingRight && facingRight)
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                facingRight = false;
            }
        }

        if (isFleeing)
        {
            if(Vector2.Distance(transform.position, player.position) > 8)
            {
                respawning = true;
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }



        if (!isFleeing && !isCoroutineRunning)
        {
            Vector2 dirToPlayer = player.position - transform.position;

            float x = dirToPlayer.x;
            float y = dirToPlayer.y;

            float ellipseCheck =
                (x * x) / (fleeRadiusX * fleeRadiusX) +
                (y * y) / (fleeRadiusY * fleeRadiusY);

            if (ellipseCheck <= 1f)
            {
                StartCoroutine(FleeAfterDelay());
            }
        }
        else if (isFleeing)
        {
            // Détection et évitement simple des obstacles
            RaycastHit2D hit = Physics2D.Raycast(transform.position, fleeDirection, obstacleDetectDistance, obstacleLayer);
            if (hit.collider != null)
            {
                Vector3 avoidDir = Vector3.Cross(hit.normal, Vector3.forward).normalized;
                fleeDirection = (fleeDirection + avoidDir * 0.5f).normalized;

                if (debugMode)
                    Debug.DrawRay(transform.position, fleeDirection * obstacleDetectDistance, Color.red);
            }
            else
            {
                if (debugMode)
                    Debug.DrawRay(transform.position, fleeDirection * obstacleDetectDistance, Color.green);
            }
            transform.position += fleeDirection * fleeSpeed * Time.deltaTime;
        }
    }

    public void Invoke_StartFlee()
    {
        StartCoroutine(FleeAfterDelay());
    }

    public IEnumerator FleeAfterDelay()
    {
        anim.SetTrigger("scream");
        isCoroutineRunning = true;
        yield return new WaitForSeconds(fleeDelay);
        StartFlee();
    }

    public void StartFlee()
    {
        if (isFleeing) return;

        isFleeing = true;

        // Direction opposée au joueur
        fleeDirection = (transform.position - player.position).normalized;
        fleeDirection += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
        fleeDirection.Normalize();

        // Créer l'onde
        if (wavePrefab != null)
        {
            GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
            wave.GetComponent<SC_piaf_wave>().Initialize(this);
        }

        // Gestion de l'échelle
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        int segments = 60;
        Vector3 prevPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float x = Mathf.Cos(angle) * fleeRadiusX;
            float y = Mathf.Sin(angle) * fleeRadiusY;

            Vector3 point = transform.position + new Vector3(x, y, 0);

            if (i > 0)
                Gizmos.DrawLine(prevPoint, point);

            prevPoint = point;
        }
    }

    void reset()
    {
        anim.SetTrigger("idle");

        respawning = false;
        // Désactive visuellement
        gameObject.SetActive(false);

        transform.GetChild(0).gameObject.SetActive(true);
        // Reset état
        isFleeing = false;
        isCoroutineRunning = false;
        fleeDirection = Vector3.zero;

        transform.position = startPosition;
        transform.localScale = originalScale;
        facingRight = false;

        gameObject.SetActive(true);

    }
}