using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Escena final en la ofrenda: cuenta la llegada de la abuela con texto que se escribe solo.
/// La última línea cambia según cuántas fotos de recuerdo se encontraron (de 9).
/// Al terminar muestra el panel de fin con el botón para volver al título.
/// </summary>
public class FinalScene : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] TMP_Text storyText;
    [SerializeField] TMP_Text photoCountText;
    [SerializeField] CanvasGroup endPanel;

    [Header("Historia")]
    [TextArea(2, 4)]
    [SerializeField] string[] lines =
    {
        "Las veladoras se avivan.",
        "El pan de muerto todavía está tibio.",
        "La familia no puede verla, pero sabe que llegó.",
        "Aurora baja de tu lomo y acaricia tu cabeza."
    };
    [TextArea(2, 4)][SerializeField] string endingAll = "«Me trajiste todos mis recuerdos. Ahora sí estoy completa.»";
    [TextArea(2, 4)][SerializeField] string endingSome = "«Algunos recuerdos se quedaron en el camino, pero mi familia guarda el resto.»";
    [TextArea(2, 4)][SerializeField] string endingFew = "«Ya casi no recuerdo nada... pero mientras ellos me recuerden, aquí estaré.»";
    [SerializeField] int totalPhotos = 9;
    [SerializeField] int somePhotosThreshold = 4;

    [Header("Ritmo")]
    [SerializeField] float startDelay = 1.5f;
    [SerializeField] float charsPerSecond = 28f;
    [SerializeField] float lineHold = 2.2f;

    [Header("Navegación")]
    [SerializeField] string titleScene = "00_Titulo";

    bool loading;

    void Start()
    {
        if (storyText != null) storyText.text = "";
        if (photoCountText != null) photoCountText.text = "";
        SetEndPanel(0f);
        StartCoroutine(PlayStory());
    }

    IEnumerator PlayStory()
    {
        yield return new WaitForSecondsRealtime(startDelay);

        foreach (string line in lines)
            yield return TypeLine(line);

        int found = MemoryPhoto.TotalCollected;
        string ending = found >= totalPhotos ? endingAll
                      : found >= somePhotosThreshold ? endingSome
                      : endingFew;
        yield return TypeLine(ending);

        if (photoCountText != null)
            photoCountText.text = $"Recuerdos encontrados: {found}/{totalPhotos}";

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            SetEndPanel(t);
            yield return null;
        }
        SetEndPanel(1f);
    }

    IEnumerator TypeLine(string line)
    {
        if (storyText == null) yield break;

        storyText.text = line;
        storyText.maxVisibleCharacters = 0;
        float visible = 0f;
        while (visible < line.Length)
        {
            visible += charsPerSecond * Time.unscaledDeltaTime;
            storyText.maxVisibleCharacters = Mathf.FloorToInt(visible);
            yield return null;
        }
        storyText.maxVisibleCharacters = line.Length;
        yield return new WaitForSecondsRealtime(lineHold);
    }

    void SetEndPanel(float alpha)
    {
        if (endPanel == null) return;
        endPanel.alpha = alpha;
        endPanel.interactable = alpha >= 1f;
        endPanel.blocksRaycasts = alpha >= 1f;
    }

    /// <summary>Botón "Volver al inicio" del panel de fin.</summary>
    public void GoToTitle()
    {
        if (loading) return;
        loading = true;
        StartCoroutine(LoadScene(titleScene));
    }

    IEnumerator LoadScene(string sceneName)
    {
        if (ScreenFader.Instance != null) yield return ScreenFader.Instance.FadeOut(1f);
        SceneManager.LoadScene(sceneName);
    }
}
