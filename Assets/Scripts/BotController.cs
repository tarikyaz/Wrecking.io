using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    [SerializeField] CarController carController;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform targetT;
    void Start()
    {
        navMeshAgent.speed = 0;
        navMeshAgent.angularSpeed = 0;
    }
    private void FixedUpdate()
    {
        navMeshAgent.SetDestination(targetT.position);
        if (Vector3.Distance(targetT.position, transform.position) > .1f)
        {
            carController.Move(navMeshAgent.steeringTarget - transform.position);
        }

    }
}
