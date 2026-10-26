using UnityEngine;

/// <summary>
/// Pétalo de cempasúchil de los caminos que ponen los vivos. Recarga la luz de Xólotl.
/// Necesita un Collider2D marcado como Trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Petal : MonoBehaviour
{
    [SerializeField] float lightAmount = 1.5f;
    [SerializeField] ParticleSystem pickupFxPrefab;
    [SerializeField] AudioClip pickupSfx;
    [Range(0f, 1f)][SerializeField] float sfxVolume = 0.7f;

    [Header("Flotar")]
    [SerializeField] float bobAmplitude = 0.08f;
    [SerializeField] float bobSpeed = 3f;

    Vector3 startPos;
    bool collected;

    void Reset() => GetComponent<Collider2D>().isTrigger = true;

    void Start()
    {
        startPos = transform.position;
        // Desfase para que no floten todos igual
        bobSpeed += Random.Range(-0.5f, 0.5f);
    }

    void Update()
    {
        transform.position = startPos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobAmplitude);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || other.attachedRigidbody == null) return;

        XolotlLight xolotlLight = other.attachedRigidbody.GetComponent<XolotlLight>();
        if (xolotlLight == null || xolotlLight.IsEmpty) return;

        collected = true;
        xolotlLight.AddLight(lightAmount);

        if (pickupFxPrefab != null)
            Instantiate(pickupFxPrefab, transform.position, Quaternion.identity);
        if (pickupSfx != null)
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position, sfxVolume);

        Destroy(gameObject);
    }
}
