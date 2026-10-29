using UnityEngine;

/// <summary>
/// Sacudida de cámara. Va en la MainCamera, junto a CameraFollow.
/// Uso desde cualquier script: CameraShake.Shake(0.15f, 0.2f);
/// No pelea con CameraFollow: en Update quita el desplazamiento del cuadro anterior
/// (antes de que CameraFollow calcule) y en LateUpdate, después de CameraFollow, agrega el nuevo.
/// </summary>
[DefaultExecutionOrder(1000)]
public class CameraShake : MonoBehaviour
{
    static CameraShake instance;

    [Tooltip("Multiplicador global; 0 apaga todas las sacudidas")]
    [Range(0f, 2f)][SerializeField] float globalStrength = 1f;
    [SerializeField] float frequency = 30f;

    float strength;
    float duration;
    float timer;
    Vector3 lastOffset;
    float seed;

    void Awake()
    {
        instance = this;
        seed = Random.value * 100f;
    }

    void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    /// <summary>Fuerza en unidades del mundo (1 píxel = 0.0625). Usa de 0.1 a 0.3.</summary>
    public static void Shake(float strength, float duration)
    {
        if (instance == null) return;
        // Si ya hay una sacudida más fuerte en curso, no la reemplaza
        if (instance.timer > 0f && instance.strength * (instance.timer / instance.duration) > strength) return;
        instance.strength = strength * instance.globalStrength;
        instance.duration = Mathf.Max(0.01f, duration);
        instance.timer = instance.duration;
    }

    void Update()
    {
        transform.position -= lastOffset;
        lastOffset = Vector3.zero;
    }

    void LateUpdate()
    {
        if (timer <= 0f) return;
        timer -= Time.unscaledDeltaTime;
        float fade = Mathf.Clamp01(timer / duration);
        float t = Time.unscaledTime * frequency;
        float x = (Mathf.PerlinNoise(seed, t) * 2f - 1f) * strength * fade;
        float y = (Mathf.PerlinNoise(seed + 50f, t) * 2f - 1f) * strength * fade;
        lastOffset = new Vector3(x, y, 0f);
        transform.position += lastOffset;
    }
}
