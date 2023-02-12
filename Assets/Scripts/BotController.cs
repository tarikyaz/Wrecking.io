using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public CarController _CarController;
    [SerializeField] float durationToChangeRandomPoint = 5;
    Transform targetT;
    float timer = 0;

    private void FixedUpdate()
    {
        if (_CarController.isDied)
        {
            return;
        }
        Vector3 targetPos = Vector3.zero;
        if (targetT != null)
        {
            targetPos = targetT.position;
        }
        targetPos.y = 0;
        if (targetPos != Vector3.zero && timer <= durationToChangeRandomPoint && _CarController.NavMeshAgent.isActiveAndEnabled && Vector3.Distance(targetPos, transform.position) > 5)
        {
            timer += Time.fixedDeltaTime;
            _CarController.NavMeshAgent.SetDestination(targetPos);
            _CarController.Move(_CarController.NavMeshAgent.steeringTarget - transform.position);
        }
        else
        {
            _CarController.Stop();
            if (InGameManager.Instance != null)
            {
                targetT = InGameManager.Instance.GetRandomPoint(targetT);
                timer = 0;
            }
        }
    }
}
