using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    //Start is called before the first frame update
    public int count = 0;
    public int id;
    private void OnTriggerEnter(Collider other)
    {
        count++;
        GameEvents.current.DoorwayTriggerEnter(id);
    }

    private void OnTriggerExit(Collider other)
    {
        count--;
        if (count == 0)
        {
            GameEvents.current.DoorwayTriggerExit(id);
        }
    }
}
