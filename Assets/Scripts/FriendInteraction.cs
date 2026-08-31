using Unity.VisualScripting;
using UnityEngine;

public class FriendInteraction : MonoBehaviour
{
    
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Friend"))
        {
            Debug.Log("Oli");
        }
    }
}
