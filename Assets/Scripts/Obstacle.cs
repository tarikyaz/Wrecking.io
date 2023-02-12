using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] Transform jumperObject;
    Sequence animationSeq;
    private void OnEnable()
    {
        jumperObject.localScale = Vector3.one;
    }
    internal bool IsActive = false;
    public void Collect(CarController carController)
    {
        if (IsActive)
        {
            animationSeq.Kill();
            animationSeq = DOTween.Sequence();
            IsActive = false;
            Vector3 dir = transform.forward;
            dir.Normalize();
            dir.y = 1;
            carController.Kill(dir);
            animationSeq.Append(jumperObject.DOPunchScale(Vector3.zero, 1, 1));
            animationSeq.Append(transform.DOScale(0, .5f));

        }
    }

}
