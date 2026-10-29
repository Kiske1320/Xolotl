using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Pantalla de introducción entre el título y el primer nivel.
/// Muestra la historia línea por línea con efecto de máquina de escribir.
/// Saltar (Jump): si la línea se está escribiendo, la completa; si ya está completa, pasa a la siguiente.
/// Pausa (Esc): se salta toda la intro.
/// Al terminar hace fundido a negro y carga el primer nivel.
/// </summary>
public class IntroScreen : MonoBehaviour
{
    [Header("Textos")]
    [SerializeField] TMP_Text storyText;
    [SerializeField] TMP_Text hintText;
    [TextArea(2, 4)]
    [SerializeField] string[] lines =
    {
        "Cada noche del 1 de noviembre, las almas cruzan del Mictlán para visitar a quienes las recuerdan.",
        "La abuela Aurora ya no encuentra el camino. Viaja en tu lomo, confiando en tu fuego.",
        "Eres Xólotl, el perro que guía a los muertos. Llévala hasta su ofrenda.",
        "Tu luz se apaga con el tiempo. Los pétalos de cempasúchil la avivan y las veladoras te protegen.",
        "En la oscuridad te acechan los olvidados, almas que ya nadie recuerda. La luz los detiene.",
        "Llega antes del amanecer."
    };

    [Header("Ritmo")]
    [SerializeField] float charsPerSecond = 40f;
    [SerializeField] float lineFadeTime = 0.35f;

    [Header("Controles")]
    [SerializeField] InputActionReference advanceAction;   // Jump
    [SerializeField] InputActionReference skipAction;      // Pause

    [Header("Salida")]
    [SerializeField] string nextScene = "01_Panteon";
    [SerializeField] float fadeOutDuration = 0.8f;
    [SerializeField] AudioSource typeSfx;                  // opcional

    int lineIndex;
    bool typing;
    bool finishing;

    void OnEnable()
    {
        advanceAction?.action.Enable();
        skipAction?.action.Enable();
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (hintText != null)
        {
            string key = advanceAction != null ? advanceAction.action.GetBindingDisplayString(0) : "Espacio";
            hintText.text = $"[{key}] continuar     [Esc] saltar";
        }
        StartCoroutine(ShowLine(0));
    }

    void Update()
    {
        if (finishing) return;

        if (skipAction != null && skipAction.action.WasPressedThisFrame())
        {
            StartCoroutine(Finish());
            return;
        }

        if (advanceAction != null && advanceAction.action.WasPressedThisFrame())
        {
            if (typing)
            {
                typing = false; // la corrutina completa la línea
            }
            else if (lineIndex < lines.Length - 1)
            {
                StartCoroutine(ShowLine(lineIndex + 1));
            }
            else
            {
                StartCoroutine(Finish());
            }
        }
    }

    IEnumerator ShowLine(int index)
    {
        lineIndex = index;
        typing = true;

        // Desvanece la línea anterior
        if (index > 0) yield return FadeText(1f, 0f);

        storyText.text = lines[index];
        storyText.maxVisibleCharacters = 0;
        storyText.alpha = 1f;
        storyText.ForceMeshUpdate();
        int total = storyText.textInfo.characterCount;

        float shown = 0f;
        while (typing && shown < total)
        {
            shown += charsPerSecond * Time.unscaledDeltaTime;
            int visible = Mathf.Min(total, Mathf.FloorToInt(shown));
            if (visible > storyText.maxVisibleCharacters && typeSfx != null && visible % 3 == 0)
                typeSfx.Play();
            storyText.maxVisibleCharacters = visible;
            yield return null;
        }

        storyText.maxVisibleCharacters = total;
        typing = false;
    }

    IEnumerator FadeText(float from, float to)
    {
        float t = 0f;
        while (t < lineFadeTime)
        {
            t += Time.unscaledDeltaTime;
            storyText.alpha = Mathf.Lerp(from, to, t / lineFadeTime);
            yield return null;
        }
        storyText.alpha = to;
    }

    IEnumerator Finish()
    {
        finishing = true;
        if (ScreenFader.Instance != null) yield return ScreenFader.Instance.FadeOut(fadeOutDuration);
        if (Application.CanStreamedLevelBeLoaded(nextScene)) SceneManager.LoadScene(nextScene);
        else Debug.LogError($"IntroScreen: la escena '{nextScene}' no está en Build Settings");
    }
}
