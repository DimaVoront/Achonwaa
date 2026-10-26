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
    [Tooltip("GameObject que contiene los AudioSource (o sus hijos)")]
    [SerializeField] private GameObject audioHolder;

    [Header("Ajustes de Transición")]
    [SerializeField] private float fadeDuration = 2f;

    private ColorAdjustments colorAdjustments;
    private const float BLACK_EXPOSURE = -10f;
    private float normalExposure = 0f;

    // Guardamos los AudioSource y sus volúmenes objetivo/originales
    private AudioSource[] audioSources;
    private float[] targetAudioVolumes;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Inicializar fuentes de audio si el GameObject fue asignado
        if (audioHolder != null)
        {
            // Busca todos los AudioSource dentro del GameObject asignado (incluyendo hijos)
            audioSources = audioHolder.GetComponentsInChildren<AudioSource>();
            targetAudioVolumes = new float[audioSources.Length];

            // Guardamos el volumen original de cada AudioSource
            for (int i = 0; i < audioSources.Length; i++)
            {
                targetAudioVolumes[i] = audioSources[i].volume;
            }
        }

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

        // Iniciar volúmenes de los audios en 0 para el Fade In
        SetAudioVolumes(0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;

            // Transición visual
            colorAdjustments.postExposure.value = Mathf.Lerp(BLACK_EXPOSURE, normalExposure, progress);

            // Fade In de volumen (de 0 al volumen objetivo de cada AudioSource)
            UpdateAudioVolumes(0f, progress);

            yield return null;
        }

        colorAdjustments.postExposure.value = normalExposure;
        UpdateAudioVolumes(0f, 1f);
    }

    public IEnumerator FadeToBlack(string scene)
    {
        yield return new WaitForSeconds(2f);
        float elapsedTime = 0f;
        float currentExposure = colorAdjustments.postExposure.value;

        // Guardar volúmenes actuales en caso de que hayan cambiado
        float[] startVolumes = new float[audioSources != null ? audioSources.Length : 0];
        if (audioSources != null)
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                if (audioSources[i] != null)
                    startVolumes[i] = audioSources[i].volume;
            }
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;

            // Transición visual
            colorAdjustments.postExposure.value = Mathf.Lerp(currentExposure, BLACK_EXPOSURE, progress);

            // Fade Out de volumen (del volumen actual a 0)
            if (audioSources != null)
            {
                for (int i = 0; i < audioSources.Length; i++)
                {
                    if (audioSources[i] != null)
                    {
                        audioSources[i].volume = Mathf.Lerp(startVolumes[i], 0f, progress);
                    }
                }
            }

            yield return null;
        }

        colorAdjustments.postExposure.value = BLACK_EXPOSURE;
        SetAudioVolumes(0f);

        SceneManager.LoadScene(scene);
    }

    // Establece un volumen fijo para todos los AudioSource
    private void SetAudioVolumes(float volumeLevel)
    {
        if (audioSources == null) return;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] != null)
            {
                audioSources[i].volume = Mathf.Lerp(0f, targetAudioVolumes[i], volumeLevel);
            }
        }
    }

    // Interpola el volumen de cada AudioSource según su volumen objetivo guardado
    private void UpdateAudioVolumes(float startProgress, float progress)
    {
        if (audioSources == null) return;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] != null)
            {
                audioSources[i].volume = Mathf.Lerp(0f, targetAudioVolumes[i], progress);
            }
        }
    }
}
