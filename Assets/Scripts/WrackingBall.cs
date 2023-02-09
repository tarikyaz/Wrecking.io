using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class WrackingBall : MonoBehaviour
{
    private CarController carController;
    public Action<CarController> OnHitCar;
    public Rigidbody rb;
    [SerializeField] Collider coll;
    public void SetCar(CarController car)
    {
        carController = car;
        Physics.IgnoreCollision(coll, car.Coll);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Car"))
        {
            if (collision.collider.TryGetComponent(out CarController cc))
            {
                if (cc != carController)
                {
                    OnHitCar?.Invoke(cc);
                }
            }
        }
    }
}
