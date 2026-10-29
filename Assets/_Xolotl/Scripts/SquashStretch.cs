using UnityEngine;

/// <summary>
/// Aplastar al aterrizar y estirar al saltar. Va en el objeto HIJO que tiene el
/// SpriteRenderer de Xólotl (nunca en la raíz, para no escalar el collider).
/// Compensa la posición según el pivote del sprite para que las patas no floten.
/// </summary>
public class SquashStretch : MonoBehaviour
{
    [SerializeField] XolotlController controller;
    [SerializeField] Rigidbody2D body;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Aterrizaje")]
    [SerializeField] Vector2 landScale = new(1.25f, 0.78f);
    [SerializeField] float minFallSpeed = 6f;

    [Header("Salto")]
    [SerializeField] Vector2 jumpScale = new(0.85f, 1.18f);
    [SerializeField] float minJumpSpeed = 4f;

    [SerializeField] float returnSpeed = 10f;

    Vector3 baseLocalPos;
    Vector2 scale = Vector2.one;
    bool wasGrounded = true;
    float lastVelY;

    void Awake()
    {
        if (controller == null) controller = GetComponentInParent<XolotlController>();
        if (body == null) body = GetComponentInParent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        baseLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        if (controller == null || body == null) return;

        bool grounded = controller.IsGrounded;
        float vy = body.linearVelocity.y;

        if (grounded && !wasGrounded && lastVelY < -minFallSpeed) scale = landScale;
        else if (!grounded && wasGrounded && vy > minJumpSpeed) scale = jumpScale;

        wasGrounded = grounded;
        lastVelY = vy;

        scale = Vector2.Lerp(scale, Vector2.one, returnSpeed * Time.deltaTime);
        transform.localScale = new Vector3(scale.x, scale.y, 1f);

        // Mantiene las patas en el piso aunque el pivote del sprite esté al centro
        float pivotN = 0f, height = 0f;
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            Sprite s = spriteRenderer.sprite;
            pivotN = s.pivot.y / s.rect.height;
            height = s.bounds.size.y;
        }
        transform.localPosition = baseLocalPos - new Vector3(0f, (1f - scale.y) * height * pivotN, 0f);
    }
}
