using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class string_body : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshCollider>().convex = true;
        HingeJoint stringbody = gameObject.AddComponent<HingeJoint>();
        stringbody.connectedBody = gameObject.transform.parent.gameObject.GetComponent<Rigidbody>();
    }
}
