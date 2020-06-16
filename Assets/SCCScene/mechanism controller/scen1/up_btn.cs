using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class up_btn : MonoBehaviour
{
    public GameObject elevater;
    void OnCollisionEnter(Collision co)
    {
        //Debug.Log(co.gameObject.name);
        if(elevater.GetComponent<elevator_controller>().state == 0)
            elevater.GetComponent<elevator_controller>().state = 1;
    }
}
