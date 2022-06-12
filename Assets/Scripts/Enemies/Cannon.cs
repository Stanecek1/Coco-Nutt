using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject bullet;


    private float nextActionTime = 0.0f;
    public float period = 1f;
    public float bulletSpeed;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            GameObject instBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 90));
            instBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        } 
    }
}
