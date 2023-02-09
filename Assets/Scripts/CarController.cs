using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10, distanceFromBall , ballMovementSmoothness;
    [SerializeField] WrackingBall wrackingBall;
    [SerializeField] LineRenderer lineRenderer;
    private void Start()
    {
        lineRenderer.positionCount = 2;
    }
    public void Move(Vector3 dir)
    {
        rb.MoveRotation(Quaternion.LookRotation(dir.normalized));
        rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
    }
    private void LateUpdate()
    {
        Vector3 targetBallPos = Vector3.Slerp(wrackingBall.rb.position, rb.position - transform.forward * distanceFromBall, Time.fixedTime * ballMovementSmoothness);
        targetBallPos.y = wrackingBall.rb.position.y;
        wrackingBall.rb.position = targetBallPos;
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        lineRenderer.SetPosition(1, wrackingBall.transform.position);

    }
    internal void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
