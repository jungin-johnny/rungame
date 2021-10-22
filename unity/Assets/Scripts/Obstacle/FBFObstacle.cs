using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBFObstacle : MonoBehaviour
{
    public Rigidbody myRigidbody;
    public bool isTrap = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(isTrap == true && collision.transform.CompareTag("Cube"))
        {
            myRigidbody.isKinematic = false;
            myRigidbody.useGravity = true;
        }
    }
}
