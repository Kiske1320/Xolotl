using UnityEngine;

/// <summary>
/// Veladora que pusieron los vivos. Encendida: da luz, protege y es checkpoint al tocarla.
/// Apagada (por el viento): no da luz; la llamarada de Xólotl la enciende si la alcanza.
/// Necesita un Collider2D en modo Trigger para detectar a Xólotl.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Veladora : LightSource
{
    [Header("Veladora")]
    [SerializeField] bool startLit = true;
    [SerializeField] float litRadius = 2.5f;
    [SerializeField] SpriteRenderer flameRenderer;
    [Tooltip("Dónde reaparece Xólotl. Si está vacío, usa la posición de la veladora.")]
    [SerializeField] Transform respawnPoint;

    [Header("Recarga")]
    [Tooltip("Luz por segundo que recupera Xólotl mientras está junto a una veladora encendida")]
    [SerializeField] float rechargePerSecond = 1.5f;

    [Header("Parpadeo de la flama")]
    [SerializeField] float flickerAmount = 0.12f;
    [SerializeField] float flickerSpeed = 7f;

    [Header("Efectos (opcionales)")]
    [SerializeField] ParticleSystem igniteFx;
    [SerializeField] AudioClip igniteSfx;
    [SerializeField] AudioClip checkpointSfx;

    bool isLit;
    float flickerOffset;
    XolotlLight xolotl;

    public bool IsLit => isLit;
    public Vector3 RespawnPosition => respawnPoint != null ? respawnPoint.position : transform.position;

    void Reset() => GetComponent<Collider2D>().isTrigger = true;

    protected override void Start()
    {
        xolotl = FindAnyObjectByType<XolotlLight>();
        flickerOffset = Random.Range(0f, 10f);
        SetLit(startLit, playEffects: false);
    }

    void Update()
    {
        if (isLit)
        {
            if (light2D != null)
                light2D.pointLightOuterRadius = litRadius + Mathf.Sin((Time.time + flickerOffset) * flickerSpeed) * flickerAmount;

            // Zona segura: junto a la veladora, el fuego de Xólotl se recupera
            if (xolotl != null && rechargePerSecond > 0f &&
                Vector2.Distance(xolotl.transform.position, transform.position) <= litRadius)
                xolotl.AddLight(rechargePerSecond * Time.deltaTime);
            return;
        }

        // Apagada: la llamarada de Xólotl la enciende si la veladora queda dentro de su radio
        if (xolotl != null && xolotl.IsFlaring)
        {
            float dist = Vector2.Distance(xolotl.transform.position, transform.position);
            if (dist <= xolotl.Radius) SetLit(true, playEffects: true);
        }
    }

    public void SetLit(bool lit, bool playEffects)
    {
        isLit = lit;
        radius = lit ? litRadius : 0f;

        if (light2D != null) light2D.enabled = lit;
        if (flameRenderer != null) flameRenderer.enabled = lit;

        if (lit && playEffects)
        {
            if (igniteFx != null) igniteFx.Play();
            if (igniteSfx != null) AudioSource.PlayClipAtPoint(igniteSfx, transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLit || other.attachedRigidbody == null) return;
        if (other.attachedRigidbody.GetComponent<XolotlLight>() == null) return;

        if (GameManager.Instance != null && GameManager.Instance.SetCheckpoint(this))
        {
            if (checkpointSfx != null) AudioSource.PlayClipAtPoint(checkpointSfx, transform.position);
        }
    }
}
