using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObstacle : MonoBehaviour
{
    public Rigidbody myRigidbody;
    private Vector3 speed = new Vector3(0.0f, 0.0f, (-170.0f/7.0f) * 0.1f);
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + speed * Time.deltaTime);
    }
}
