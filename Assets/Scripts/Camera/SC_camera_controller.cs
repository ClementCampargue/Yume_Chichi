using UnityEngine;


public class SC_camera_controller : MonoBehaviour
{
    public static SC_camera_controller instance;

    public Transform target;
    public Vector2 offset;
    public bool Horizontal_follow = true;
    public bool Vertical_follow = true;

    [Header("Collision Settings")]
    public LayerMask obstacleMask;
    public float collisionOffset = 0.1f;
    public float returnAfterCollisionSpeed = 8f; // vitesse de retour après collision

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

    private bool wasCollidingX = false;
    private bool wasCollidingY = false;

    private void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition;

        // -------- LOGIQUE CAMERA --------
        if (isTransitioning)
        {
            if (returningToPlayer)
            {
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

        // -------- COLLISION CAMERA + RETOUR DOUX --------
        Vector2 desiredPosition = newPosition;
        Vector2 collidedPosition = HandleCameraCollision(currentPosition, desiredPosition);

        // Gestion collision par axe
        // Axe X
        if (Mathf.Abs(collidedPosition.x - desiredPosition.x) > 0.001f)
        {
            wasCollidingX = true;
            newPosition.x = collidedPosition.x;
        }
        else if (wasCollidingX)
        {
            newPosition.x = Mathf.MoveTowards(currentPosition.x, desiredPosition.x, returnAfterCollisionSpeed * Time.deltaTime);
            if (Mathf.Abs(newPosition.x - desiredPosition.x) < 0.02f)
            {
                newPosition.x = desiredPosition.x;
                wasCollidingX = false;
            }
        }
        else
        {
            newPosition.x = desiredPosition.x;
        }

        // Axe Y
        if (Mathf.Abs(collidedPosition.y - desiredPosition.y) > 0.001f)
        {
            wasCollidingY = true;
            newPosition.y = collidedPosition.y;
        }
        else if (wasCollidingY)
        {
            newPosition.y = Mathf.MoveTowards(currentPosition.y, desiredPosition.y, returnAfterCollisionSpeed * Time.deltaTime);
            if (Mathf.Abs(newPosition.y - desiredPosition.y) < 0.02f)
            {
                newPosition.y = desiredPosition.y;
                wasCollidingY = false;
            }
        }
        else
        {
            newPosition.y = desiredPosition.y;
        }

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    Vector2 HandleCameraCollision(Vector2 current, Vector2 targetPos)
    {
        Vector2 finalPos = current;

        // --- TEST AXE X ---
        Vector2 testX = new Vector2(targetPos.x, current.y);
        RaycastHit2D hitX = Physics2D.Linecast(current, testX, obstacleMask);

        if (hitX.collider == null)
        {
            finalPos.x = testX.x;
        }

        // --- TEST AXE Y ---
        Vector2 testY = new Vector2(finalPos.x, targetPos.y);
        RaycastHit2D hitY = Physics2D.Linecast(new Vector2(finalPos.x, current.y), testY, obstacleMask);

        if (hitY.collider == null)
        {
            finalPos.y = testY.y;
        }

        return finalPos;
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