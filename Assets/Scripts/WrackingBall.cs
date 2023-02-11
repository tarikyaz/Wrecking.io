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
    [SerializeField] GameObject[] ballsArray = new GameObject[0];
    public void SetCar(CarController car)
    {
        carController = car;
        Physics.IgnoreCollision(coll, car.Coll);
        transform.SetParent(null, true);
        car.OnKill += () => Destroy(gameObject);
        ballsArray[UnityEngine.Random.Range(0, ballsArray.Length)].SetActive(true);
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
