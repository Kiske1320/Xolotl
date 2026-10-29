using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla la derrota y el reintento de cada nivel.
/// Si la luz de Xólotl llega a cero: se bloquean los controles, aparece la pantalla de derrota
/// y al presionar Saltar reaparece en la última veladora con parte de su luz.
/// Uno por escena.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /// <summary>Se dispara cuando Xólotl reaparece. Los pétalos se escuchan aquí para volver.</summary>
    public static event Action OnRespawn;

    [Header("Xólotl")]
    [SerializeField] XolotlLight xolotlLight;
    [SerializeField] XolotlController xolotlController;
    [Tooltip("Punto de inicio del nivel, antes de tocar cualquier veladora")]
    [SerializeField] Transform levelStart;

    [Header("Reintento")]
    [Range(0.1f, 1f)][SerializeField] float respawnLightFraction = 1f;
    [SerializeField] float defeatDelay = 1f;
    [SerializeField] InputActionReference retryAction; // usa la acción Jump

    [Header("Pantalla de derrota")]
    [SerializeField] CanvasGroup defeatPanel;
    [SerializeField] float fadeDuration = 0.6f;

    [Header("Sonido")]
    [SerializeField] AudioClip defeatSfx;
    [Range(0f, 1f)][SerializeField] float defeatVolume = 0.8f;
    [SerializeField] AudioClip respawnSfx;
    [Range(0f, 1f)][SerializeField] float respawnVolume = 0.6f;

    Vector3 checkpointPosition;
    Veladora currentCheckpoint;
    bool isDefeated;
    bool canRetry;
    Rigidbody2D xolotlRb;

    void Awake()
    {
        Instance = this;
        if (xolotlLight == null) xolotlLight = FindAnyObjectByType<XolotlLight>();
        if (xolotlController == null) xolotlController = FindAnyObjectByType<XolotlController>();
        xolotlRb = xolotlController.GetComponent<Rigidbody2D>();

        checkpointPosition = levelStart != null ? levelStart.position : xolotlController.transform.position;
        SetPanelVisible(0f);
    }

    void OnEnable()
    {
        if (xolotlLight != null) xolotlLight.OnLightEmpty += HandleLightEmpty;
        if (retryAction != null) retryAction.action.Enable();
    }

    void OnDisable()
    {
        if (xolotlLight != null) xolotlLight.OnLightEmpty -= HandleLightEmpty;
        if (Instance == this) Instance = null;
    }

    void Update()
    {
        if (canRetry && retryAction != null && retryAction.action.WasPressedThisFrame())
            StartCoroutine(Respawn());
    }

    /// <summary>La llama una veladora encendida al tocarla. Devuelve true si es un checkpoint nuevo.</summary>
    public bool SetCheckpoint(Veladora veladora)
    {
        if (veladora == currentCheckpoint) return false;
        currentCheckpoint = veladora;
        checkpointPosition = veladora.RespawnPosition;
        return true;
    }

    void HandleLightEmpty()
    {
        if (isDefeated) return;
        StartCoroutine(Defeat());
    }

    IEnumerator Defeat()
    {
        isDefeated = true;
        xolotlController.SetControlsEnabled(false);
        AudioManager.Play(defeatSfx, defeatVolume);
        CameraShake.Shake(0.25f, 0.35f);

        yield return new WaitForSeconds(defeatDelay);
        yield return Fade(0f, 1f);
        canRetry = true;
    }

    IEnumerator Respawn()
    {
        canRetry = false;

        xolotlRb.linearVelocity = Vector2.zero;
        xolotlRb.position = checkpointPosition;
        xolotlController.transform.position = checkpointPosition;

        xolotlLight.Refill(respawnLightFraction);
        OnRespawn?.Invoke();
        AudioManager.Play(respawnSfx, respawnVolume);

        CameraFollow cam = FindAnyObjectByType<CameraFollow>();
        if (cam != null) cam.SnapToTarget();

        yield return Fade(1f, 0f);

        xolotlController.SetControlsEnabled(true);
        isDefeated = false;
    }

    IEnumerator Fade(float from, float to)
    {
        if (defeatPanel == null) yield break;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetPanelVisible(Mathf.Lerp(from, to, t / fadeDuration));
            yield return null;
        }
        SetPanelVisible(to);
    }

    void SetPanelVisible(float alpha)
    {
        if (defeatPanel == null) return;
        defeatPanel.alpha = alpha;
        defeatPanel.blocksRaycasts = alpha > 0.5f;
        defeatPanel.interactable = alpha > 0.5f;
    }
}
