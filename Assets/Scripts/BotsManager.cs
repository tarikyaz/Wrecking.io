using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotsManager : MonoBehaviour
{
    [SerializeField] Transform pointsParet;
    Transform[] randomPointsInArenaArray = new Transform[0];

    public Transform GetRandomPoint( Transform lastTarget)
    {
        if (randomPointsInArenaArray.Length < 1)
        {
            var points = pointsParet.GetComponentsInChildren<Transform>().ToList();
            points.Remove(pointsParet);
            randomPointsInArenaArray = points.ToArray();
        }
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
