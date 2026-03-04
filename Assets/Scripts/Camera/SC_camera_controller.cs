using UnityEngine;


public class SC_camera_controller : MonoBehaviour
{
    public static SC_camera_controller instance;

    public Transform target;
    public Vector2 offset;
    public bool Horizontal_follow = true;
    public bool Vertical_follow = true;

    [Header("Transition Settings")]
    public float cutsceneSmoothSpeed = 2f;
    public float minSpeed = 5f;
    public float acceleration = 10f;
    public float maxSpeed = 25f;

    private Transform cutsceneTarget;

    private bool inCutscene = false;
    private bool isTransitioning = false;
    private bool returningToPlayer = false;

    private Vector2 transitionTarget;

    private void Awake()
    {
        instance = this;
    }

    void LateUpdate() // IMPORTANT : caméra = LateUpdate
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition;

        if (isTransitioning)
        {
            if (returningToPlayer)
            {
                // Retour vers le joueur avec accélération progressive
                transitionTarget = new Vector2(
                    target.position.x + offset.x,
                    target.position.y + offset.y
                );

                float distance = Vector2.Distance(currentPosition, transitionTarget);
                float speed = Mathf.Clamp(minSpeed + distance * acceleration, minSpeed, maxSpeed);

                newPosition = Vector2.MoveTowards(
                    currentPosition,
                    transitionTarget,
                    speed * Time.deltaTime
                );

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

                    newPosition = Vector2.Lerp(
                        currentPosition,
                        transitionTarget,
                        cutsceneSmoothSpeed * Time.deltaTime
                    );

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
                newPosition = cutsceneTarget.position;
            }
            else
            {
                if (Horizontal_follow)
                    newPosition.x = target.position.x + offset.x;

                if (Vertical_follow)
                    newPosition.y = target.position.y + offset.y;
            }
        }

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
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