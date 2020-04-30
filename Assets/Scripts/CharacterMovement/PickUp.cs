using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    private bool isRejecting;

    // Start is called before the first frame update
    private float armRange;
    public float maxArmRange;
    public GameObject leftArm2;
    public float elongSpeed;

    void Start()
    {
        isRejecting = true;
        armRange = 0.0f;
    }
    
    public void setReject(bool sta)
    {
        isRejecting = sta;
    }

    protected void handCtrl()
    {
        //手臂延长
        //用肩膀到前臂的伸长线去做
        if (Input.GetMouseButton(1))
        {
            if (armRange <= maxArmRange)
            {
                leftArm2.transform.Translate(new Vector3(0, 1, 0)
                    * elongSpeed * Time.deltaTime, Space.Self);

                armRange += elongSpeed * Time.deltaTime;
                //Debug.Log(armRange);
            }

        }
        else
        {
            if (armRange > 0)
            {

                leftArm2.transform.Translate(new Vector3(0, 1, 0)
                    * -elongSpeed * Time.deltaTime, Space.Self);
                armRange -= elongSpeed * Time.deltaTime;
                //Debug.Log(armRange);
            }
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isRejecting) return;
        if(collision.gameObject.tag == "pickable")
        {
            //Debug .Log("collision happened!");
            this.GetComponent<FixedJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isRejecting)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                this.GetComponent<FixedJoint>().connectedBody = null;
                //Debug.Log("release");
            }
            handCtrl();
        }
        else
        {
            //拒绝作动之后强制回收
            if (armRange > 0)
            {

                leftArm2.transform.Translate(new Vector3(0, 1, 0)
                    * -elongSpeed * Time.deltaTime, Space.Self);
                armRange -= elongSpeed * Time.deltaTime;
                //Debug.Log(armRange);
            }
            this.GetComponent<FixedJoint>().connectedBody = null;
            //Debug.Log("release");
        }

    }
}
