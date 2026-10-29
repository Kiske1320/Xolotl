using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Sonidos de un botón: hover al pasar el cursor o al seleccionarlo con teclado/control,
/// y click al presionarlo. Va en el mismo objeto que el Button.
/// En el botón "Volver" de los créditos, clickSfx es ui_atras.
/// </summary>
[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] AudioClip clickSfx;
    [Range(0f, 1f)][SerializeField] float clickVolume = 0.5f;
    [SerializeField] AudioClip hoverSfx;
    [Range(0f, 1f)][SerializeField] float hoverVolume = 0.3f;

    Button button;
    float enabledTime;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClick);
    }

    void OnDestroy() => button.onClick.RemoveListener(PlayClick);

    // Al abrir un panel se selecciona un botón solo; ese primer select no suena
    void OnEnable() => enabledTime = Time.unscaledTime;

    void PlayClick() => AudioManager.Play(clickSfx, clickVolume);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable) AudioManager.Play(hoverSfx, hoverVolume);
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Con el mouse ya sonó en OnPointerEnter
        if (eventData is PointerEventData) return;
        if (Time.unscaledTime - enabledTime < 0.1f) return;
        AudioManager.Play(hoverSfx, hoverVolume);
    }
}
