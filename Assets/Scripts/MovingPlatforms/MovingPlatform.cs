using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject[] points;
    private GameObject player;
    private int target;
    public bool moving;
    private Rigidbody rb;
    private bool forward;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = 1;
        moving = true;
        forward = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving)
        {
            float dist = Vector3.Distance(points[target].transform.position, transform.position);
            if (dist > .25){
                Vector3 direction = (points[target].transform.position - transform.position).normalized;
                gameObject.transform.Translate(direction * Time.smoothDeltaTime * speed);
            }
            else{
                if (forward){
                    if (target + 1 > points.Length-1)
                    {
                        target--;
                        forward = false;
                    }
                    else{
                        target++;
                    }
                }
                else{
                    if (target - 1 < 0){
                        target++;
                        forward = true;
                    }
                    else{
                        target--;
                    }
                }  
            } 
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //parent the platform to the player
            collision.gameObject.transform.SetParent(gameObject.transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //unpparent the player to the platform
            collision.gameObject.transform.parent = null;
        }
    }
}
