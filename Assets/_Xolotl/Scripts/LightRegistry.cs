using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Registro global de todas las luces activas (Xólotl, veladoras, vecinos con vela).
/// Cualquier sistema pregunta IsLit(punto) para saber si algo está iluminado.
/// </summary>
public static class LightRegistry
{
    static readonly List<LightSource> sources = new();

    public static void Add(LightSource source)
    {
        if (!sources.Contains(source)) sources.Add(source);
    }

    public static void Remove(LightSource source) => sources.Remove(source);

    public static bool IsLit(Vector2 point)
    {
        for (int i = 0; i < sources.Count; i++)
        {
            LightSource s = sources[i];
            if (s.Radius <= 0f) continue;
            if (((Vector2)s.transform.position - point).sqrMagnitude <= s.Radius * s.Radius)
                return true;
        }
        return false;
    }

    // Se limpia al cambiar de escena o al entrar a Play con Domain Reload desactivado
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetRegistry() => sources.Clear();
}
