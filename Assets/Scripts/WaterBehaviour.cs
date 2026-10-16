using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Mucura"))
        {
            Scene5FManager.instance.GrabWater();
        }
    }
}
