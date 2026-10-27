using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interfaz del nivel: barra de luz (con color según cuánta queda y pulso cuando queda poca)
/// y contador de fotos de recuerdo del nivel.
/// Va en el UI_Canvas. La barra es una Image tipo Filled, horizontal.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] XolotlLight xolotlLight;
    [SerializeField] Image lightFill;
    [SerializeField] TMP_Text photoText;

    [Header("Barra de luz")]
    [SerializeField] Gradient lightColor;
    [SerializeField] float fillSpeed = 3f;
    [Range(0f, 1f)][SerializeField] float lowLightWarning = 0.25f;
    [SerializeField] float pulseSpeed = 8f;

    [Header("Fotos")]
    [SerializeField] string photoLabel = "Recuerdos";

    MemoryPhoto[] levelPhotos;
    float targetFill = 1f;

    void Reset()
    {
        // Naranja cempasúchil cuando está llena, rojo apagado cuando se acaba
        lightColor = new Gradient();
        lightColor.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(0.75f, 0.15f, 0.1f), 0f),
                new GradientColorKey(new Color(1f, 0.62f, 0.11f), 0.5f),
                new GradientColorKey(new Color(1f, 0.85f, 0.4f), 1f)
            },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });
    }

    void Awake()
    {
        if (xolotlLight == null) xolotlLight = FindAnyObjectByType<XolotlLight>();
        levelPhotos = FindObjectsByType<MemoryPhoto>(FindObjectsInactive.Include);
    }

    void OnEnable()
    {
        if (xolotlLight != null) xolotlLight.OnLightChanged += HandleLightChanged;
        MemoryPhoto.OnPhotoCollected += HandlePhotoCollected;
    }

    void OnDisable()
    {
        if (xolotlLight != null) xolotlLight.OnLightChanged -= HandleLightChanged;
        MemoryPhoto.OnPhotoCollected -= HandlePhotoCollected;
    }

    void Start() => RefreshPhotos();

    void Update()
    {
        if (lightFill == null) return;

        lightFill.fillAmount = Mathf.MoveTowards(lightFill.fillAmount, targetFill, fillSpeed * Time.deltaTime);

        Color c = lightColor != null ? lightColor.Evaluate(lightFill.fillAmount) : Color.white;
        if (targetFill > 0f && targetFill < lowLightWarning)
            c.a = Mathf.Lerp(0.35f, 1f, (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f);
        lightFill.color = c;
    }

    void HandleLightChanged(float normalized) => targetFill = normalized;

    void HandlePhotoCollected(int total) => RefreshPhotos();

    void RefreshPhotos()
    {
        if (photoText == null) return;
        if (levelPhotos == null || levelPhotos.Length == 0)
        {
            photoText.text = "";
            return;
        }

        int got = 0;
        foreach (MemoryPhoto p in levelPhotos)
            if (p != null && p.IsCollected) got++;

        photoText.text = $"{photoLabel} {got}/{levelPhotos.Length}";
    }
}