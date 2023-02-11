using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Joystick joystick => InGameManager.Instance.Joystick;
    public CarController carController;
    bool movmentUnlocked = false;
    private void Start()
    {
        carController.NavMeshAgent.enabled = false;
    }
    private void FixedUpdate()
    {

        if (carController ==null)
        {
            return;
        }
        if (carController.isDied)
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
                carController.Move(camDir);
            }
        }
        else
        {
            carController.Stop();
            movmentUnlocked = false;

        }
    }
}
