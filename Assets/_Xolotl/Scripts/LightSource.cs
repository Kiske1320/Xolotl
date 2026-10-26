using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Clase base de cualquier luz que cuenta para el juego.
/// El radio de juego (Radius) es el que usan olvidados, puentes y veladoras.
/// </summary>
public class LightSource : MonoBehaviour
{
    [Header("Luz base")]
    [SerializeField] protected Light2D light2D;
    [SerializeField] protected float radius = 6f;

    public float Radius => radius;

    protected virtual void OnEnable() => LightRegistry.Add(this);
    protected virtual void OnDisable() => LightRegistry.Remove(this);

    protected virtual void Start() => SyncLight();

    protected void SyncLight()
    {
        if (light2D != null) light2D.pointLightOuterRadius = radius;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.62f, 0.1f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
