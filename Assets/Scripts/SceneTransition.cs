using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;

    [Header("Referencias")]
    [SerializeField] private Volume volume;

    [Header("Ajustes de Transición")]
    [SerializeField] private float fadeDuration = 1.5f;

    private ColorAdjustments colorAdjustments;
    private const float BLACK_EXPOSURE = -10f;
    private float normalExposure = 0f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (volume != null && volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.overrideState = true;
            normalExposure = colorAdjustments.postExposure.value;
            StartCoroutine(FadeFromBlack());
        }
        else
        {
            Debug.LogError("No se encontró el componente Color Adjustments en el Volume Profile.");
        }
    }

    public IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;
        colorAdjustments.postExposure.value = BLACK_EXPOSURE;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;
            colorAdjustments.postExposure.value = Mathf.Lerp(BLACK_EXPOSURE, normalExposure, progress);
            yield return null;
        }

        colorAdjustments.postExposure.value = normalExposure;
    }

    public IEnumerator FadeToBlack(string scene)
    {
        yield return new WaitForSeconds(2f);
        float elapsedTime = 0f;
        float currentExposure = colorAdjustments.postExposure.value;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;
            colorAdjustments.postExposure.value = Mathf.Lerp(currentExposure, BLACK_EXPOSURE, progress);
            yield return null;
        }

        colorAdjustments.postExposure.value = BLACK_EXPOSURE;
        SceneManager.LoadScene(scene);
    }
}
