using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    [SerializeField] CarController carController;
    [SerializeField] float durationToChangeRandomPoint = 5;
    Transform targetT;
    float timer = 0;

    private void FixedUpdate()
    {
        if (carController.isDied)
        {
            return;
        }
        Vector3 targetPos = Vector3.zero;
        if (targetT != null)
        {
            targetPos = targetT.position;
        }
        targetPos.y = 0;
        if (targetPos != Vector3.zero && timer <= durationToChangeRandomPoint && carController.NavMeshAgent.isActiveAndEnabled && Vector3.Distance(targetPos, transform.position) > 5)
        {
            timer += Time.fixedDeltaTime;
            carController.NavMeshAgent.SetDestination(targetPos);
            carController.Move(carController.NavMeshAgent.steeringTarget - transform.position);
        }
        else
        {
            carController.Stop();
            if (InGameManager.Instance != null)
            {
                targetT = InGameManager.Instance.botsManager.GetRandomPoint(targetT);
                timer = 0;
            }
        }
    }
}
