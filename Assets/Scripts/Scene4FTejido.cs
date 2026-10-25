using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene4FTejido : MonoBehaviour
{
    [Header("Parámetros del Tejido")]
    [Tooltip("Distancia vertical (en metros) que bajará cada tabla al crearse una nueva.")]
    [SerializeField] private float distanciaDesplazamiento = 0.2f;

    [Tooltip("Velocidad de la animación de bajada suave.")]
    [SerializeField] private float velocidadMovimiento = 0.2394f;

    [Tooltip("Distancia hacia atrás (en Z) en la que aparecerá la nueva tabla antes de moverse al frente.")]
    [SerializeField] private float offsetProfundidad = 0.1f;

    private List<GameObject> listaTablas = new List<GameObject>();
    public void AgregarNuevaTabla(GameObject nuevaTabla)
    {
        // Mover las tablas existentes hacia abajo
        for (int i = 0; i < listaTablas.Count; i++)
        {
            if (listaTablas[i] != null)
            {
                Vector3 posicionObjetivo = listaTablas[i].transform.position + Vector3.down * distanciaDesplazamiento;
                StartCoroutine(MoverTablaSuave(listaTablas[i].transform, posicionObjetivo));
            }
        }

        Vector3 posicionFinalNueva = nuevaTabla.transform.position;

        nuevaTabla.transform.position = posicionFinalNueva + (Vector3.forward * offsetProfundidad);

        StartCoroutine(MoverTablaSuave(nuevaTabla.transform, posicionFinalNueva));

        listaTablas.Add(nuevaTabla);
    }

    private IEnumerator MoverTablaSuave(Transform tablaTransform, Vector3 destino)
    {
        while (tablaTransform != null && Vector3.Distance(tablaTransform.position, destino) > 0.001f)
        {
            tablaTransform.position = Vector3.Lerp(tablaTransform.position, destino, Time.deltaTime * velocidadMovimiento);
            yield return null;
        }

        if (tablaTransform != null)
        {
            tablaTransform.position = destino;
        }
    }
}
