using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class DirectedAgent : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void MoveToLocation(Vector3 targetPoint)
    {
        agent.destination = targetPoint;
        agent.isStopped = false;
    }

    void Update()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat("forwardSpeed", speed);
    }
}
