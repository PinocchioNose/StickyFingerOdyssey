using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchWall : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "catchable")
    //    {
    //        Debug.Log("collision here");
    //        this.GetComponent<FixedJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
    //    }
    //}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "catchable")
        {
            // creates joint
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            // sets joint position to point of contact
            joint.anchor = col.contacts[0].point;
            // conects the joint to the other object
            joint.connectedBody = col.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
            // Stops objects from continuing to collide and creating more joints
            joint.enableCollision = false;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.GetComponent<FixedJoint>().connectedBody = null;

        }
    }
}
