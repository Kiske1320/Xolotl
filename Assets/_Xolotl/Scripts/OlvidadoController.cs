using UnityEngine;

/// <summary>
/// Olvidado: alma que ya nadie recuerda. Flota hacia Xólotl solo en la oscuridad.
/// Bajo cualquier luz se congela y se vuelve translúcido, pero tocarlo sigue quitando luz.
/// La llamarada lo empuja lejos.
/// No usa física: el daño se calcula por distancia, así no le afecta que
/// Player contra Enemy esté desactivado en la matriz de colisión.
/// Un solo script con dos configuraciones: normal (lento) y veloz.
/// </summary>
public class OlvidadoController : MonoBehaviour
{
    [Header("Persecución")]
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float detectionRange = 10f;
    [Tooltip("Variante del sótano: solo avanza mientras Xólotl se mueve")]
    [SerializeField] bool onlyMoveWhenPlayerMoves = false;
    [SerializeField] float playerMoveThreshold = 0.3f;

    [Header("Daño")]
    [SerializeField] float hitRadius = 0.7f;
    [SerializeField] float lightDamage = 1.5f;
    [SerializeField] float hitCooldown = 1f;
    [SerializeField] Vector2 knockbackForce = new(6f, 5f);

    [Header("Llamarada")]
    [SerializeField] float repelSpeed = 8f;
    [SerializeField] float repelDuration = 0.5f;

    [Header("Visual")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [Range(0f, 1f)][SerializeField] float frozenAlpha = 0.3f;
    [SerializeField] float alphaSpeed = 4f;
    [SerializeField] float bobAmplitude = 0.15f;
    [SerializeField] float bobSpeed = 2f;
    [Tooltip("Actívalo si el sprite original mira a la izquierda")]
    [SerializeField] bool spriteFacesLeft = false;

    [Header("Audio (opcional)")]
    [SerializeField] AudioClip hitSfx;
    [Range(0f, 1f)][SerializeField] float sfxVolume = 0.7f;

    XolotlLight xolotlLight;
    XolotlController xolotlController;
    Rigidbody2D playerRb;
    Vector3 startPosition;
    Vector3 basePosition;   // posición sin el balanceo
    Vector2 repelVelocity;
    float repelTimer;
    float hitTimer;
    float bobTime;
    float alpha = 1f;
    float baseAlpha = 1f;   // alpha del color del sprite fuera de la luz
    bool isFrozen;
    bool repelledThisFlare;
    bool pendingReset;

    public bool IsFrozen => isFrozen;

    void Awake()
    {
        xolotlLight = FindAnyObjectByType<XolotlLight>();
        if (xolotlLight != null)
        {
            xolotlController = xolotlLight.GetComponent<XolotlController>();
            playerRb = xolotlLight.GetComponent<Rigidbody2D>();
        }
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null) baseAlpha = alpha = spriteRenderer.color.a;

        startPosition = transform.position;
        basePosition = startPosition;
        bobTime = Random.Range(0f, 10f);
    }

    void Update()
    {
        if (xolotlLight == null) return;
        float dt = Time.deltaTime;

        // Mientras Xólotl está derrotado no se mueve; al reaparecer, el olvidado vuelve a su inicio
        if (xolotlLight.IsEmpty)
        {
            pendingReset = true;
            return;
        }
        if (pendingReset)
        {
            pendingReset = false;
            basePosition = startPosition;
            repelTimer = 0f;
        }

        if (hitTimer > 0f) hitTimer -= dt;

        Vector2 playerPos = xolotlLight.transform.position;
        Vector2 toPlayer = playerPos - (Vector2)basePosition;
        float distance = toPlayer.magnitude;

        isFrozen = LightRegistry.IsLit(basePosition);

        // Variante del sótano: si Xólotl se queda quieto, el olvidado también
        bool playerMoving = playerRb == null ||
            playerRb.linearVelocity.sqrMagnitude > playerMoveThreshold * playerMoveThreshold;
        bool canChase = !onlyMoveWhenPlayerMoves || playerMoving;

        // La llamarada empuja a los olvidados que alcanza (una vez por llamarada)
        if (xolotlLight.IsFlaring)
        {
            if (!repelledThisFlare && distance <= xolotlLight.Radius && distance > 0.01f)
            {
                repelledThisFlare = true;
                repelVelocity = -toPlayer / distance * repelSpeed;
                repelTimer = repelDuration;
            }
        }
        else
        {
            repelledThisFlare = false;
        }

        if (repelTimer > 0f)
        {
            repelTimer -= dt;
            basePosition += (Vector3)(repelVelocity * dt);
        }
        else if (!isFrozen && canChase && distance <= detectionRange && distance > 0.01f)
        {
            basePosition += (Vector3)(toPlayer / distance * moveSpeed * dt);
        }

        // El balanceo se detiene cuando está congelado, sin saltos visuales
        if (!isFrozen && canChase) bobTime += dt;
        transform.position = basePosition + Vector3.up * (Mathf.Sin(bobTime * bobSpeed) * bobAmplitude);

        UpdateVisual(toPlayer.x, dt);
        TryHit(playerPos);
    }

    void UpdateVisual(float directionX, float dt)
    {
        if (spriteRenderer == null) return;

        alpha = Mathf.MoveTowards(alpha, isFrozen ? frozenAlpha : baseAlpha, alphaSpeed * dt);
        Color c = spriteRenderer.color;
        c.a = alpha;
        spriteRenderer.color = c;

        if (Mathf.Abs(directionX) > 0.05f)
        {
            bool lookRight = directionX > 0f;
            spriteRenderer.flipX = spriteFacesLeft ? lookRight : !lookRight;
        }
    }

    void TryHit(Vector2 playerPos)
    {
        if (hitTimer > 0f) return;
        if (Vector2.Distance(transform.position, playerPos) > hitRadius) return;

        hitTimer = hitCooldown;
        xolotlLight.Drain(lightDamage);

        if (xolotlController != null)
        {
            float side = playerPos.x >= transform.position.x ? 1f : -1f;
            xolotlController.Knockback(new Vector2(knockbackForce.x * side, knockbackForce.y));
        }

        if (hitSfx != null) AudioSource.PlayClipAtPoint(hitSfx, transform.position, sfxVolume);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.6f, 0.8f, 1f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
