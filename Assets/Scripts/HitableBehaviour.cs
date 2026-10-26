using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
    [SerializeField] ParticleSystem deathParticles;

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
                deathParticles.Play();
                RandomAudioPlayer.instance.ReproducirAudioAleatorio();
                if (isSaino)
                {
                    Scene4MManager.instance.AddSaino();
                }
                if (isConqueror)
                {
                    StartCoroutine(BajarPosicionY());
                    Scene5MManager.instance.AddSaino();
                }
                Collider col = GetComponent<Collider>();
                col.enabled = false;
            }
        }
        
    }

    private IEnumerator BajarPosicionY()
    {
        float duracion = 0.5f;        
        float tiempoTranscurrido = 0f;

        float inicioY = -0.5f;
        float finY = -1.0f;

        Vector3 posicionInicial = transform.position;

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float porcentaje = tiempoTranscurrido / duracion;

            float nuevoY = Mathf.Lerp(inicioY, finY, porcentaje);
            transform.position = new Vector3(posicionInicial.x, nuevoY, posicionInicial.z);

            yield return null;
        }
        transform.position = new Vector3(posicionInicial.x, finY, posicionInicial.z);
    }
}
