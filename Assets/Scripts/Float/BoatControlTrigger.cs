using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControlTrigger : MonoBehaviour
{
    static public bool ifEnterTrigger = false;
    private bool ifSure = false; // if user want to drive the boat
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (ifSure)
            {
                Debug.Log("enter");
                ifEnterTrigger = true;
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (ifSure)
            {
                Debug.Log("stay");
                ifEnterTrigger = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("exit");
            ifEnterTrigger = false;
            ifSure = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ifSure = !ifSure;
            if (!ifSure)
                ifEnterTrigger = false;
        }
    }
}
