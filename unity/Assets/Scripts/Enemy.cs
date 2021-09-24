using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Rigidbody myRigid;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.0f, 0.0f, -Time.deltaTime * speed);
        /*
        myRigid.AddForce(0.0f, 0.0f, -5.0f);
        */
        if (transform.position.y < -10.0f)
            Destroy(gameObject);
    }
}
