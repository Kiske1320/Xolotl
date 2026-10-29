using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Un ParticleSystem reutilizable por escena para ráfagas (pétalos, llamarada, chispas).
/// Va en un objeto con ParticleSystem (Simulation Space: World, Play On Awake apagado,
/// Emission sin Rate). Se registra con un nombre y cualquier script lo usa así:
///   ParticleBurst.Play("petalo", posicion);
/// No crea ni destruye objetos: mueve el sistema y llama Emit, ideal para WebGL.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class ParticleBurst : MonoBehaviour
{
    static readonly Dictionary<string, ParticleBurst> bursts = new();

    [SerializeField] string id = "petalo";
    [SerializeField] int count = 12;

    ParticleSystem ps;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatic() => bursts.Clear();

    void Awake() => ps = GetComponent<ParticleSystem>();
    void OnEnable() => bursts[id] = this;

    void OnDisable()
    {
        if (bursts.TryGetValue(id, out var b) && b == this) bursts.Remove(id);
    }

    public static void Play(string id, Vector3 position, int countOverride = -1)
    {
        if (!bursts.TryGetValue(id, out var b) || b == null) return;
        b.transform.position = position;
        b.ps.Emit(countOverride > 0 ? countOverride : b.count);
    }
}
