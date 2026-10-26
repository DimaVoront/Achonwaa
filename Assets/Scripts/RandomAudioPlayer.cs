using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    public static RandomAudioPlayer instance;


    private void Awake()
    {
        instance = this;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    public void ReproducirAudioAleatorio()
    {
        if (audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("RandomAudioPlayer: No hay AudioClips asignados en el array.", this);
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("RandomAudioPlayer: No se encontró ningún AudioSource asignado.", this);
            return;
        }

        int indiceAleatorio = Random.Range(0, audioClips.Length);
        AudioClip clipSeleccionado = audioClips[indiceAleatorio];

        audioSource.PlayOneShot(clipSeleccionado);
    }
}
