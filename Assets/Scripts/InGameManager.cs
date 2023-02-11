using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{
    public Action OnRaceStart, OnStopRace;
    public Joystick Joystick;
    public BotsManager botsManager;
    public Camera cam;
    public PlayerController player;
    [SerializeField] float camHeight = 75;
    [SerializeField] Vector3 Offset;
    [SerializeField] TMP_Text counting_Text, playersLeft_Text, resault_Text;
    [SerializeField] Button Restart_Button;
    [SerializeField] Transform wall;
    Sequence countingSeq;
    int alivePlyers;
    internal bool isFighting = false;
    private void Start()
    {
        Application.targetFrameRate = 60;
        countingSeq.Kill();
        countingSeq = DOTween.Sequence();
        counting_Text.gameObject.SetActive(true);
        countingSeq.AppendCallback(() => counting_Text.text = 3.ToString());
        countingSeq.Append(counting_Text.transform.DOScale(1, .5f).From(0));
        countingSeq.AppendInterval(.5f);
        countingSeq.AppendCallback(() => counting_Text.text = 2.ToString());
        countingSeq.Append(counting_Text.transform.DOScale(1, .5f).From(0));
        countingSeq.AppendInterval(.5f);
        countingSeq.AppendCallback(() => counting_Text.text = 1.ToString());
        countingSeq.Append(counting_Text.transform.DOScale(1, .5f).From(0));
        countingSeq.AppendInterval(.5f);
        countingSeq.AppendCallback(() =>
        {
            isFighting = true;
            counting_Text.gameObject.SetActive(false);
            OnRaceStart?.Invoke();
        });
        alivePlyers = botsManager.NumOfBots + 1;
        playersLeft_Text.text = alivePlyers.ToString();
        Restart_Button.gameObject.SetActive(false);
        Restart_Button.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }

    public void PlayerDied()
    {
        alivePlyers--;
        playersLeft_Text.text = alivePlyers.ToString();
        if (player.carController.isDied)
        {
            Lost();
            return;
        }

        if (alivePlyers <= 1)
        {
            Win();
        }
    }
    void Lost()
    {
        resault_Text.gameObject.SetActive(true);
        resault_Text.color = Color.red;
        resault_Text.text = "You lost !";
        OnStopRace?.Invoke();
        Restart_Button.gameObject.SetActive(true);
        isFighting = false;
    }
    void Win()
    {
        resault_Text.gameObject.SetActive(true);
        resault_Text.color = Color.green;
        resault_Text.text = "You win !";
        OnStopRace?.Invoke();
        Restart_Button.gameObject.SetActive(true);
        isFighting = false;
        wall.DOScaleZ(0, 2).OnComplete(()=> { wall.gameObject.SetActive(false); });
    }
    private void LateUpdate()
    {
        var camPos = player != null && !player.carController.isDied ? player.transform.position : Vector3.zero;
        camPos += Offset;
        cam.transform.position = camPos;
        cam.transform.LookAt(camPos - Offset);
    }
}
