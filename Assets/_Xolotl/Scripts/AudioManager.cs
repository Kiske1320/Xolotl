using UnityEngine;

/// <summary>
/// Efectos de sonido 2D desde cualquier script: AudioManager.Play(clip).
/// Se crea solo la primera vez que se usa y sobrevive a los cambios de escena.
/// Usa un pequeño grupo de AudioSources para que varios sonidos suenen a la vez.
/// Variación de tono aleatoria (±5 % por defecto) para que no suene repetitivo.
/// El volumen general lo controla AudioListener.volume (slider de la pausa).
/// </summary>
public class AudioManager : MonoBehaviour
{
    const int PoolSize = 10;

    static AudioManager instance;
    AudioSource[] pool;
    int next;

    static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("AudioManager");
                DontDestroyOnLoad(go);
                instance = go.AddComponent<AudioManager>();
            }
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatic() => instance = null;

    void Awake()
    {
        pool = new AudioSource[PoolSize];
        for (int i = 0; i < PoolSize; i++)
        {
            pool[i] = gameObject.AddComponent<AudioSource>();
            pool[i].playOnAwake = false;
            pool[i].spatialBlend = 0f;
        }
    }

    /// <summary>Reproduce un efecto una vez.</summary>
    public static void Play(AudioClip clip, float volume = 1f, float pitchVariation = 0.05f)
    {
        if (clip == null) return;
        Instance.PlayInternal(clip, volume, pitchVariation);
    }

    /// <summary>Reproduce uno de varios clips al azar (por ejemplo petalo_1, 2, 3).</summary>
    public static void PlayRandom(AudioClip[] clips, float volume = 1f, float pitchVariation = 0.05f)
    {
        if (clips == null || clips.Length == 0) return;
        Play(clips[Random.Range(0, clips.Length)], volume, pitchVariation);
    }

    void PlayInternal(AudioClip clip, float volume, float pitchVariation)
    {
        AudioSource source = pool[next];
        next = (next + 1) % PoolSize;
        source.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        source.PlayOneShot(clip, volume);
    }
}
