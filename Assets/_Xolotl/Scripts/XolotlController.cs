using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Movimiento de Xólotl: aceleración, salto con coyote time, jump buffer,
/// salto variable, caída más rápida y botón de llamarada.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class XolotlController : MonoBehaviour
{
    [Header("Input (arrastra las acciones de InputSystem_Actions)")]
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] InputActionReference flareAction;

    [Header("Movimiento")]
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float acceleration = 60f;
    [SerializeField] float deceleration = 70f;

    [Header("Salto")]
    [SerializeField] float jumpForce = 13f;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.1f;
    [Range(0f, 1f)][SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float fallGravityMultiplier = 1.8f;
    [SerializeField] float maxFallSpeed = 18f;

    [Header("Detección de suelo")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 groundCheckSize = new(0.8f, 0.1f);
    [SerializeField] LayerMask groundLayers;

    [Header("Referencias")]
    [SerializeField] XolotlLight xolotlLight;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [Tooltip("Actívalo si el sprite original mira a la izquierda")]
    [SerializeField] bool spriteFacesLeft = false;

    Rigidbody2D rb;
    float baseGravity;
    float moveInput;
    float coyoteCounter;
    float jumpBufferCounter;
    float knockbackTimer;
    bool isGrounded;
    bool facingRight = true;
    bool controlsEnabled = true;

    static readonly int SpeedHash = Animator.StringToHash("Speed");
    static readonly int GroundedHash = Animator.StringToHash("Grounded");
    static readonly int VerticalSpeedHash = Animator.StringToHash("VerticalSpeed");
    static readonly int FlareHash = Animator.StringToHash("Flare");

    public bool IsGrounded => isGrounded;
    public bool FacingRight => facingRight;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseGravity = rb.gravityScale;
        if (xolotlLight == null) xolotlLight = GetComponent<XolotlLight>();
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        flareAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        flareAction.action.Disable();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayers);

        if (knockbackTimer > 0f) knockbackTimer -= Time.deltaTime;

        if (!controlsEnabled)
        {
            moveInput = 0f;
            UpdateAnimator();
            return;
        }

        moveInput = moveAction.action.ReadValue<Vector2>().x;

        // Coyote time: puedes saltar unos milisegundos después de dejar la orilla
        coyoteCounter = isGrounded ? coyoteTime : coyoteCounter - Time.deltaTime;

        // Jump buffer: si presionas justo antes de tocar el suelo, el salto se guarda
        if (jumpAction.action.WasPressedThisFrame()) jumpBufferCounter = jumpBufferTime;
        else jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }

        // Salto variable: soltar el botón corta el salto
        if (jumpAction.action.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);

        if (flareAction.action.WasPressedThisFrame() && xolotlLight != null && xolotlLight.TryFlare())
        {
            if (animator != null) animator.SetTrigger(FlareHash);
        }

        if (moveInput > 0.01f && !facingRight) Flip();
        else if (moveInput < -0.01f && facingRight) Flip();

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;

        if (knockbackTimer <= 0f)
        {
            float targetSpeed = moveInput * moveSpeed;
            float rate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
            velocity.x = Mathf.MoveTowards(velocity.x, targetSpeed, rate * Time.fixedDeltaTime);
        }

        rb.gravityScale = velocity.y < 0f ? baseGravity * fallGravityMultiplier : baseGravity;
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

        rb.linearVelocity = velocity;
    }

    void Flip()
    {
        facingRight = !facingRight;
        if (spriteRenderer != null)
            spriteRenderer.flipX = spriteFacesLeft ? facingRight : !facingRight;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;
        animator.SetFloat(SpeedHash, Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool(GroundedHash, isGrounded);
        animator.SetFloat(VerticalSpeedHash, rb.linearVelocity.y);
    }

    /// <summary>Empujón cuando te toca un olvidado.</summary>
    public void Knockback(Vector2 force, float duration = 0.2f)
    {
        rb.linearVelocity = force;
        knockbackTimer = duration;
    }

    /// <summary>Para derrota, escena final y pausa.</summary>
    public void SetControlsEnabled(bool enabled)
    {
        controlsEnabled = enabled;
        if (!enabled) rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
