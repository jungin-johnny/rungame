using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.0f, 0.0f, Time.deltaTime * 1.5f);

        if (transform.position.z > 40.0f && (GameManager.instance.currentState == EGameState.Learning || GameManager.instance.currentState == EGameState.Testing))
            transform.position = new Vector3(0.0f, 0.0f, -10.0f);
    }
}
