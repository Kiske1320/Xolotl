using System.Collections;
using UnityEngine;

/// <summary>
/// Fundido a negro de pantalla completa. Al iniciar la escena aparece desde negro.
/// Otros scripts (LevelExit, la escena final) llaman FadeOut antes de cambiar de escena.
/// Va en un Panel negro a pantalla completa con CanvasGroup, en el Canvas de la escena,
/// encima de todo lo demás.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] float fadeInDuration = 0.8f;

    CanvasGroup canvasGroup;

    void Awake()
    {
        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    void Start() => StartCoroutine(Fade(1f, 0f, fadeInDuration));

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public IEnumerator FadeOut(float duration) => Fade(canvasGroup.alpha, 1f, duration);

    IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
