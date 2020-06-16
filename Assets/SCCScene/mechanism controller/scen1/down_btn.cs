using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class down_btn : MonoBehaviour
{
    public GameObject elevater;
    void OnCollisionEnter()
    {
        if (elevater.GetComponent<elevator_controller>().state == 0)
            elevater.GetComponent<elevator_controller>().state = 2;
    }
}
