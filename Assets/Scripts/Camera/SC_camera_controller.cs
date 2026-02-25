using UnityEngine;


public class SC_camera_controller : MonoBehaviour
{
    public static SC_camera_controller instance;

    public Transform target;
    public Vector2 offset;
    public bool Horizontal_follow = true;
    public bool Vertical_follow = true;

    private Rigidbody2D rb;

    [Header("Transition Settings")]
    public float cutsceneSmoothSpeed = 2f; // vitesse pour focus cutscene
    public float minSpeed = 5f;            // vitesse min pour rattraper joueur
    public float acceleration = 10f;       // facteur d'accélération selon distance
    public float maxSpeed = 25f;           // vitesse max retour joueur

    private Vector2 velocity;              // pour SmoothDamp si nécessaire

    private Transform cutsceneTarget;

    private bool inCutscene = false;
    private bool isTransitioning = false;
    private bool returningToPlayer = false;

    private Vector2 transitionTarget;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position;

        if (isTransitioning)
        {
            if (returningToPlayer)
            {
                // Retour vers le joueur avec accélération progressive
                transitionTarget = new Vector2(target.position.x + offset.x, target.position.y + offset.y);

                float distance = Vector2.Distance(rb.position, transitionTarget);
                float speed = Mathf.Clamp(minSpeed + distance * acceleration, minSpeed, maxSpeed);

                newPosition = Vector2.MoveTowards(rb.position, transitionTarget, speed * Time.fixedDeltaTime);

                if (distance < 0.02f)
                {
                    newPosition = transitionTarget;
                    isTransitioning = false;
                }
            }
            else
            {
                // Focus cutscene smooth
                if (cutsceneTarget != null)
                {
                    transitionTarget = cutsceneTarget.position;
                    newPosition = Vector2.Lerp(rb.position, transitionTarget, cutsceneSmoothSpeed * Time.fixedDeltaTime);

                    if (Vector2.Distance(newPosition, transitionTarget) < 0.02f)
                    {
                        newPosition = transitionTarget;
                        isTransitioning = false;
                    }
                }
            }
        }
        else
        {
            if (inCutscene && cutsceneTarget != null)
            {
                // Caméra fixe sur l'élément cutscene
                newPosition = cutsceneTarget.position;
            }
            else
            {
                // Suivi instantané joueur
                if (Horizontal_follow)
                    newPosition.x = target.position.x + offset.x;
                if (Vertical_follow)
                    newPosition.y = target.position.y + offset.y;
            }
        }

        rb.MovePosition(newPosition);
    }

    public void start_cutscene(Transform aim)
    {
        cutsceneTarget = aim;
        inCutscene = true;

        transitionTarget = aim.position;
        isTransitioning = true;
        returningToPlayer = false;
    }

    public void end_cutscene()
    {
        inCutscene = false;

        isTransitioning = true;
        returningToPlayer = true;
    }
}