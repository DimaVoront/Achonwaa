using System;
using UnityEngine;
using UnityEngine.AI;

public class HitableBehaviour : MonoBehaviour
{
    [SerializeField] int hp;
    NavMeshAgent agent;
    [SerializeField] Transform[] patrolPoints;
    int currentPatrol = 0;
    [SerializeField] bool isSaino = false;
    [SerializeField] bool isConqueror = false;

    [SerializeField] Animator anims;
    bool isDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        
    }

    private void Update()
    {
        if(!isDead)
            PatrolPattern();

        if (isDead)
            agent.isStopped = true;
            
    }

    public void PatrolPattern()
    {
        agent.SetDestination(patrolPoints[currentPatrol].position);
        if(Vector3.Distance(transform.position, patrolPoints[currentPatrol].position) < 1.5)
        {
            if (currentPatrol >= patrolPoints.Length - 1)
            {
                currentPatrol = 0;
            } else
            {
                currentPatrol++;
            }
            agent.SetDestination(patrolPoints[currentPatrol].position);
        }

    }

    public void TakeDamage()
    {
        if(!isDead)
        {
            hp = hp - 10;
            if (hp <= 0)
            {
                anims.SetTrigger("Die");
                isDead = true;
                if(isSaino)
                {
                    Scene4MManager.instance.AddSaino();
                }
                if (isConqueror)
                {
                    Scene5MManager.instance.AddSaino();
                }
            }
        }
        
    }
}
