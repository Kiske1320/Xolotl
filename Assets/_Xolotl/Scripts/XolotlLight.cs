using System;
using UnityEngine;

/// <summary>
/// El fuego de Xólotl: se consume con el tiempo, se recarga con pétalos
/// y se gasta en la llamarada. Si llega a cero, dispara OnLightEmpty (derrota).
/// Va en el GameObject raíz de Xólotl; el Light2D puede ser un hijo.
/// </summary>
public class XolotlLight : LightSource
{
    [Header("Combustible de luz")]
    [SerializeField] float maxLight = 6f;
    [SerializeField] float minVisualRadius = 0.6f;
    [SerializeField] float drainPerSecond = 0.15f;

    [Header("Llamarada")]
    [SerializeField] float flareCost = 1.5f;
    [SerializeField] float flareRadius = 9f;
    [SerializeField] float flareDuration = 1f;
    [SerializeField] float flareCooldown = 3f;

    [Header("Parpadeo con poca luz")]
    [SerializeField] float lowLightThreshold = 1.5f;
    [SerializeField] float flickerAmount = 0.15f;
    [SerializeField] float flickerSpeed = 18f;

    public event Action OnLightEmpty;
    public event Action OnFlare;
    public event Action<float> OnLightChanged; // valor normalizado 0..1 para la UI

    float currentLight;
    float flareTimer;
    float cooldownTimer;
    bool isEmpty;
    bool drainPaused;

    public float LightNormalized => maxLight > 0f ? currentLight / maxLight : 0f;
    public bool IsFlaring => flareTimer > 0f;
    public bool IsEmpty => isEmpty;
    public bool CanFlare => cooldownTimer <= 0f && currentLight > flareCost && !isEmpty;

    protected override void Start()
    {
        currentLight = maxLight;
        ApplyRadius();
        OnLightChanged?.Invoke(LightNormalized);
    }

    void Update()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
        if (flareTimer > 0f) flareTimer -= Time.deltaTime;

        if (!isEmpty && !drainPaused)
        {
            currentLight -= drainPerSecond * Time.deltaTime;
            OnLightChanged?.Invoke(LightNormalized);
            if (currentLight <= 0f) Extinguish();
        }

        ApplyRadius();
    }

    void ApplyRadius()
    {
        float gameplayRadius;
        if (isEmpty) gameplayRadius = 0f;
        else if (IsFlaring) gameplayRadius = flareRadius;
        else gameplayRadius = Mathf.Max(currentLight, minVisualRadius);

        radius = gameplayRadius;

        float visualRadius = gameplayRadius;
        if (!isEmpty && !IsFlaring && currentLight < lowLightThreshold)
            visualRadius += Mathf.Sin(Time.time * flickerSpeed) * flickerAmount;

        if (light2D != null) light2D.pointLightOuterRadius = Mathf.Max(0f, visualRadius);
    }

    void Extinguish()
    {
        currentLight = 0f;
        isEmpty = true;
        flareTimer = 0f;
        OnLightChanged?.Invoke(0f);
        OnLightEmpty?.Invoke();
    }

    /// <summary>Pétalos y otras recargas.</summary>
    public void AddLight(float amount)
    {
        if (isEmpty) return;
        currentLight = Mathf.Min(currentLight + amount, maxLight);
        OnLightChanged?.Invoke(LightNormalized);
    }

    /// <summary>Daño de un olvidado.</summary>
    public void Drain(float amount)
    {
        if (isEmpty) return;
        currentLight -= amount;
        OnLightChanged?.Invoke(LightNormalized);
        if (currentLight <= 0f) Extinguish();
    }

    /// <summary>La llama XolotlController al presionar el botón. Devuelve true si se ejecutó.</summary>
    public bool TryFlare()
    {
        if (!CanFlare) return false;
        currentLight -= flareCost;
        flareTimer = flareDuration;
        cooldownTimer = flareCooldown;
        OnLightChanged?.Invoke(LightNormalized);
        OnFlare?.Invoke();
        return true;
    }

    /// <summary>Reaparición en una veladora (por ejemplo con 0.5 = 50% de luz).</summary>
    public void Refill(float fraction)
    {
        currentLight = maxLight * Mathf.Clamp01(fraction);
        isEmpty = false;
        flareTimer = 0f;
        cooldownTimer = 0f;
        OnLightChanged?.Invoke(LightNormalized);
        ApplyRadius();
    }

    /// <summary>Para la escena final o cinemáticas.</summary>
    public void SetDrainPaused(bool paused) => drainPaused = paused;
}
