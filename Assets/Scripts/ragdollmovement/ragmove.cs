using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragmove : MonoBehaviour
{
    public HingeJoint moveBone;
    public Transform Obj;
    public bool Invert;

    // Update is called once per frame
    void Update()
    {
        JointSpring moveForce = moveBone.spring;
        
        moveForce.targetPosition = Obj.transform.localEulerAngles.x;

        if(moveForce.targetPosition > 180)
            moveForce.targetPosition -= 360;

        moveForce.targetPosition = Mathf.Clamp(moveForce.targetPosition,moveBone.limits.min + 5,moveBone.limits.max - 5);

        if(Invert)
            moveForce.targetPosition *= -1;

        moveBone.spring = moveForce;      
    }
}
