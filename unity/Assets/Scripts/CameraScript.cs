using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 startPosition;
    public Transform chaseTransform;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(chaseTransform != null)
        {
            transform.localPosition = startPosition + new Vector3(chaseTransform.position.x, 0.0f, chaseTransform.position.z);
        }
        //transform.localPosition = startPosition + new Vector3(chaseTransform.position.x - (chaseTransform.position.x % 100.0f), 0.0f, chaseTransform.position.z);
    }
}
