using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBlock : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        /*
        if (GameManager.instance.currentState == GameState.Playing)
        {
            if (col.CompareTag("Tile") == true)
            {
                Rigidbody rigidbody = col.attachedRigidbody;
                rigidbody.isKinematic = false;
                rigidbody.AddTorque(new Vector3(Random.Range(-250.0f, 250.0f),
                       Random.Range(-250.0f, 250.0f), Random.Range(-250.0f, 250.0f)));

                if (rigidbody != null)
                    rigidbody.useGravity = true;
            }
            else if (col.CompareTag("Lava") == true)
            {
                Destroy(col.gameObject, 5.0f);
            }
        }*/
    }
}
