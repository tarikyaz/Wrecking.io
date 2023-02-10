using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Collider Coll;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10, distanceFromBall, ballMovementSmoothness, ballHitForce = 10;
    [SerializeField] WrackingBall wrackingBall;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Animator carAnim;
    [SerializeField] ParticleSystem smokeVFX;
    bool isDied = false;
    bool isMoving = false;
    private void OnEnable()
    {
        wrackingBall.OnHitCar += OnHitCar;
    }



    private void OnDisable()
    {
        wrackingBall.OnHitCar -= OnHitCar;
    }

    private void OnHitCar(CarController car)
    {
        if (isMoving)
        {
            Vector3 hitDir = car.transform.position - wrackingBall.transform.position;
            hitDir.Normalize();
            hitDir.y = 1;
            car.Hit(hitDir);
        }
    }
    private void Start()
    {
        lineRenderer.positionCount = 2;
        wrackingBall.SetCar(this);

    }
    public void Move(Vector3 dir)
    {
        if (!isDied)
        {
            smokeVFX.Emit(10);
            isMoving = true;
            Vector3 localDir = transform.InverseTransformDirection(dir);
            if (localDir.x > .05f)
            {
                if (carAnim.GetBool("Center"))
                {
                    carAnim.SetBool("Center", false);
                    carAnim.speed = 0;
                }
                else
                {
                    carAnim.Play("SteerR", 0, Mathf.InverseLerp(0, 1, localDir.x));
                }
            }
            else if (localDir.x < -.05f)
            {
                if (carAnim.GetBool("Center"))
                {
                    carAnim.SetBool("Center", false);
                    carAnim.speed = 0;
                }
                else
                {
                    carAnim.Play("SteerL", 0, Mathf.InverseLerp(0, -1, localDir.x));
                }
            }
            else
            {
                carAnim.SetBool("Center", true);
                carAnim.speed = 1;
            }
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRot, speed * Time.fixedDeltaTime));
            rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
        }
    }
    private void LateUpdate()
    {
        if (!isDied)
        {
            Vector3 targetBallPos = transform.TransformPoint(0, 0, -distanceFromBall);
            float t = Time.fixedTime * ballMovementSmoothness;
            targetBallPos.y = wrackingBall.rb.position.y;
            if (targetBallPos.y > 2)
            {
                targetBallPos.y = 2;
            }
            else if (targetBallPos.y < 1)
            {
                targetBallPos.y = 1;
            }
            wrackingBall.rb.position = Vector3.Slerp(wrackingBall.rb.position, targetBallPos, t);
            lineRenderer.SetPosition(0, lineRenderer.transform.position);
            lineRenderer.SetPosition(1, wrackingBall.transform.position);
        }
    }
    internal void Stop()
    {
        isMoving = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

    }
    public void Hit(Vector3 dir)
    {
        if (!isDied)
        {
            isDied = true;
            isMoving = false;
            wrackingBall.OnHitCar -= OnHitCar;
            lineRenderer.gameObject.SetActive(false);
            rb.constraints = RigidbodyConstraints.None;
            rb.mass = 5f;
            rb.angularDrag = .0005f;
            rb.velocity = dir * ballHitForce;
            Destroy(gameObject, 10);
        }
    }
}
