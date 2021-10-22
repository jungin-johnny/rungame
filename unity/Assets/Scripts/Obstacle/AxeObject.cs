using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class AxeObject : MonoBehaviour
{
    public Ease ease;

    // Start is called before the first frame update
    void Start()
    {
        RotateAxe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RotateAxe()
    {
        if (transform.eulerAngles.z > 180.0f)
        {
            transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 120.0f), 4.0f).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 120.0f), 4.0f).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Cube"))
        {
            collision.rigidbody.AddForce(new Vector3(collision.transform.position.x - collision.contacts[0].point.x, 0.2f, 0.0f) * 30.0f, ForceMode.Impulse);
        }
    }
}
