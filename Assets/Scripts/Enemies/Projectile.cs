using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 dir;
    private Vector3 previousLocation;
    // Start is called before the first frame update
    void Start()
    {
        previousLocation = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        dir =  transform.position - previousLocation;
        previousLocation = transform.position;
    }

}
