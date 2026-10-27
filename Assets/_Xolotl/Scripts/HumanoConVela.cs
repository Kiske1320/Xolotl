using UnityEngine;

/// <summary>
/// Vecino del pueblo que camina con su vela entre dos puntos.
/// No ve a Xólotl. Su luz congela a los olvidados y hace sólidos los puentes de papel picado,
/// así que el jugador aprovecha su ritmo para avanzar.
/// Importante: PointA y PointB NO deben ser hijos del vecino (se guardan al iniciar).
/// </summary>
public class HumanoConVela : LightSource
{
    [Header("Ruta")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float walkSpeed = 1.2f;
    [SerializeField] float waitAtEnds = 1.5f;
    [Tooltip("Retraso antes de empezar a caminar, para escalonar varios vecinos")]
    [SerializeField] float startDelay = 0f;

    [Header("Visual")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [Tooltip("Actívalo si el sprite original mira a la izquierda")]
    [SerializeField] bool spriteFacesLeft = false;
    [SerializeField] Animator animator;

    [Header("Parpadeo de la vela")]
    [SerializeField] float flickerAmount = 0.1f;
    [SerializeField] float flickerSpeed = 9f;

    static readonly int SpeedHash = Animator.StringToHash("Speed");

    float targetAX, targetBX;
    bool goingToB = true;
    float waitTimer;
    float flickerOffset;

    void Reset() => radius = 3f;

    protected override void Start()
    {
        base.Start();
        targetAX = pointA != null ? pointA.position.x : transform.position.x - 3f;
        targetBX = pointB != null ? pointB.position.x : transform.position.x + 3f;
        waitTimer = startDelay;
        flickerOffset = Random.Range(0f, 10f);
        FaceDirection(targetBX - transform.position.x);
    }

    void Update()
    {
        if (light2D != null)
            light2D.pointLightOuterRadius = radius + Mathf.Sin((Time.time + flickerOffset) * flickerSpeed) * flickerAmount;

        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            SetAnimSpeed(0f);
            return;
        }

        float targetX = goingToB ? targetBX : targetAX;
        Vector3 pos = transform.position;
        pos.x = Mathf.MoveTowards(pos.x, targetX, walkSpeed * Time.deltaTime);
        transform.position = pos;
        SetAnimSpeed(walkSpeed);

        if (Mathf.Approximately(pos.x, targetX))
        {
            goingToB = !goingToB;
            waitTimer = waitAtEnds;
            FaceDirection((goingToB ? targetBX : targetAX) - pos.x);
        }
    }

    void FaceDirection(float directionX)
    {
        if (spriteRenderer == null || Mathf.Abs(directionX) < 0.01f) return;
        bool lookRight = directionX > 0f;
        spriteRenderer.flipX = spriteFacesLeft ? lookRight : !lookRight;
    }

    void SetAnimSpeed(float speed)
    {
        if (animator != null) animator.SetFloat(SpeedHash, speed);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        if (pointA == null || pointB == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
