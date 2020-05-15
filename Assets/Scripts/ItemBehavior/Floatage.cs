using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floatage : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        if (other.gameObject.GetComponent<FloatObject>())
        {
            other.gameObject.GetComponent<FloatObject>().setIsInWater(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        if (other.gameObject.GetComponent<FloatObject>())
        {
            other.gameObject.GetComponent<FloatObject>().setIsInWater(false);
        }
    }
}
