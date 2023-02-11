using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public Joystick Joystick;
    public BotsManager botsManager;
    public Camera cam;
    public PlayerController player;
    [SerializeField] float camHeight = 75;
    [SerializeField] Vector3 Offset;
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    private void LateUpdate()
    {
        var camPos = player != null && !player.carController.isDied ? player.transform.position : Vector3.zero;
        camPos += Offset;
        cam.transform.position = camPos;
        cam.transform.LookAt(camPos - Offset);
    }
}
