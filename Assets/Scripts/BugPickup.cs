using UnityEngine;
using UnityEngine.AI;

public class BugPickup : MonoBehaviour
{
    BugAI bugAI;
    NavMeshAgent agent;
    Rigidbody rb;

    void Awake()
    {
        bugAI = GetComponent<BugAI>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public void Pickup()
    {
        bugAI.enabled = false;
        agent.enabled = false;

        rb.isKinematic = false;

        gameObject.tag = "bug";
    }
}
