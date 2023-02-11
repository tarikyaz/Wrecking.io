using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotsManager : MonoBehaviour
{
    [SerializeField] Transform pointsParet;
    [SerializeField] BotController botPrefab;
    public int NumOfBots = 3;
    Transform[] randomPointsInArenaArray = new Transform[0];
    List<BotController> botsList = new List<BotController>();
    private void Start()
    {
        InitBots();
    }

    private void InitBots()
    {
        InitPoints();
        List<Transform> availablePosList = new List<Transform>();
        availablePosList.AddRange(randomPointsInArenaArray);
        for (int i = 0; i < NumOfBots; i++)
        {
            BotController newBot = Instantiate(botPrefab);
            var posT = availablePosList[UnityEngine.Random.Range(0, availablePosList.Count)];
            newBot.transform.position = posT.position;
            newBot.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
            botsList.Add(newBot);
            availablePosList.Remove(posT);
        }
    }

    public Transform GetRandomPoint(Transform lastTarget)
    {
        List<Transform> listOfRandomPoints = new List<Transform>();
        listOfRandomPoints.AddRange(randomPointsInArenaArray);
        if (listOfRandomPoints.Contains(lastTarget))
        {
            listOfRandomPoints.Remove(lastTarget);
        }
        var point = listOfRandomPoints[UnityEngine.Random.Range(0, listOfRandomPoints.Count)];
        return point;
    }

    private void InitPoints()
    {
        if (randomPointsInArenaArray.Length < 1)
        {
            var points = pointsParet.GetComponentsInChildren<Transform>().ToList();
            points.Remove(pointsParet);
            randomPointsInArenaArray = points.ToArray();
        }
    }
}
