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

    private bool leftStatus;
    private bool rightStatus;

    
    void Start()
    {
        leftStatus = true;
        rightStatus = false;
        //leftArm.GetComponent<CatchWall>().setReject(false);
        //rightArm.GetComponent<PickUp>().setReject(false);
    }

    public void exchange(int command)
    {
        if (command == 1)
        {
            leftStatus = !leftStatus;
            if(leftStatus == false)
            {
                FixedJoint tempFixed = leftArm.GetComponent<FixedJoint>();
                if(tempFixed != null)
                {
                    tempFixed.connectedBody = null;
                    Destroy(leftArm.GetComponent<FixedJoint>());
                }
                //leftArm.GetComponent<FixedJoint>().connectedBody = null;
            }
            else
            {
                if(leftArm.GetComponent<FixedJoint>() == null)
                {
                    FixedJoint tempFixedJoint = leftArm.AddComponent<FixedJoint>();
                    tempFixedJoint.breakForce = 100;
                }
            }
            GameObject tempCamera = GameObject.Find("Tour Camera");
            tempCamera.GetComponent<StickyUI>().leftImageUIStatusChange(leftStatus);
            //leftArm.GetComponent<CatchWall>().setReject(leftStatus);
            //rightArm.GetComponent<PickUp>().setReject(true);
            //leftArm.GetComponent<CatchWall>().enabled = true;
            //rightArm.GetComponent<PickUp>().enabled = false;
        }
        else if (command == 2)
        {
            
            int temp = rightArm.GetComponent<CatchWall>().setSticky(rightStatus);
            GameObject tempCamera = GameObject.Find("Tour Camera");
            tempCamera.GetComponent<StickyUI>().rightImageUIStatusChange(rightStatus);
            if (rightStatus) Debug.Log("set true");
            else Debug.Log("set false");
            if(temp == 0)
            {
                rightStatus = !rightStatus;
            }
            //rightArm.GetComponent<PickUp>().setReject(rightStatus);
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
