using UnityEngine;

/// <summary>
/// Solo visual: copia el volteo (flipX) de otro SpriteRenderer y refleja la posición X local,
/// para que el alma de Aurora mire hacia donde mira Xólotl y siga sentada sobre su lomo.
/// </summary>
public class SeguirVolteo : MonoBehaviour
{
    [SerializeField] SpriteRenderer target;
    [Tooltip("Actívalo si este sprite y el del objetivo miran hacia lados distintos en su arte original")]
    [SerializeField] bool invertir = false;

    SpriteRenderer spriteRenderer;
    float baseX;
    bool lastFlip;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseX = transform.localPosition.x;
        lastFlip = target != null && target.flipX;
        Apply(lastFlip);
    }

    void LateUpdate()
    {
        if (target == null || target.flipX == lastFlip) return;
        lastFlip = target.flipX;
        Apply(lastFlip);
    }

    void Apply(bool targetFlip)
    {
        // La posición base corresponde a cuando el objetivo no está volteado
        Vector3 p = transform.localPosition;
        p.x = targetFlip ? -baseX : baseX;
        transform.localPosition = p;
        if (spriteRenderer != null) spriteRenderer.flipX = targetFlip != invertir;
    }
}
