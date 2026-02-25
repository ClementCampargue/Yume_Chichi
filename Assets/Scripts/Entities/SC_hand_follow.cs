using System.Collections.Generic;
using UnityEngine;

public class SC_hand_follow : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 4f;
    public float returnSpeed = 6f;
    public float acceleration = 8f;
    public float detectionDistance = 5f;
    public float attackDuration = 2f;
    public float freezeDuration = 0.5f;
    public float objectiveFreezeDuration = 1f;

    [Header("Avoidance")]
    public float avoidanceDistance = 1f;
    public float avoidanceStrength = 3f;
    public LayerMask obstacleLayer;

    [Header("Objective")]
    public float objectiveDetectionDistance = 5f;
    public string objectiveTag = "Objective";

    [Header("Grab")]
    public float grabReleaseMashThreshold = 5f;
    public float grabStunDuration = 1f;
    public float grabDelayBeforeReturn = 1f;

    [Header("Animation")]
    public Animator animator;

    private Transform player;
    private Transform currentTarget;
    private Rigidbody2D rb;

    private Vector2 startPosition;

    private bool isAttacking;
    private bool isFreezing;
    private bool isReturning;
    private bool isGrabbing;
    private bool isStunned;
    private bool isGrabDelay;

    private float attackTimer;
    private float freezeTimer;
    private float stunTimer;
    private float grabDelayTimer;

    private float currentSpeed;

    private List<Vector2> pathPositions = new List<Vector2>();
    private int returnIndex;

    private int mashCounter;

    private SC_trigger_cutscene cutscene;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        cutscene = GetComponent<SC_trigger_cutscene>();
        startPosition = rb.position;
        currentTarget = player;

        if (!animator)
            animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!player) return;

        HandleStun();

        if (isStunned) return;

        DetectTarget();

        if (!isAttacking && !isReturning && !isFreezing && !isGrabbing)
            TryStartAttack();

        if (isAttacking)
        {
            HandleAttack();
            return;
        }

        if (isFreezing)
        {
            HandleFreeze();
            return;
        }

        if (isReturning || isGrabbing)
        {
            HandleReturn();
            return;
        }
    }

    void HandleStun()
    {
        if (!isStunned) return;

        stunTimer -= Time.fixedDeltaTime;

        if (stunTimer <= 0f)
        {
            isStunned = false;
            isReturning = true;
            animator.SetTrigger("StartMove");
        }
    }

    void DetectTarget()
    {
        currentTarget = player;

        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(rb.position, objectiveDetectionDistance);

        foreach (var col in colliders)
        {
            if (col.CompareTag(objectiveTag))
            {
                currentTarget = col.transform;
                break;
            }
        }
    }

    void TryStartAttack()
    {
        float distance = Vector2.Distance(rb.position, currentTarget.position);

        if (distance > detectionDistance) return;

        isAttacking = true;
        attackTimer = attackDuration;
        currentSpeed = 0f;

        pathPositions.Clear();
        pathPositions.Add(rb.position);
        cutscene.start_cutscene();
        animator.SetTrigger("StartMove");
    }

    void HandleAttack()
    {
        attackTimer -= Time.fixedDeltaTime;

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            maxSpeed,
            acceleration * Time.fixedDeltaTime
        );

        Vector2 newPos = MoveTowards(currentTarget.position, currentSpeed);

        if (Vector2.Distance(pathPositions[pathPositions.Count - 1], newPos) > 0.05f)
            pathPositions.Add(newPos);

        CheckPlayerGrab();
        CheckObjectiveHit();

        if (attackTimer <= 0f && !isGrabbing)
        {
            StartFreeze(freezeDuration);
        }
    }

    void CheckPlayerGrab()
    {
        if (isGrabbing) return;
        if (!currentTarget.CompareTag("Player")) return;

        if (Vector2.Distance(rb.position, player.position) < 0.5f)
        {
            isGrabbing = true;
            isAttacking = false;
            isGrabDelay = true;
            grabDelayTimer = grabDelayBeforeReturn;
            mashCounter = 0;

            returnIndex = pathPositions.Count - 1;

            animator.SetTrigger("ReachTarget");
        }
    }

    void CheckObjectiveHit()
    {
        if (!currentTarget.CompareTag(objectiveTag)) return;

        if (Vector2.Distance(rb.position, currentTarget.position) < 0.1f)
        {
            isAttacking = false;
            StartFreeze(objectiveFreezeDuration);
            animator.SetTrigger("ReachTarget");
        }
    }

    void StartFreeze(float duration)
    {
        isAttacking = false;
        isFreezing = true;
        freezeTimer = duration;
        returnIndex = pathPositions.Count - 1;
    }

    void HandleFreeze()
    {
        freezeTimer -= Time.fixedDeltaTime;

        if (freezeTimer <= 0f)
        {
            isFreezing = false;
            isReturning = true;
            animator.SetTrigger("StartMove");
        }
    }

    void HandleReturn()
    {
        if (isGrabbing && isGrabDelay)
        {
            grabDelayTimer -= Time.fixedDeltaTime;
            player.position = rb.position;

            if (grabDelayTimer <= 0f)
                isGrabDelay = false;
            else
                return;
        }

        Vector2 targetPos =
            (returnIndex >= 0) ? pathPositions[returnIndex] : startPosition;

        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            targetPos,
            returnSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPos);

        if (isGrabbing)
            player.position = newPos;

        RotateTowards(rb.position - targetPos);

        if (Vector2.Distance(newPos, targetPos) <= 0.01f)
            returnIndex--;

        if (returnIndex < 0)
        {
            rb.position = startPosition;
            rb.rotation = 0f;
            isReturning = false;
            isGrabbing = false;
        }
    }

    Vector2 MoveTowards(Vector2 target, float speed)
    {
        Vector2 direction = (target - rb.position).normalized;

        RaycastHit2D hit =
            Physics2D.Raycast(rb.position, direction, avoidanceDistance, obstacleLayer);

        Vector2 moveDirection = direction;

        if (hit.collider)
        {
            Vector2 avoidDir =
                Vector2.Perpendicular(hit.normal).normalized;

            moveDirection =
                (direction + avoidDir * avoidanceStrength).normalized;
        }

        RotateTowards(moveDirection);

        Vector2 newPos =
            rb.position + moveDirection * speed * Time.fixedDeltaTime;

        rb.MovePosition(newPos);

        return newPos;
    }

    void RotateTowards(Vector2 dir)
    {
        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rb.rotation = angle;
    }

    public void MashAttempt()
    {
        if (!isGrabbing) return;

        mashCounter++;

        if (mashCounter >= grabReleaseMashThreshold)
        {
            isGrabbing = false;
            isGrabDelay = false;
            isStunned = true;
            stunTimer = grabStunDuration;

            isReturning = true;
            animator.SetTrigger("StartMove");
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
            MashAttempt();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Vector2 pos =
            Application.isPlaying ? rb.position : transform.position;

        Gizmos.DrawWireSphere(pos, detectionDistance);

        Gizmos.color = new Color(1f, 0f, 1f, 0.3f);
        Gizmos.DrawWireSphere(pos, objectiveDetectionDistance);
    }
}