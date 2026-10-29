using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Foto de recuerdo de la abuela, escondida en caminos opcionales.
/// Al tomarla da un poco de luz y cuenta para el final (3 por nivel, 9 en total).
/// Las fotos tomadas se recuerdan entre escenas y no reaparecen al morir.
/// Necesita un Collider2D en Trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class MemoryPhoto : MonoBehaviour
{
    [Tooltip("Identificador único. Si se deja vacío usa escena/nombre del objeto.")]
    [SerializeField] string photoId;
    [SerializeField] float lightBonus = 1f;
    [SerializeField] ParticleSystem pickupFxPrefab;
    [SerializeField] AudioClip pickupSfx;
    [Range(0f, 1f)][SerializeField] float sfxVolume = 0.8f;

    [Header("Flotar")]
    [SerializeField] float bobAmplitude = 0.12f;
    [SerializeField] float bobSpeed = 2f;

    static readonly HashSet<string> collected = new();

    /// <summary>Fotos tomadas en toda la partida.</summary>
    public static int TotalCollected => collected.Count;

    /// <summary>Se dispara al tomar una foto, con el total de la partida.</summary>
    public static event Action<int> OnPhotoCollected;

    /// <summary>Para empezar una partida nueva desde el título.</summary>
    public static void ResetAll() => collected.Clear();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetOnPlay() => collected.Clear();

    string id;
    Vector3 startPos;

    public bool IsCollected => collected.Contains(Id);

    string Id
    {
        get
        {
            if (string.IsNullOrEmpty(id))
                id = string.IsNullOrEmpty(photoId) ? $"{SceneManager.GetActiveScene().name}/{name}" : photoId;
            return id;
        }
    }

    void Reset() => GetComponent<Collider2D>().isTrigger = true;

    void Start()
    {
        startPos = transform.position;
        if (IsCollected) gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = startPos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobAmplitude);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsCollected || other.attachedRigidbody == null) return;

        XolotlLight xolotlLight = other.attachedRigidbody.GetComponent<XolotlLight>();
        if (xolotlLight == null || xolotlLight.IsEmpty) return;

        collected.Add(Id);
        xolotlLight.AddLight(lightBonus);

        if (pickupFxPrefab != null)
            Instantiate(pickupFxPrefab, transform.position, Quaternion.identity);
        AudioManager.Play(pickupSfx, sfxVolume);

        OnPhotoCollected?.Invoke(TotalCollected);
        gameObject.SetActive(false);
    }
}
