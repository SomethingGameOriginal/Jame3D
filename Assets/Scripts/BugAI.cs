using UnityEngine;
using UnityEngine.AI;

public class BugAI : MonoBehaviour
{
    NavMeshAgent agent;

    public float wanderRadius = 5;
    public float waitTime = 2;
    float timer;

    [Header("Испуг")]
    public float scareChance = 0.15f; // шанс испуга
    public float scareCooldown = 5;
    public float scareDistance = 3;
    float scareTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!agent.isOnNavMesh) return;

        timer -= Time.deltaTime;

        if (timer <= 0 && !agent.pathPending &&
            (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance))
        {
            Vector3 randomPoint = transform.position +
                Random.insideUnitSphere * wanderRadius;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit,
                wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            timer = waitTime;
        }

        scareTimer -= Time.deltaTime;

        if (scareTimer <= 0)
        {
            if (Random.value < scareChance)
            {
                Vector3 randomDirection = Random.insideUnitSphere * scareDistance;
                randomDirection.y = 0;

                if (NavMesh.SamplePosition(transform.position + randomDirection, out NavMeshHit hit, scareDistance, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }

            scareTimer = scareCooldown;
        }
    }
}
