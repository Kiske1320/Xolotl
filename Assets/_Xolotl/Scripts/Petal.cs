using UnityEngine;

/// <summary>
/// Pétalo de cempasúchil de los caminos que ponen los vivos. Recarga la luz de Xólotl.
/// Al recogerlo se oculta (no se destruye) y vuelve a aparecer cuando Xólotl reaparece
/// en una veladora, para que el jugador nunca se quede sin forma de recargar.
/// Necesita un Collider2D marcado como Trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Petal : MonoBehaviour
{
    [SerializeField] float lightAmount = 1.5f;
    [SerializeField] ParticleSystem pickupFxPrefab;
    [Tooltip("Se elige uno al azar (petalo_1, 2, 3)")]
    [SerializeField] AudioClip[] pickupSfx;
    [Range(0f, 1f)][SerializeField] float sfxVolume = 0.6f;

    [Header("Flotar")]
    [SerializeField] float bobAmplitude = 0.08f;
    [SerializeField] float bobSpeed = 3f;

    Vector3 startPos;
    bool collected;
    Collider2D col;
    SpriteRenderer[] renderers;

    void Reset() => GetComponent<Collider2D>().isTrigger = true;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        GameManager.OnRespawn += Restore;
    }

    void OnDestroy() => GameManager.OnRespawn -= Restore;

    void Start()
    {
        startPos = transform.position;
        // Desfase para que no floten todos igual
        bobSpeed += Random.Range(-0.5f, 0.5f);
    }

    void Update()
    {
        if (collected) return;
        transform.position = startPos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobAmplitude);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || other.attachedRigidbody == null) return;

        XolotlLight xolotlLight = other.attachedRigidbody.GetComponent<XolotlLight>();
        if (xolotlLight == null || xolotlLight.IsEmpty) return;

        xolotlLight.AddLight(lightAmount);

        if (pickupFxPrefab != null)
            Instantiate(pickupFxPrefab, transform.position, Quaternion.identity);
        AudioManager.PlayRandom(pickupSfx, sfxVolume);

        SetCollected(true);
    }

    void Restore() => SetCollected(false);

    void SetCollected(bool value)
    {
        collected = value;
        col.enabled = !value;
        foreach (SpriteRenderer r in renderers)
            if (r != null) r.enabled = !value;
    }
}
