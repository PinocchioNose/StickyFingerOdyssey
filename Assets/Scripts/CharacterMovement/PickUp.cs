using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "pickable")
        {
            Debug .Log("collision happened!");
            this.GetComponents<SpringJoint>()[1].connectedBody = collision.gameObject.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            this.GetComponents<SpringJoint>()[1].connectedBody = null;
            //Debug.Log("release");
        }
    }
}
