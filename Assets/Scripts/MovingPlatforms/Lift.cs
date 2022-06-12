using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float liftHeight;
    private float startHeight;


    private Vector3 startPosition;
    public GameObject endPosition;
    public int speed;

    public bool lifting = false;

    private Vector3 from;
    private Vector3 to;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.GetComponent<Rigidbody>().position;
        startHeight = gameObject.GetComponent<Rigidbody>().position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lifting){
            //distance between the lift and the end position
            float dist = Vector3.Distance(endPosition.transform.position, transform.position);
            //if were not at the end position
            if (dist > .25){

                Vector3 direction = (endPosition.transform.position - transform.position).normalized;
                gameObject.transform.Translate(direction * Time.smoothDeltaTime * speed);
                //move to end position
                //gameObject.transform.Translate(Vector3.up * Time.smoothDeltaTime * speed);
            }
        }
        else
        {
            //distance between the lift and the start position
            float dist = Vector3.Distance(startPosition, transform.position);
            //if were not at the start position
            if (dist > .25){
                //move back to start position
                Vector3 direction = (startPosition - transform.position).normalized;
                gameObject.transform.Translate(direction * Time.smoothDeltaTime * speed);
                //gameObject.transform.Translate(Vector3.down * Time.smoothDeltaTime * speed);
            }
        }
    }




}
