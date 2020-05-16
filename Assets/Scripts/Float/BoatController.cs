using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    private float currentSpeed;
    private Vector3 lastPosition;
    
    public float getCurrentSpeed()
    {
        return currentSpeed;
    }

    private void FixedUpdate()
    {
        CalculateSpeed();
    }

    void CalculateSpeed()
    {
        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

}
