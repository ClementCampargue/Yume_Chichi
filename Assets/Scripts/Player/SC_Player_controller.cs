using Unity.Cinemachine;
using UnityEngine;

public class SC_Player_controller : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float debug_speed = 5f;
    public bool can_act = true;

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("State")]
    public bool facing_right = false;
    public bool sleeping;
    [HideInInspector] public float stairSpeed;

    private Vector2 input;
    private Vector2 respawnCoordinates;

    public static SC_Player_controller instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        int save = PlayerPrefs.GetInt("Save");
        respawnCoordinates = new Vector2(
            PlayerPrefs.GetFloat("Respawn_x" + save),
            PlayerPrefs.GetFloat("Respawn_y" + save)
        );
    }

    void Update()
    {
        if (!can_act) return;

        ReadInput();
        HandleAnimations();
        HandleActions();
    }

    void FixedUpdate()
    {
        if (!can_act) return;

        ApplyMovement();
    }

    // ================= INPUT =================

    void ReadInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input = Vector2.ClampMagnitude(input, 1f);
    }

    // ================= MOVEMENT =================

    void ApplyMovement()
    {
        Vector2 velocity = input * moveSpeed;

        if (stairSpeed != 0 && input.x != 0)
        {
            velocity.y = Mathf.Sign(input.x) * stairSpeed * moveSpeed;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.linearVelocity = velocity * debug_speed;
        }
        else
        {
            rb.linearVelocity = velocity;
        }
    }

    // ================= ANIMATIONS =================

    void HandleAnimations()
    {
        bool isMoving = input.sqrMagnitude != 0f;
        animator.SetBool("Moving", isMoving);

        if (!isMoving) return;

        if (Mathf.Abs(input.y) > Mathf.Abs(input.x))
        {
            // Mouvement vertical → reset du flip
            animator.SetBool("Up", input.y > 0);
            animator.SetBool("Down", input.y < 0);
            animator.SetBool("Side", false);

            // Reset flip à la valeur "de base" (par exemple personnage face droite)
            if (!facing_right)
                Flip();  // si le personnage n'est pas déjà "face à droite"
        }
        else
        {
            // Mouvement horizontal → flip possible
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Side", true);

            if (input.x > 0 && !facing_right)
                Flip();
            else if (input.x < 0 && facing_right)
                Flip();
        }
    }
    public void Flip()
    {
        facing_right = !facing_right;
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    // ================= ACTIONS =================

    void HandleActions()
    {
        //    if (Input.GetButtonDown("wake_up"))
        //    {
        //        if (sleeping) WakeUp();
        //       else GoBackToSleep();
        //  }

        //    if (Input.GetButtonDown("Action"))
        //   {
        //   PerformAction();
        //   }
    }

    void PerformAction()
    {
        // � impl�menter
    }

    void WakeUp()
    {
        int save = PlayerPrefs.GetInt("Save");
        PlayerPrefs.SetFloat("Respawn_x" + save, transform.position.x);
        PlayerPrefs.SetFloat("Respawn_y" + save, transform.position.y);
        respawnCoordinates = transform.position;
        sleeping = false;
    }

    void GoBackToSleep()
    {
        sleeping = true;
    }

    public void disable_controller()
    {
        animator.SetBool("Moving", false);
        can_act = false;
        rb.linearVelocity = Vector2.zero;

    }

    public void enable_controller()
    {
        can_act = true;
    }
}