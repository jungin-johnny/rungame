using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public GameObject[] blockObjects;
    public float blockPeriod;
    private float resumeBlockTime;

    // Start is called before the first frame update
    void Start()
    {
        resumeBlockTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        resumeBlockTime -= Time.deltaTime;
        if(resumeBlockTime <= 0.0f)
        {
            resumeBlockTime = blockPeriod;
            MakeMoveBlock();
        }
    }

    private void MakeMoveBlock()
    {
        int randomHole = Random.Range(0, 5);

        for(int index = 0; index < 5; ++index)
        {
            if (index == randomHole)
                continue;

            Vector3 pos = new Vector3(-31.0f + 15.5f * index, 9.5f, 190.0f);
            Vector3 scale = new Vector3(3.75f, 3.75f, 3.7f);

            GameObject obj = Instantiate(blockObjects[index], transform);
            obj.transform.localPosition = pos;
            obj.transform.localScale = scale;
        }
    }
}
