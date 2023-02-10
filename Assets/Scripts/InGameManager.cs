using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public Joystick Joystick;
    public BotsManager botsManager;
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}
