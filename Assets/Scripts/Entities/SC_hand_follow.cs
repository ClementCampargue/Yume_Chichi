using System.Collections.Generic;
using UnityEngine;

public class SC_hand_follow : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4f;
    public float returnSpeed = 6f;
    public float detectionDistance = 5f;
    public float attackDuration = 2f;
    public float freezeDuration = 0.5f;
    public float avoidanceDistance = 1f;
    public float avoidanceStrength = 3f;
    public LayerMask obstacleLayer;
    public string objectiveTag = "Objective";
    public float grabReleaseMashThreshold = 5f;
    public float grabStunDuration = 1f;
    public float grabDelayBeforeReturn = 1f;

    private Transform player;
    private Transform currentTarget;
    private Rigidbody2D rb;

    private Vector2 startPosition;

    private bool isAttacking = false;
    private bool isFreezing = false;
    private bool isReturning = false;
    private bool isGrabbingPlayer = false;
    private bool isStunned = false;
    private bool isGrabDelay = false;

    private float attackTimer = 0f;
    private float freezeTimer = 0f;
    private float stunTimer = 0f;
    private float grabDelayTimer = 0f;

    private List<Vector2> pathPositions = new List<Vector2>();
    private int returnIndex = 0;

    private int mashCounter = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        currentTarget = player;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Gestion du stun
        if (isStunned)
        {
            stunTimer -= Time.fixedDeltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                isReturning = true;
            }
            return;
        }

        // Détection distance et changement de cible si objectif
        Collider2D objectiveNearby = Physics2D.OverlapCircle(rb.position, detectionDistance, LayerMask.GetMask(objectiveTag));
        currentTarget = objectiveNearby != null ? objectiveNearby.transform : player;

        float distanceToTarget = Vector2.Distance(rb.position, currentTarget.position);

        // Activation attaque
        if (!isAttacking && !isReturning && !isFreezing && !isGrabbingPlayer && distanceToTarget <= detectionDistance)
        {
            isAttacking = true;
            attackTimer = attackDuration;
            pathPositions.Clear();
            pathPositions.Add(rb.position);
        }

        // --- PHASE ATTAQUE ---
        if (isAttacking)
        {
            attackTimer -= Time.fixedDeltaTime;

            Vector2 newPos = MoveTowards(currentTarget.position, speed, false); // rotation normale

            // Enregistrement du chemin
            if (Vector2.Distance(pathPositions[pathPositions.Count - 1], newPos) > 0.05f)
                pathPositions.Add(newPos);

            // Vérifie si on touche le joueur
            if (!isGrabbingPlayer && Vector2.Distance(rb.position, player.position) < 0.5f)
            {
                isGrabbingPlayer = true;
                isAttacking = false;
                attackTimer = 0f;
                mashCounter = 0;
                isGrabDelay = true;
                grabDelayTimer = grabDelayBeforeReturn;

                if (pathPositions.Count == 0 || pathPositions[pathPositions.Count - 1] != rb.position)
                    pathPositions.Add(rb.position);

                returnIndex = pathPositions.Count - 1;
            }

            if (attackTimer <= 0f && !isGrabbingPlayer)
            {
                isAttacking = false;
                isFreezing = true;
                freezeTimer = freezeDuration;
                returnIndex = pathPositions.Count - 1;
            }

            return;
        }

        // --- PHASE FREEZE ---
        if (isFreezing)
        {
            freezeTimer -= Time.fixedDeltaTime;
            if (freezeTimer <= 0f)
            {
                isFreezing = false;
                isReturning = true;
            }
            return;
        }

        // --- PHASE RETOUR (inclut grab) ---
        if (isReturning || isGrabbingPlayer)
        {
            // Délai avant retour si grab
            if (isGrabbingPlayer && isGrabDelay)
            {
                grabDelayTimer -= Time.fixedDeltaTime;
                player.position = rb.position;

                if (grabDelayTimer <= 0f)
                    isGrabDelay = false;
                else
                    return;
            }

            Vector2 targetPos = (returnIndex >= 0) ? pathPositions[returnIndex] : startPosition;
            Vector2 currentPos = rb.position;
            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, returnSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            // Déplace le joueur avec la main si grab
            if (isGrabbingPlayer)
                player.position = newPos;

            // Rotation vers l'opposé du mouvement de retour
            Vector2 rotationDir = currentPos - targetPos;
            float angle = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle;

            if (Vector2.Distance(newPos, targetPos) <= 0.01f)
                returnIndex--;

            if (returnIndex < 0)
            {
                rb.position = startPosition;
                rb.rotation = 0f;
                isReturning = false;
                if (isGrabbingPlayer) isGrabbingPlayer = false;
            }

            return;
        }
    }

    Vector2 MoveTowards(Vector2 targetPosition, float currentSpeed, bool invertRotation)
    {
        Vector2 direction = (targetPosition - rb.position).normalized;

        // Gestion obstacles
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, avoidanceDistance, obstacleLayer);
        Vector2 moveDirection = direction;

        if (hit.collider != null)
        {
            Vector2 avoidDirection = Vector2.Perpendicular(hit.normal).normalized;
            moveDirection = (direction + avoidDirection * avoidanceStrength).normalized;
        }

        // Rotation
        Vector2 rotationDir = invertRotation ? -moveDirection : moveDirection;
        float angle = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        // Déplacement
        Vector2 newPosition = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        return newPosition;
    }

    public void MashAttempt()
    {
        if (isGrabbingPlayer)
        {
            mashCounter++;
            if (mashCounter >= grabReleaseMashThreshold)
            {
                // Libération
                isGrabbingPlayer = false;
                isGrabDelay = false;
                isStunned = true;
                stunTimer = grabStunDuration;

                isReturning = true;
            }
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            MashAttempt();
    }
    private void OnDrawGizmosSelected()
    {
        // Couleur du gizmo : jaune translucide
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);

        // Dessine un cercle autour de la position de la main (ou startPosition si non play mode)
        Vector2 gizmoPosition = Application.isPlaying ? rb.position : transform.position;
        Gizmos.DrawSphere(gizmoPosition, 0.1f); // petit point central
        Gizmos.DrawWireSphere(gizmoPosition, detectionDistance);
    }
}