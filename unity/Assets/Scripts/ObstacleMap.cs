using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMap : MonoBehaviour
{
    public int minObstacle;
    public int maxObstacle;
    public GameObject[] obstacleObject;
    private List<GameObject> makeblock = new List<GameObject>();
    public float period;
    private float resumeTime;

    // Start is called before the first frame update
    void Start()
    {
        RandomPlaceObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        resumeTime -= Time.deltaTime;

        if (resumeTime <= 0.0f && GameManager.instance.currentState == EGameState.Learning)
        {
            RandomPlaceObstacle();
        }
    }

    private void RandomPlaceObstacle()
    {
        foreach (GameObject obj in makeblock)
        {
            Destroy(obj);
        }

        makeblock.Clear();

        int numObstacle = Random.Range(Mathf.Clamp(minObstacle, 0, minObstacle + 1), maxObstacle);

        for(int index = 0; index < numObstacle; ++index)
        {
            int randomObstacle = Random.Range(0, obstacleObject.Length);            

            GameObject obj = Instantiate(obstacleObject[randomObstacle], transform) as GameObject;
            obj.transform.localPosition = new Vector3(Random.Range(-27.0f, 27.0f), 5.0f, 20.0f + (160.0f / numObstacle) * (index + 1));
            if (randomObstacle < 3)
            {
                obj.transform.localPosition -= new Vector3(0.0f, 4.0f);
            }
            obj.transform.rotation = Quaternion.Euler(-180.0f, Random.Range(0.0f, 360.0f), -180.0f);

            makeblock.Add(obj);
        }

        resumeTime = period;
    }
}
