
 using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Scene4Yarn : MonoBehaviour
{
    [Header("Identificador")]
    [Tooltip("Asigna un ID único a cada bolita (ej: 1, 2, 3)")]
    public int idBolita;

    [Header("Efectos")]
    [SerializeField] private TrailRenderer trailRenderer;

    private void Awake()
    {
        if (trailRenderer == null)
        {
            trailRenderer = GetComponentInChildren<TrailRenderer>();
        }
    }

    public void DestruirConTrail()
    {
        if (trailRenderer != null)
        {
            trailRenderer.transform.SetParent(null);
            trailRenderer.emitting = false; 
            Destroy(trailRenderer.gameObject, trailRenderer.time);
        }
        Destroy(gameObject);
    }
}

