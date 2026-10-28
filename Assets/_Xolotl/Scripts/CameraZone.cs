using UnityEngine;

/// <summary>
/// Zona de cámara para partes bajas (cripta, sótano): mientras Xólotl está dentro,
/// la cámara usa estos límites verticales; al salir, vuelve a los del nivel.
/// Así la superficie se ve con el piso abajo y las zonas subterráneas se ven completas.
/// Necesita un BoxCollider2D en Trigger que cubra toda la zona baja (de entrada a salida).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CameraZone : MonoBehaviour
{
    [SerializeField] float zoneMinY = -6f;
    [SerializeField] float zoneMaxY = 4f;

    CameraFollow cameraFollow;

    void Reset() => GetComponent<Collider2D>().isTrigger = true;

    void Awake() => cameraFollow = FindAnyObjectByType<CameraFollow>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (cameraFollow == null || !IsXolotl(other)) return;
        cameraFollow.SetVerticalBounds(this, zoneMinY, zoneMaxY);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (cameraFollow == null || !IsXolotl(other)) return;
        cameraFollow.ClearVerticalBounds(this);
    }

    static bool IsXolotl(Collider2D other) =>
        other.attachedRigidbody != null && other.attachedRigidbody.GetComponent<XolotlLight>() != null;
}
