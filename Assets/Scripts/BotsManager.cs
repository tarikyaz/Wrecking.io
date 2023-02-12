using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotsManager : MonoBehaviour
{
    [SerializeField] BotController botPrefab;
    public int NumOfBots = 3;
    internal List<BotController> botsList = new List<BotController>();
    private void Start()
    {
        InitBots();
    }

    private void InitBots()
    {
        InGameManager.Instance.InitPoints();
        List<Transform> availablePosList = new List<Transform>();
        availablePosList.AddRange(InGameManager.Instance.randomPointsInArenaArray);
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




}
