using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] TMP_Text counting_Text, playersLeft_Text, resault_Text, winCount_text, loseCount_text;
    [SerializeField] Button Restart_Button;
    [SerializeField] Transform wall;
    [SerializeField] Transform pointsParet;

    Sequence countingSeq;
    int alivePlyers;
    internal bool isFighting = false;

    string winCountStr = "winCount";
    string loseCountStr = "lostCount";
    internal Transform[] randomPointsInArenaArray = new Transform[0];


    int winCount
    {
        get => PlayerPrefs.GetInt(winCountStr, 0);
        set => PlayerPrefs.SetInt(winCountStr, value);
    }
    int loseCount
    {
        get => PlayerPrefs.GetInt(loseCountStr, 0);
        set => PlayerPrefs.SetInt(loseCountStr, value);
    }

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
        RefresCountTexts();
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
        loseCount++;
        RefresCountTexts();
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
        winCount++;
        RefresCountTexts();
    }
    private void LateUpdate()
    {
        var camPos = player != null && !player.carController.isDied ? player.transform.position : Vector3.zero;
        camPos += Offset;
        cam.transform.position = camPos;
        cam.transform.LookAt(camPos - Offset);
    }
    void RefresCountTexts()
    {
        winCount_text.text = winCount.ToString();
        loseCount_text.text = loseCount.ToString();
    }
    public void InitPoints()
    {
        if (randomPointsInArenaArray.Length < 1)
        {
            var points = pointsParet.GetComponentsInChildren<Transform>().ToList();
            points.Remove(pointsParet);
            randomPointsInArenaArray = points.ToArray();
        }
    }
    public Transform GetRandomPoint(Transform lastTarget)
    {
        InitPoints();
        List<Transform> listOfRandomPoints = new List<Transform>();
        listOfRandomPoints.AddRange(randomPointsInArenaArray);
        if (listOfRandomPoints.Contains(lastTarget))
        {
            listOfRandomPoints.Remove(lastTarget);
        }
        var point = listOfRandomPoints[UnityEngine.Random.Range(0, listOfRandomPoints.Count)];
        return point;
    }
}
