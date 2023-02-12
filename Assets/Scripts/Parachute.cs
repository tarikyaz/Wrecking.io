using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    [SerializeField] Transform parachute;
    [SerializeField] float superPowerDuration = 5;
    internal bool topDisabled;
    bool isCollceted = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!topDisabled)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                DisableTop();
            }
        }
    }

    private void DisableTop()
    {
        if (!topDisabled)
        {
            topDisabled = true;
            parachute.transform.SetParent(null, true);
            parachute.DOScale(0, 2).OnComplete(() =>
            {
                Destroy(parachute.gameObject);
                Destroy(gameObject, 10);
            });
        }
    }

    internal void Collect(CarController carController)
    {
        if (!isCollceted)
        {
            isCollceted = true;
            DisableTop();
            carController.AcrtivateSuperPower(superPowerDuration);
            transform.DOScale(0, .5f).OnComplete(() => Destroy(gameObject));
        }
    }
}
