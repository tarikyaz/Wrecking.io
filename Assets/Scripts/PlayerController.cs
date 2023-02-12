using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Joystick joystick => InGameManager.Instance.Joystick;
    public CarController _CarController;
    bool movmentUnlocked = false;
    private void Start()
    {
        _CarController.NavMeshAgent.enabled = false;
    }
    private void FixedUpdate()
    {

        if (_CarController ==null)
        {
            return;
        }
        if (_CarController.isDied)
        {
            return;
        }
        Transform camT = InGameManager.Instance.cam.transform;
        if (joystick.IsHolding)
        {
            if (joystick.Direction.magnitude > .7f)
            {
                movmentUnlocked = true;
            }
            if (movmentUnlocked)
            {
                var movmentDir = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
                var camDir = camT.TransformDirection(movmentDir);
                camDir.y = 0;
                camDir.Normalize();
                _CarController.Move(camDir);
            }
        }
        else
        {
            _CarController.Stop();
            movmentUnlocked = false;

        }
    }
}
