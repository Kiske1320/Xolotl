using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Salida del nivel (por ejemplo, la reja del panteón). Al tocarla, Xólotl deja de perder luz,
/// se bloquean los controles, la pantalla se funde a negro y carga la siguiente escena.
/// Necesita un Collider2D en Trigger. La escena siguiente debe estar en la lista de Build Profiles.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LevelExit : MonoBehaviour
{
    [SerializeField] string nextSceneName = "02_Calles";
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] AudioClip exitSfx;

    bool used;

    void Reset() => GetComponent<Collider2D>().isTrigger = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used || other.attachedRigidbody == null) return;

        XolotlLight xolotlLight = other.attachedRigidbody.GetComponent<XolotlLight>();
        if (xolotlLight == null || xolotlLight.IsEmpty) return;

        used = true;
        StartCoroutine(Exit(xolotlLight, other.attachedRigidbody.GetComponent<XolotlController>()));
    }

    IEnumerator Exit(XolotlLight xolotlLight, XolotlController controller)
    {
        xolotlLight.SetDrainPaused(true);
        if (controller != null) controller.SetControlsEnabled(false);
        if (exitSfx != null) AudioSource.PlayClipAtPoint(exitSfx, transform.position);

        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeOut(fadeDuration);
        else
            yield return new WaitForSeconds(fadeDuration);

        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning($"[LevelExit] La escena '{nextSceneName}' no está en Build Profiles. Se recarga la escena actual.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
