using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] Obstacle obstacle;
    [SerializeField] Vector2 delayToSpwanObstacle;
    [SerializeField] float obstacleLifetime = 5;
    void Start()
    {
        obstacle.gameObject.SetActive(false);
        StartCoroutine(SpwanObstacles());
    }

    IEnumerator SpwanObstacles()
    {
        Transform lastPointT = null;
        yield return new WaitUntil(() => InGameManager.Instance.isFighting == true);
        while (InGameManager.Instance.isFighting)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(delayToSpwanObstacle.x, delayToSpwanObstacle.y));
            float duration = obstacleLifetime;
            if (InGameManager.Instance.isFighting)
            {
                lastPointT = InGameManager.Instance.GetRandomPoint(lastPointT);
                obstacle.transform.position = lastPointT.position + new Vector3(UnityEngine.Random.Range(-5.0f, 5.0f), 0, UnityEngine.Random.Range(-5.0f, 5.0f));
                obstacle.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
                obstacle.transform.localScale = Vector3.zero;
                obstacle.gameObject.SetActive(true);
                obstacle.IsActive = true;
                while (duration >= 0)
                {
                    yield return new WaitForFixedUpdate();
                    duration -= Time.fixedDeltaTime;
                    obstacle.transform.localScale = Vector3.Lerp(obstacle.transform.localScale, Vector3.one, Time.fixedDeltaTime * 10);
                    
                }
                yield return obstacle.transform.DOScale(0, .5f).WaitForCompletion();
                obstacle.IsActive = false;
                obstacle.gameObject.SetActive(false);
            }
        }
    }
}
