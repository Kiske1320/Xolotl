using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Menú de pausa: Continuar, Reiniciar nivel, volumen general y Salir al título.
/// Se abre con la acción Pause (Escape, P o Start del control). Va en el UI_Canvas.
/// Nota: en navegador, Escape también saca de pantalla completa; por eso P también pausa.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] InputActionReference pauseAction;
    [SerializeField] GameObject pausePanel;
    [SerializeField] Slider volumeSlider;
    [SerializeField] string titleScene = "00_Titulo";

    static float savedVolume = 1f;
    bool paused;

    public bool IsPaused => paused;

    void OnEnable()
    {
        if (pauseAction != null) pauseAction.action.Enable();
    }

    void Start()
    {
        AudioListener.volume = savedVolume;
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        SetPaused(false);
    }

    void Update()
    {
        if (pauseAction != null && pauseAction.action.WasPressedThisFrame())
            SetPaused(!paused);
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
        if (volumeSlider != null) volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }

    void SetPaused(bool value)
    {
        paused = value;
        Time.timeScale = paused ? 0f : 1f;
        if (pausePanel != null) pausePanel.SetActive(paused);
    }

    void SetVolume(float value)
    {
        savedVolume = value;
        AudioListener.volume = value;
    }

    /// <summary>Botón "Continuar".</summary>
    public void Resume() => SetPaused(false);

    /// <summary>Botón "Reiniciar nivel".</summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Botón "Salir al título".</summary>
    public void QuitToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleScene);
    }
}
