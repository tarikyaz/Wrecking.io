using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Joystick joystick => InGameManager.Instance.Joystick;
    [SerializeField] CarController carController;
    bool movmentUnlocked = false;
    private void FixedUpdate()
    {
        Transform camT = Camera.main.transform;
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
