using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (GameManager.instance.currentState == GameState.Playing)
        {
            if (col.CompareTag("Tile") == true)
            {
                Destroy(col.gameObject);
            }
        }
    }
}
