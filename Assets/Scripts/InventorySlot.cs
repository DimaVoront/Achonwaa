using System.Reflection.Metadata.Ecma335;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InventorySlot : MonoBehaviour
{
    private XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnItemStored);
        socket.selectExited.AddListener(OnItemRemoved);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnItemStored);
        socket.selectExited.RemoveListener(OnItemRemoved);
    }

    private void OnItemStored(SelectEnterEventArgs args)
    {
        GameObject fruit = args.interactableObject.transform.gameObject;
        Debug.Log($"Fruta guardada en el inventario: {fruit.name}");
        if(Scene5FManager.instance != null)
        {
            Scene5FManager.instance.FruitColected();
        }

       
    }

    private void OnItemRemoved(SelectExitEventArgs args)
    {
        GameObject fruit = args.interactableObject.transform.gameObject;
        Debug.Log($"Fruta retirada del inventario: {fruit.name}");
        if (Scene5FManager.instance != null)
        {
            Scene5FManager.instance.FruitRemoved();
        }
    }
}