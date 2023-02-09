using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Joystick joystick => InGameManager.Instance.Joystick;
    [SerializeField] CarController carController;
    private void Update()
    {
        Transform camT = Camera.main.transform;
        if (joystick.IsHolding)
        {
            var movmentDir = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
            var camDir = camT.TransformDirection(movmentDir);
            camDir.y = 0;
            camDir.Normalize();
            carController.Move(camDir);
        }
        else
        {
            carController.Stop();
        }
        camT.LookAt(carController.transform.position);
    }
}
