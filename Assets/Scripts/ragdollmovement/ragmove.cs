using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragmove : MonoBehaviour
{
    public HingeJoint moveBone;
    public Transform Obj;
    public bool Invert;

    private Transform[] calfTrans;
    
    // Vector3 modifyAng;
    void Start()
    {
        calfTrans = GetComponentsInChildren<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        JointSpring moveForce = moveBone.spring;
        
        moveForce.targetPosition = Obj.transform.localEulerAngles.x;
        // Component[] children = GetComponentsInChildren(typeof(Transform));
        // Debug.Log(children[1].name);
        // Debug.Log(calfTrans[1].name);
        // Debug.Log(calfTrans[1].name);
        // Debug.Log(calfTrans[1].localEulerAngles.x);
        // if(calfTrans[1].localEulerAngles.x > 180){
        //     // Debug.Log("rotation error");
        //     // calfTrans[1].localEulerAngles = new Vector3(0, 0, 0);
        //     // modifyAng = calfTrans[1].localEulerAngles;
        //     // modifyAng.x = 0;
        //     // calfTrans[1].localEulerAngles = modifyAng;
        //     // calfTrans[1].localEulerAngles = new Vector3(0, y, z);
        // }
    
        if(moveForce.targetPosition > 180)
            moveForce.targetPosition -= 360;

        moveForce.targetPosition = Mathf.Clamp(moveForce.targetPosition,moveBone.limits.min + 5,moveBone.limits.max - 5);

        if(Invert)
            moveForce.targetPosition *= -1;

        moveBone.spring = moveForce;      
    }
}
