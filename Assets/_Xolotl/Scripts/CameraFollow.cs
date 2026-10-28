using UnityEngine;

/// <summary>
/// Cámara que sigue a Xólotl con suavizado, mira hacia adelante según su dirección
/// y no se sale de los límites del nivel. Basada en CameraLockY de DEAD ENCORE.
/// En vertical solo se ajusta cuando Xólotl aterriza o cae, para que los saltos no la muevan.
/// Va en la Main Camera, que debe estar en la raíz de la Hierarchy (no como hija de Xólotl).
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] XolotlController targetController;

    [Header("Seguimiento")]
    [SerializeField] float lookAhead = 2.5f;
    [SerializeField] float smoothTimeX = 0.25f;
    [SerializeField] float smoothTimeY = 0.35f;
    [SerializeField] float verticalOffset = 1.5f;
    [Tooltip("Si Xólotl cae más que esto por debajo del encuadre, la cámara lo sigue aunque no haya aterrizado")]
    [SerializeField] float fallThreshold = 1.5f;

    [Header("Límites del nivel (posición del centro de la cámara)")]
    [SerializeField] bool useBounds = true;
    [SerializeField] float minX = 0f;
    [SerializeField] float maxX = 50f;
    [SerializeField] float minY = 0f;
    [SerializeField] float maxY = 10f;

    float velocityX, velocityY;
    float targetY;
    float defaultMinY, defaultMaxY;
    Object currentZone;

    void Awake()
    {
        defaultMinY = minY;
        defaultMaxY = maxY;
    }

    /// <summary>La llama una CameraZone al entrar Xólotl: límites verticales solo para esa zona.</summary>
    public void SetVerticalBounds(Object zone, float zoneMinY, float zoneMaxY)
    {
        currentZone = zone;
        minY = zoneMinY;
        maxY = zoneMaxY;
    }

    /// <summary>La llama una CameraZone al salir Xólotl: regresa a los límites normales del nivel.</summary>
    public void ClearVerticalBounds(Object zone)
    {
        if (currentZone != zone) return;
        currentZone = null;
        minY = defaultMinY;
        maxY = defaultMaxY;
    }

    void Start()
    {
        if (target == null) return;
        if (targetController == null) targetController = target.GetComponent<XolotlController>();

        targetY = target.position.y + verticalOffset;
        Vector3 start = new(target.position.x, targetY, transform.position.z);
        transform.position = Clamp(start);
    }

    void LateUpdate()
    {
        if (target == null) return;

        float dir = targetController == null || targetController.FacingRight ? 1f : -1f;
        float desiredX = target.position.x + lookAhead * dir;

        float y = target.position.y + verticalOffset;
        bool grounded = targetController == null || targetController.IsGrounded;
        if (grounded || y < targetY - fallThreshold) targetY = y;

        Vector3 desired = Clamp(new Vector3(desiredX, targetY, transform.position.z));

        Vector3 pos = transform.position;
        pos.x = Mathf.SmoothDamp(pos.x, desired.x, ref velocityX, smoothTimeX);
        pos.y = Mathf.SmoothDamp(pos.y, desired.y, ref velocityY, smoothTimeY);
        transform.position = pos;
    }

    Vector3 Clamp(Vector3 p)
    {
        if (!useBounds) return p;
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.y = Mathf.Clamp(p.y, minY, maxY);
        return p;
    }

    /// <summary>Para reapariciones: coloca la cámara de golpe sobre Xólotl.</summary>
    public void SnapToTarget()
    {
        if (target == null) return;
        targetY = target.position.y + verticalOffset;
        transform.position = Clamp(new Vector3(target.position.x, targetY, transform.position.z));
        velocityX = velocityY = 0f;
    }

    void OnDrawGizmosSelected()
    {
        if (!useBounds) return;
        Gizmos.color = Color.cyan;
        Vector3 center = new((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, 0f);
        Vector3 size = new(maxX - minX, maxY - minY, 0f);
        Gizmos.DrawWireCube(center, size);
    }
}
