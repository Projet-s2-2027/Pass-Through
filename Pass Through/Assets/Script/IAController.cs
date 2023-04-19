using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerIA : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private GameObject player;

    [Header("Stats")]
    [SerializeField]
    private float detectionRadius;
    [SerializeField]
    private float wanderRadius; // Nouvelle variable pour la distance de la destination aléatoire
    [SerializeField]
    private float wanderTimer; // Temps d'attente avant de générer une nouvelle destination aléatoire

    private bool hasDestination;
    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent n'est pas assigné.");
        }

        if (player == null)
        {
            Debug.LogError("Le joueur n'est pas assigné.");
        }

        timer = wanderTimer;
    }

    void Update()
    {
        if (agent == null || player == null)
        {
            return;
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < detectionRadius)
        {
            agent.SetDestination(player.transform.position);
            timer = wanderTimer; // Réinitialiser le timer si le joueur est détecté
        }
        else
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = wanderTimer;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
