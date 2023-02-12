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
    [SerializeField] Vector2 randomDelayForParachute = new Vector2(4, 10);
    [SerializeField] Parachute parachutePrefab;
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
        StartCoroutine(ParachutteGenerator());
    }

    IEnumerator ParachutteGenerator()
    {
        yield return new WaitUntil(() => isFighting == true);
        while (isFighting)
        {
            Transform lastPointT = null;

            yield return new WaitForSeconds(UnityEngine.Random.Range(randomDelayForParachute.x, randomDelayForParachute.y));
            if (isFighting)
            {
                var p = Instantiate(parachutePrefab);
                lastPointT = GetRandomPoint(lastPointT);
                p.transform.position = lastPointT.position + Vector3.up * 50;
                p.transform.rotation = Quaternion.identity;
                yield return new WaitUntil(() => p == null);
            }
        }
    }
    public void PlayerDied()
    {
        alivePlyers--;
        playersLeft_Text.text = alivePlyers.ToString();
        if (player._CarController.isDied)
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
        foreach (var bot in botsManager.botsList)
        {
            bot._CarController.PlayCharacterAnimation(true,-1);
        }
        player._CarController.PlayCharacterAnimation(false,-1);
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
        foreach (var bot in botsManager.botsList)
        {
            bot._CarController.PlayCharacterAnimation(false, -1);
        }
        player._CarController.PlayCharacterAnimation(true, -1);
        cam.DOFieldOfView(13, 3);
    }
    private void FixedUpdate()
    {
        var camPos = player != null && !player._CarController.isDied ? player.transform.position : Vector3.zero;
        camPos += Offset;
        cam.transform.position = camPos;
        Vector3 lookposition = player != null && player._CarController.isDied ? player.transform.position : camPos - Offset;
        Quaternion targetRot = Quaternion.LookRotation(lookposition - camPos);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRot, Time.deltaTime * 5);
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
