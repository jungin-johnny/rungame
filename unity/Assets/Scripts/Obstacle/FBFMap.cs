using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBFMap : MonoBehaviour
{
    public float period;
    private float resumeTime;
    public GameObject[] blockObjects;
    private List<GameObject> makeblock = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        MakeTile();
    }

    public void MakeTile()
    {
        foreach(GameObject obj in makeblock)
        {
            Destroy(obj);
        }

        makeblock.Clear();

        for (int col = 0; col < 4; ++col)
        {
            int trabNum = Random.Range(0, 4);
            for (int row = 0; row < 4; ++row)
            {
                int index = Random.Range(0, blockObjects.Length);

                GameObject obj = Instantiate(blockObjects[index], transform);
                obj.transform.localPosition = new Vector3(-29.25f + 19.5f * row, 0.0f, 19.5f + 19.0f * col); ;
                obj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

                if (row == trabNum)
                {
                    obj.GetComponent<FBFObstacle>().isTrap = true;
                }

                makeblock.Add(obj);
            }
        }

        for (int col = 0; col < 4; ++col)
        {
            for (int row = 0; row < 4; ++row)
            {
                int index = Random.Range(0, blockObjects.Length);

                GameObject obj = Instantiate(blockObjects[index], transform);
                obj.transform.localPosition = new Vector3(-29.25f + 19.5f * row, 0.0f, 135f + 19.0f * col); ;
                obj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

                makeblock.Add(obj);
            }
        }

        resumeTime = period;
    }

    // Update is called once per frame
    void Update()
    {
        resumeTime -= Time.deltaTime;

        if (resumeTime <= 0.0f && GameManager.instance.currentState == EGameState.Learning)
        {
            MakeTile();
        }
    }
}
