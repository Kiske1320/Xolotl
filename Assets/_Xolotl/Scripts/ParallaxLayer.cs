using UnityEngine;

/// <summary>
/// Capa de fondo con parallax: se mueve más lento que la cámara para dar profundidad.
/// factor 0 = se mueve igual que el mundo (no hay parallax); factor 1 = se queda pegada a la cámara.
/// Los fondos lejanos usan factores altos (0.8 a 0.95) y los cercanos bajos (0.3 a 0.6).
/// Usa un SpriteRenderer en modo Tiled lo bastante ancho para cubrir todo el recorrido.
/// </summary>
public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [Range(0f, 1f)][SerializeField] float factorX = 0.85f;
    [Range(0f, 1f)][SerializeField] float factorY = 0.9f;

    Vector3 startPosition;
    Vector3 cameraStart;
    bool iniciado;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
        startPosition = transform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;
        // Se toma la cámara en el primer LateUpdate: para entonces CameraFollow ya la colocó en su Start.
        if (!iniciado)
        {
            cameraStart = cameraTransform.position;
            iniciado = true;
        }
        Vector3 delta = cameraTransform.position - cameraStart;
        transform.position = new Vector3(
            startPosition.x + delta.x * factorX,
            startPosition.y + delta.y * factorY,
            startPosition.z);
    }
}
