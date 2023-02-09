using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10;
    void Start()
    {
        
    }
    public void Move(Vector3 dir)
    {
        Vector3 targetPos = transform.position + dir;
        rb.MoveRotation(Quaternion.LookRotation(targetPos - transform.position));
        rb.MovePosition(targetPos);
        
    }

    internal void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
