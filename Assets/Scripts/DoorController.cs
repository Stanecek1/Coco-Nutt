using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool triggered = false;
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit += OnDoorwayClose;
    }

    // Update is called once per frame
    private void OnDoorwayOpen(int id)
    {
        if (id == this.id)
        {
            if (triggered == false)
            {
                LeanTween.moveLocalY(gameObject, -20f, 1.5f).setEaseOutQuad();
                triggered = true;
            }
        }
    }

    private void OnDoorwayClose(int id)
    {
        if (id == this.id)
        {

            if (triggered == true)
            {
                LeanTween.moveLocalY(gameObject, 0f, 1.5f).setEaseOutQuad();
                triggered = false;
            }
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit -= OnDoorwayClose;
    }
}
