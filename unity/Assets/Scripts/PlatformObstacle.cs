using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObstacle : MonoBehaviour
{
    public Rigidbody myRigidbody;
    private Vector3 speed;
    // Start is called before the first frame update
    void Start()
    {
        int direction = Random.Range(0, 2);
        if (direction == 0)
        {
            speed = new Vector3(0.75f, 0.0f, 0.0f);
        }
        else
        {
            speed = new Vector3(-0.75f, 0.0f, 0.0f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Mathf.Abs(transform.localPosition.x) > 10.0f)
        {
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -10.0f, 10.0f),
                transform.localPosition.y, transform.localPosition.z);
            speed *= -1;
        }

        myRigidbody.MovePosition(myRigidbody.position + speed * Time.deltaTime);

    }
}
