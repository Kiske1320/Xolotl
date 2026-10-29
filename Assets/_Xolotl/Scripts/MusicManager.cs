using System.Collections;
using UnityEngine;

/// <summary>
/// Música de la escena. Va en un objeto "Musica" de cada escena con su clip.
/// El reproductor es persistente: si la escena siguiente usa la misma canción
/// (título → intro), sigue sonando sin cortarse; si es otra, hace un crossfade.
/// Si hay una ChaseWall activa en la escena, cambia a chaseMusic y regresa
/// a la música normal cuando la marea se reinicia (al reaparecer).
/// Opcional: un loop de ambiente (viento) que suena debajo de la música.
/// </summary>
public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip music;
    [SerializeField] AudioClip chaseMusic;
    [Range(0f, 1f)][SerializeField] float volume = 0.45f;
    [SerializeField] float crossfadeTime = 1.2f;

    [Header("Ambiente (opcional)")]
    [SerializeField] AudioClip ambienceLoop;
    [Range(0f, 1f)][SerializeField] float ambienceVolume = 0.2f;

    ChaseWall chaseWall;
    bool chasing;

    void Start()
    {
        chaseWall = FindAnyObjectByType<ChaseWall>();
        MusicPlayer.Instance.PlayMusic(music, volume, crossfadeTime);
        MusicPlayer.Instance.PlayAmbience(ambienceLoop, ambienceVolume);
    }

    void Update()
    {
        if (chaseWall == null || chaseMusic == null) return;
        bool shouldChase = chaseWall.IsActive;
        if (shouldChase == chasing) return;
        chasing = shouldChase;
        MusicPlayer.Instance.PlayMusic(chasing ? chaseMusic : music, volume, chasing ? 0.4f : crossfadeTime);
    }
}
