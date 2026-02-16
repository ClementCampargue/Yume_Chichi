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
    public float objectiveFreezeDuration = 1f; // Nouveau délai pour les objectives
    public float avoidanceDistance = 1f;
    public float avoidanceStrength = 3f;
    public LayerMask obstacleLayer;
    public float ObjectiveDetectionDistance = 5f;
    public string objectiveTag = "Objective";
    public float grabReleaseMashThreshold = 5f;
    public float grabStunDuration = 1f;
    public float grabDelayBeforeReturn = 1f;

    [Header("Animation")]
    public Animator animator; // Animator attaché à la main

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
    private bool isObjectiveFreeze = false;

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

        if (animator == null)
            animator = GetComponent<Animator>();
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
                animator.SetTrigger("StartMove"); // Repart après stun
            }
            return;
        }

        // Détection distance et changement de cible si objectif
        Collider2D[] colliders = Physics2D.OverlapCircleAll(rb.position, ObjectiveDetectionDistance);
        currentTarget = player; // défaut

        bool foundObjective = false;
        foreach (var col in colliders)
        {
            if (col.CompareTag(objectiveTag))
            {
                currentTarget = col.transform;
                foundObjective = true;
                break;
            }
        }

        float distanceToTarget = Vector2.Distance(rb.position, currentTarget.position);

        // Activation attaque
        if (!isAttacking && !isReturning && !isFreezing && !isGrabbingPlayer && distanceToTarget <= detectionDistance)
        {
            isAttacking = true;
            attackTimer = attackDuration;
            pathPositions.Clear();
            pathPositions.Add(rb.position);

            animator.SetTrigger("StartMove"); // Animation de départ
        }

        // --- PHASE ATTAQUE ---
        if (isAttacking)
        {
            attackTimer -= Time.fixedDeltaTime;

            Vector2 newPos = MoveTowards(currentTarget.position, speed, false);

            if (Vector2.Distance(pathPositions[pathPositions.Count - 1], newPos) > 0.05f)
                pathPositions.Add(newPos);

            // Si on touche le joueur
            if (!isGrabbingPlayer && currentTarget.CompareTag("Player") && Vector2.Distance(rb.position, player.position) < 0.5f)
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

                animator.SetTrigger("ReachTarget"); // Animation quand elle touche le player
            }

            // Si on touche un objective
            if (!isGrabbingPlayer && currentTarget.CompareTag(objectiveTag) && Vector2.Distance(rb.position, currentTarget.position) < 0.1f)
            {
                isAttacking = false;
                isFreezing = true;
                freezeTimer = objectiveFreezeDuration; // juste un délai différent
                if (pathPositions.Count == 0 || pathPositions[pathPositions.Count - 1] != rb.position)
                    pathPositions.Add(rb.position);

                returnIndex = pathPositions.Count - 1;

                animator.SetTrigger("ReachTarget"); // Animation quand elle touche un objectif
            }

            // Si le timer attaque s’épuise
            if (attackTimer <= 0f && !isGrabbingPlayer && !currentTarget.CompareTag(objectiveTag))
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
                animator.SetTrigger("StartMove"); // Repart après freeze
            }
            return;
        }

        // --- PHASE RETOUR (inclut grab) ---
        if (isReturning || isGrabbingPlayer)
        {
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

            if (isGrabbingPlayer)
                player.position = newPos;

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

        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, avoidanceDistance, obstacleLayer);
        Vector2 moveDirection = direction;

        if (hit.collider != null)
        {
            Vector2 avoidDirection = Vector2.Perpendicular(hit.normal).normalized;
            moveDirection = (direction + avoidDirection * avoidanceStrength).normalized;
        }

        Vector2 rotationDir = invertRotation ? -moveDirection : moveDirection;
        float angle = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

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
                isGrabbingPlayer = false;
                isGrabDelay = false;
                isStunned = true;
                stunTimer = grabStunDuration;

                isReturning = true;
                animator.SetTrigger("StartMove"); // repart après stun
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
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Vector2 gizmoPosition = Application.isPlaying ? rb.position : transform.position;
        Gizmos.DrawSphere(gizmoPosition, 0.1f);
        Gizmos.DrawWireSphere(gizmoPosition, detectionDistance);

        Gizmos.color = new Color(1f, 0f, 1f, 0.3f);
        Vector2 gizmoPosition2 = Application.isPlaying ? rb.position : transform.position;
        Gizmos.DrawSphere(gizmoPosition2, 0.1f);
        Gizmos.DrawWireSphere(gizmoPosition2, ObjectiveDetectionDistance);
    }
}