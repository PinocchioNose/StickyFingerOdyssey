using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;

    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private Vector3 move_forward = Vector3.zero;

    private void FixedUpdate()
    {
        rigidbody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if (transform.position.y < waveHeight)
        {
            float displacemenMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacemenMultiplier, 0f),transform.position , ForceMode.Acceleration);
            rigidbody.AddForce(displacemenMultiplier * -rigidbody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidbody.AddTorque(displacemenMultiplier * -rigidbody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        
        }

        //if (Input.GetKey(KeyCode.I))
        //{
        //    rigidbody.AddForce(rigidbody.transform.forward * 2,ForceMode.Acceleration);
        //}
        //if (Input.GetKey(KeyCode.L))
        //{
        //    rigidbody.
        //}
    }
}
