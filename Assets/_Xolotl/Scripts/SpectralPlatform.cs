using UnityEngine;

/// <summary>
/// Puente de papel picado: solo es sólido y visible bajo la luz.
/// Por defecto la luz normal de Xólotl NO basta: lo hacen sólido la vela de un vecino,
/// una veladora o la llamarada. Así el jugador sigue a los vivos o gasta luz para cruzar.
/// Al perder la luz sigue sólido un momento (solidLingerTime) para no tirar al jugador de golpe.
/// Revisa tres puntos (izquierda, centro y derecha). Mantenlo corto, de 1 a 4 unidades.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SpectralPlatform : MonoBehaviour
{
    [Header("Luz")]
    [Tooltip("Si está activo, la luz normal de Xólotl no basta: necesita una vela, una veladora o la llamarada")]
    [SerializeField] bool requireExternalLight = true;
    [SerializeField] float solidLingerTime = 0.8f;

    [Header("Visual")]
    [SerializeField] SpriteRenderer[] renderers;
    [Range(0f, 1f)][SerializeField] float hiddenAlpha = 0.15f;
    [Range(0f, 1f)][SerializeField] float fadingAlpha = 0.55f;
    [SerializeField] float fadeSpeed = 4f;
    [Tooltip("Distancia hacia adentro desde cada borde para los puntos de revisión")]
    [SerializeField] float edgeInset = 0.2f;

    Collider2D col;
    XolotlLight xolotl;
    Vector2 localLeft, localCenter, localRight;
    float alpha = 1f;
    float lingerTimer;

    public bool IsSolid => col != null && col.enabled;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        xolotl = FindAnyObjectByType<XolotlLight>();
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<SpriteRenderer>();

        if (col is BoxCollider2D box)
        {
            float half = Mathf.Max(box.size.x * 0.5f - edgeInset, 0f);
            localCenter = box.offset;
            localLeft = box.offset + Vector2.left * half;
            localRight = box.offset + Vector2.right * half;
        }
        else
        {
            Bounds b = col.bounds;
            localCenter = transform.InverseTransformPoint(b.center);
            localLeft = transform.InverseTransformPoint(new Vector3(b.min.x + edgeInset, b.center.y));
            localRight = transform.InverseTransformPoint(new Vector3(b.max.x - edgeInset, b.center.y));
        }
    }

    void Update()
    {
        bool lit = IsPointLit(localCenter) || IsPointLit(localLeft) || IsPointLit(localRight);

        if (lit) lingerTimer = solidLingerTime;
        else if (lingerTimer > 0f) lingerTimer -= Time.deltaTime;

        bool solid = lit || lingerTimer > 0f;
        col.enabled = solid;

        float targetAlpha = lit ? 1f : (solid ? fadingAlpha : hiddenAlpha);
        alpha = Mathf.MoveTowards(alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null) continue;
            Color c = renderers[i].color;
            c.a = alpha;
            renderers[i].color = c;
        }
    }

    bool IsPointLit(Vector2 localPoint)
    {
        LightSource ignore = requireExternalLight && xolotl != null && !xolotl.IsFlaring ? xolotl : null;
        return LightRegistry.IsLit(transform.TransformPoint(localPoint), ignore);
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.TransformPoint(localLeft), 0.1f);
        Gizmos.DrawWireSphere(transform.TransformPoint(localCenter), 0.1f);
        Gizmos.DrawWireSphere(transform.TransformPoint(localRight), 0.1f);
    }
}
