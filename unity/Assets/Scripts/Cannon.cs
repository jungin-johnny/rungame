using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject ball;
    public float shootPeriod;
    private float shootResume;

    // Start is called before the first frame update
    void Start()
    {
        shootResume = Random.Range(0.0f, shootPeriod);
    }

    // Update is called once per frame
    void Update()
    {
        shootResume -= Time.deltaTime;
        if (shootResume <= 0.0f)
        {
            shootResume = Random.Range(shootPeriod * 0.8f, shootPeriod * 1.2f);
            Shoot();
        }
    }
    public void Shoot()
    {
        GameObject obj = Instantiate(ball, transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        Rigidbody rigid = obj.GetComponent<Rigidbody>();
        rigid.AddForce(transform.up * 200.0f, ForceMode.Impulse);
        GameObject.Destroy(obj, 10.0f);
    }
}
