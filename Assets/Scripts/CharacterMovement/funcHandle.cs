using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create By YiboZhong, a85392708@outlook.com
// funcHandle.cs
// for players to Use 'Q' and 'E'
// to activate the function of arm
// by enable script

public class funcHandle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftArm;
    public GameObject rightArm;

    
    void Start()
    {
        leftArm.GetComponent<CatchWall>().setReject(false);
        rightArm.GetComponent<PickUp>().setReject(true);
    }

    void exchange(int command)
    {
        if (command == 1)
        {
            leftArm.GetComponent<CatchWall>().setReject(false);
            rightArm.GetComponent<PickUp>().setReject(true);
            //leftArm.GetComponent<CatchWall>().enabled = true;
            //rightArm.GetComponent<PickUp>().enabled = false;
        }
        else if (command == 2)
        {
            leftArm.GetComponent<CatchWall>().setReject(true);
            rightArm.GetComponent<PickUp>().setReject(false);
            //leftArm.GetComponent<CatchWall>().enabled = false;
            //rightArm.GetComponent<PickUp>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            exchange(1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            exchange(2);
        }
    }
}
