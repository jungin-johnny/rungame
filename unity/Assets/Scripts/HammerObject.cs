using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, Time.deltaTime * 45.0f, 0.0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Cube"))
        {
            Vector3 fowardvec = (transform.localRotation * Vector3.right) * 20.0f;

            collision.rigidbody.AddForce(fowardvec, ForceMode.Impulse);
        }
    }
}
