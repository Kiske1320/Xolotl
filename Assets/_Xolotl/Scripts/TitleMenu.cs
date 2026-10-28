using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pantalla de título: Jugar (empieza una partida nueva) y Créditos.
/// En navegador el audio solo suena después de que el jugador hace clic, así que
/// el primer clic en "Jugar" también desbloquea el sonido.
/// </summary>
public class TitleMenu : MonoBehaviour
{
    [SerializeField] string firstLevel = "01_Panteon";
    [SerializeField] GameObject creditsPanel;

    bool loading;

    void Start()
    {
        Time.timeScale = 1f;
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    /// <summary>Botón "Jugar".</summary>
    public void Play()
    {
        if (loading) return;
        loading = true;
        MemoryPhoto.ResetAll();
        StartCoroutine(LoadFirstLevel());
    }

    /// <summary>Botones "Créditos" (true) y "Volver" del panel de créditos (false).</summary>
    public void ShowCredits(bool show)
    {
        if (creditsPanel != null) creditsPanel.SetActive(show);
    }

    IEnumerator LoadFirstLevel()
    {
        if (ScreenFader.Instance != null) yield return ScreenFader.Instance.FadeOut(1f);
        SceneManager.LoadScene(firstLevel);
    }
}
