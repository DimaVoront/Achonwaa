using System.Collections.Generic;
using UnityEngine;

public class Scene4FTelarController : MonoBehaviour
{
    [System.Serializable]
    public class EstadoBolita
    {
        public int idBolita;
        public bool trigger1Alcanzado;
        public bool trigger2Alcanzado;
        public bool trigger3Alcanzado;

        public bool EsCompleto => trigger1Alcanzado && trigger2Alcanzado && trigger3Alcanzado;
    }

    [Header("Configuración del Telar")]
    [SerializeField] private GameObject prefabCuadrado;
    [SerializeField] private Transform puntoDeInstancia;
    [SerializeField] private Scene4FTejido tejidoManager; 

    private Dictionary<int, EstadoBolita> progresoBolitas = new Dictionary<int, EstadoBolita>();

    public void RegistrarPasoTrigger(int numeroTrigger, GameObject objetoEntrante)
    {
        Scene4Yarn bolita = objetoEntrante.GetComponent<Scene4Yarn>();
        if (bolita == null) return;

        int id = bolita.idBolita;

        if (!progresoBolitas.ContainsKey(id))
        {
            progresoBolitas[id] = new EstadoBolita { idBolita = id };
        }

        EstadoBolita estado = progresoBolitas[id];

        switch (numeroTrigger)
        {
            case 1: estado.trigger1Alcanzado = true; break;
            case 2: estado.trigger2Alcanzado = true; break;
            case 3: estado.trigger3Alcanzado = true; break;
        }

        if (estado.EsCompleto)
        {
            CompletarTejido(bolita);
            Scene4FManager.instance.AddTejido();
        }
    }

    private void CompletarTejido(Scene4Yarn bolita)
    {
        Vector3 posicionSpawn = puntoDeInstancia != null ? puntoDeInstancia.position : bolita.transform.position;
        Quaternion rotacionSpawn = puntoDeInstancia != null ? puntoDeInstancia.rotation : Quaternion.identity;

     
        GameObject nuevaTabla = Instantiate(prefabCuadrado, posicionSpawn, rotacionSpawn);

    
        if (tejidoManager != null)
        {
            tejidoManager.AgregarNuevaTabla(nuevaTabla);
        }

     
        progresoBolitas.Remove(bolita.idBolita);
        bolita.DestruirConTrail();
    }
}
