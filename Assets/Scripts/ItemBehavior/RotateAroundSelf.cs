using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundSelf : MonoBehaviour
{
    public float rotateCoefficient = 1440;
    

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3(0, 116, BoatEngine.boatRotation_YAngle * rotateCoefficient);
    }
}
