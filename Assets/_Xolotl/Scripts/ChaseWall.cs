using UnityEngine;

/// <summary>
/// La marea de olvidados de la persecución final: una masa oscura que avanza hacia la derecha.
/// Se activa cuando Xólotl cruza triggerX. La luz NO la congela (son demasiados): si te alcanza,
/// se apaga tu fuego. Si te alejas demasiado, se acerca de golpe para mantener la tensión.
/// Al reaparecer, vuelve a su inicio y espera a que cruces triggerX otra vez.
/// El borde que atrapa es transform.position.x + catchOffset.
/// </summary>
public class ChaseWall : MonoBehaviour
{
    [Header("Activación")]
    [SerializeField] float triggerX = 10f;
    [SerializeField] float startDelay = 0.8f;

    [Header("Movimiento")]
    [SerializeField] float speed = 4.5f;
    [Tooltip("Distancia máxima a la que Xólotl puede adelantarse antes de que la marea lo alcance de golpe")]
    [SerializeField] float maxLead = 12f;
    [Tooltip("La marea se detiene aquí (antes de la salida del nivel)")]
    [SerializeField] float stopX = 150f;
    [SerializeField] float catchOffset = 0.5f;

    [Header("Efectos (opcionales)")]
    [SerializeField] AudioSource loopAudio;
    [Tooltip("Gemidos al azar mientras la marea está activa")]
    [SerializeField] AudioClip[] moanSfx;
    [Range(0f, 1f)][SerializeField] float moanVolume = 0.6f;
    [SerializeField] Vector2 moanInterval = new(1.5f, 3f);
    [SerializeField] float wobbleAmplitude = 0.2f;
    [SerializeField] float wobbleSpeed = 3f;

    XolotlLight xolotlLight;
    Vector3 startPosition;
    float baseX;
    float delayTimer;
    float moanTimer;
    bool active;

    public bool IsActive => active;

    void Awake()
    {
        xolotlLight = FindAnyObjectByType<XolotlLight>();
        startPosition = transform.position;
        baseX = startPosition.x;
        GameManager.OnRespawn += ResetWall;
    }

    void OnDestroy() => GameManager.OnRespawn -= ResetWall;

    void Update()
    {
        if (xolotlLight == null) return;
        float playerX = xolotlLight.transform.position.x;

        if (!active)
        {
            if (playerX >= triggerX)
            {
                active = true;
                delayTimer = startDelay;
                if (loopAudio != null) loopAudio.Play();
            }
            return;
        }

        if (xolotlLight.IsEmpty) return;

        moanTimer -= Time.deltaTime;
        if (moanTimer <= 0f)
        {
            moanTimer = Random.Range(moanInterval.x, moanInterval.y);
            AudioManager.PlayRandom(moanSfx, moanVolume);
        }

        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
        }
        else if (baseX < stopX)
        {
            baseX += speed * Time.deltaTime;
            if (playerX - baseX > maxLead) baseX = playerX - maxLead;
            baseX = Mathf.Min(baseX, stopX);
        }

        Vector3 pos = startPosition;
        pos.x = baseX;
        pos.y += Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmplitude;
        transform.position = pos;

        if (baseX + catchOffset >= playerX)
            xolotlLight.Drain(9999f);
    }

    void ResetWall()
    {
        active = false;
        moanTimer = 0f;
        baseX = startPosition.x;
        transform.position = startPosition;
        if (loopAudio != null) loopAudio.Stop();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(triggerX, -5f), new Vector3(triggerX, 15f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(stopX, -5f), new Vector3(stopX, 15f));
    }
}
