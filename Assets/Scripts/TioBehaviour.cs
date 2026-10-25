using UnityEngine;
using UnityEngine.AI;

public class TioBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Animator anims;
    [SerializeField] Transform player;
    public float distance = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
        distance = Vector3.Distance(transform.position, player.position);
        anims.SetFloat("Distance", distance);
    }
}
