using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Scene4FTejido : MonoBehaviour
{
    [Header("Parámetros del Tejido")]
    [Tooltip("Distancia vertical (en metros) que bajará cada tabla al crearse una nueva.")]
    [SerializeField] private float distanciaDesplazamiento = 0.2f;

    [Tooltip("Velocidad de la animación de bajada suave.")]
    [SerializeField] private float velocidadMovimiento = 3f;


    private List<GameObject> listaTablas = new List<GameObject>();

  
    public void AgregarNuevaTabla(GameObject nuevaTabla)
    {

        for (int i = 0; i < listaTablas.Count; i++)
        {
            if (listaTablas[i] != null)
            {
                Vector3 posicionObjetivo = listaTablas[i].transform.position + Vector3.down * distanciaDesplazamiento;
                StartCoroutine(MoverTablaSuave(listaTablas[i].transform, posicionObjetivo));
            }
        }

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
