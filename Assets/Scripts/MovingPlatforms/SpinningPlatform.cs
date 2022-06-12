using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPlatform : MonoBehaviour
{
    public float rotationsPerMinute = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);
    }

    void OnCollisionEnter(Collision collision)
    {


        collision.transform.SetParent(transform);

    }

    void OnCollisionExit(Collision collision)
    {
            //unpparent the player to the platform
            collision.transform.parent = null;
    }
}
