using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleAudioSystem : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [Header("Audio")]
        public AudioClip audioClip;

        [Header("Subtítulo")]
        [TextArea(2, 5)]
        public string subtitle;

        [Header("Pausa después del diálogo")]
        [Min(0f)]
        public float pauseAfter = 1f;
    }

    [System.Serializable]
    public class DialogueGroup
    {
        [Header("Nombre del grupo")]
        public string groupName;

        [Header("Diálogos")]
        public DialogueLine[] dialogueLines;
    }

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Texto")]
    public TextMeshProUGUI subtitleText;

    [Header("Grupos de diálogo")]
    public DialogueGroup[] dialogueGroups;

    private Coroutine dialogueCoroutine;
    private bool isPlaying;

    private void Start()
    {
        // Comenzar sin subtítulos
        subtitleText.text = "";
        StartCoroutine(LoadWaitTime());
    }

    // Iniciar un grupo específico
    public void StartDialogueGroup(int groupIndex)
    {
        // Si ya hay otro diálogo reproduciéndose,
        // detenerlo antes de comenzar el nuevo
        if (isPlaying)
        {
            StopDialogue();
        }

        dialogueCoroutine = StartCoroutine(
            PlayDialogueGroup(groupIndex)
        );
    }

    IEnumerator LoadWaitTime()
    {
        yield return new WaitForSeconds(3f);
        StartDialogueGroup(0);
    }

    private IEnumerator PlayDialogueGroup(int groupIndex)
    {
        isPlaying = true;

        // Comprobar que el grupo exista
        if (groupIndex < 0 || groupIndex >= dialogueGroups.Length)
        {
            Debug.LogError("El grupo de diálogo no existe.");
            isPlaying = false;
            yield break;
        }

        DialogueGroup currentGroup = dialogueGroups[groupIndex];

        // Recorrer todos los diálogos del grupo
        foreach (DialogueLine dialogue in currentGroup.dialogueLines)
        {
            // Comprobar que exista un audio
            if (dialogue.audioClip == null)
            {
                Debug.LogWarning(
                    "Hay un diálogo sin AudioClip en el grupo: "
                    + currentGroup.groupName
                );

                continue;
            }

            // Mostrar el subtítulo
            subtitleText.text = dialogue.subtitle;

            // Configurar y reproducir el audio
            audioSource.clip = dialogue.audioClip;
            audioSource.Play();

            // Esperar hasta que termine el audio
            yield return new WaitWhile(() => audioSource.isPlaying);

            // Ocultar el subtítulo
            subtitleText.text = "";

            // Esperar la pausa configurada
            if (dialogue.pauseAfter > 0)
            {
                yield return new WaitForSeconds(dialogue.pauseAfter);
            }
        }

        // Asegurarse de que el texto esté vacío
        subtitleText.text = "";

        isPlaying = false;
    }

    // Detener el diálogo actual
    public void StopDialogue()
    {
        // Detener audio
        audioSource.Stop();

        // Detener corrutina
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
        }

        // Ocultar subtítulo
        subtitleText.text = "";

        isPlaying = false;
    }
}