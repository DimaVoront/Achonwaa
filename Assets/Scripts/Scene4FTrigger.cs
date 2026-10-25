using UnityEngine;

public class Scene4FTrigger : MonoBehaviour
{
    [Tooltip("Número de este trigger (1, 2 o 3)")]
    public int numeroTrigger = 1;

    [SerializeField] private Scene4FTelarController controladorTelar;

    private void OnTriggerEnter(Collider other)
    {
        if (controladorTelar != null)
        {
            controladorTelar.RegistrarPasoTrigger(numeroTrigger, other.gameObject);
        }
    }
}
