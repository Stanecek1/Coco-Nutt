using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject target;
    CharacterStateManager character;
    void Start()
    {
        target = transform.parent.gameObject;
        character = GameObject.FindWithTag("Player").GetComponent<CharacterStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            character.targets.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            character.targets.Remove(target);
        }
    }
}
