using System.Collections;
using UnityEngine;

/// <summary>Reproductor persistente con dos fuentes para hacer crossfade.</summary>
public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance;
    AudioSource a, b, ambience;
    AudioSource current;
    Coroutine fade;

    public static MusicPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("MusicPlayer");
                DontDestroyOnLoad(go);
                instance = go.AddComponent<MusicPlayer>();
            }
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatic() => instance = null;

    void Awake()
    {
        a = CreateSource();
        b = CreateSource();
        ambience = CreateSource();
        current = a;
    }

    AudioSource CreateSource()
    {
        var s = gameObject.AddComponent<AudioSource>();
        s.loop = true;
        s.playOnAwake = false;
        s.spatialBlend = 0f;
        s.volume = 0f;
        return s;
    }

    public void PlayMusic(AudioClip clip, float volume, float time)
    {
        if (clip == null)
        {
            StartFade(null, 0f, time);
            return;
        }
        if (current.clip == clip && current.isPlaying)
        {
            StartFade(current, volume, time);   // misma canción: solo ajusta el volumen
            return;
        }
        AudioSource incoming = current == a ? b : a;
        incoming.clip = clip;
        incoming.volume = 0f;
        incoming.Play();
        current = incoming;
        StartFade(incoming, volume, time);
    }

    public void PlayAmbience(AudioClip clip, float volume)
    {
        if (clip == null) { ambience.Stop(); ambience.clip = null; return; }
        if (ambience.clip != clip) { ambience.clip = clip; ambience.Play(); }
        ambience.volume = volume;
    }

    void StartFade(AudioSource target, float volume, float time)
    {
        if (fade != null) StopCoroutine(fade);
        fade = StartCoroutine(Crossfade(target, volume, time));
    }

    IEnumerator Crossfade(AudioSource target, float targetVolume, float time)
    {
        AudioSource other = target == a ? b : a;
        float startTarget = target != null ? target.volume : 0f;
        float startOther = other.volume;
        float startA = a.volume, startB = b.volume;
        float t = 0f;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / time);
            if (target != null)
            {
                target.volume = Mathf.Lerp(startTarget, targetVolume, k);
                other.volume = Mathf.Lerp(startOther, 0f, k);
            }
            else
            {
                a.volume = Mathf.Lerp(startA, 0f, k);
                b.volume = Mathf.Lerp(startB, 0f, k);
            }
            yield return null;
        }
        if (target != null)
        {
            target.volume = targetVolume;
            other.volume = 0f;
            other.Stop();
        }
        else
        {
            a.Stop(); b.Stop();
        }
        fade = null;
    }
}
