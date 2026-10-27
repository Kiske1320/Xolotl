using UnityEngine;

/// <summary>
/// Vacío debajo de los puentes: si Xólotl cae aquí, su fuego se apaga (derrota)
/// y reaparece en la última veladora. Necesita un BoxCollider2D en modo Trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class FallZone : MonoBehaviour
{
    void Reset() => GetComponent<Collider2D>().isTrigger = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == null) return;
        XolotlLight xolotlLight = other.attachedRigidbody.GetComponent<XolotlLight>();
        if (xolotlLight != null && !xolotlLight.IsEmpty)
            xolotlLight.Drain(9999f);
    }
}
