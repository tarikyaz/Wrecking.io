using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    [SerializeField] Transform parachute;
    [SerializeField] float superPowerDuration = 5;
    internal bool isActive;
    bool isCollceted = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                parachute.transform.SetParent(null, true);
                parachute.DOScale(0, 2).OnComplete(() => {
                    Destroy(parachute.gameObject); 
                    Destroy(gameObject, 10); });
                    isActive = true;
            }
        }
    }

    internal void Collect(CarController carController)
    {
        if (!isCollceted && isActive)
        {
            isCollceted = true;
            carController.AcrtivateSuperPower(superPowerDuration);
            DOVirtual.DelayedCall(3, () => Destroy(gameObject));
        }
    }
}
