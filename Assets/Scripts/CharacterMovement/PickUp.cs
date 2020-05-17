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

    private int handCanElong; // 1只能伸长，0保持暂停，-1只能缩短
    private int nextHandStatus;
    void Start()
    {
        handCanElong = 0;
        nextHandStatus = 0;

        isRejecting = false;
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
        if (handCanElong == 1)
        {
            if (armRange <= maxArmRange)
            {
                leftArm2.transform.Translate(new Vector3(0, 1, 0)
                    * elongSpeed * Time.deltaTime, Space.Self);

                armRange += elongSpeed * Time.deltaTime;
                //Debug.Log(armRange);
            }
            else
            {
                handCanElong = 0;
            }

        }
        else if(handCanElong == -1)
        {
            if (armRange > 0)
            {

                leftArm2.transform.Translate(new Vector3(0, 1, 0)
                    * -elongSpeed * Time.deltaTime, Space.Self);
                armRange -= elongSpeed * Time.deltaTime;
                //Debug.Log(armRange);
            }
            else if(armRange <= 0)
            {
                handCanElong = 0;
            }
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (isRejecting) return;
        handCanElong = 0;
        Debug.Log("collied");
        if (collision.gameObject.tag == "pickable")
        {
            
            if (this.GetComponent<FixedJoint>() != null)
            {
                Debug.Log("connected");
                this.GetComponent<FixedJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(!isRejecting)
        //{
        //    if (Input.GetKeyDown(KeyCode.F))
        //    {
        //        this.GetComponent<FixedJoint>().connectedBody = null;
        //        //Debug.Log("release");
        //    }
        //    handCtrl();
        //}
        //else
        //{
        //    //拒绝作动之后强制回收
        //    if (armRange > 0)
        //    {

        //        leftArm2.transform.Translate(new Vector3(0, 1, 0)
        //            * -elongSpeed * Time.deltaTime, Space.Self);
        //        armRange -= elongSpeed * Time.deltaTime;
        //        //Debug.Log(armRange);
        //    }
        //    this.GetComponent<FixedJoint>().connectedBody = null;
        //    //Debug.Log("release");
        //}
        if(Input.GetMouseButtonDown(0))
        {
            if (nextHandStatus == 0)
            {
                handCanElong = 1;
                nextHandStatus = -1;
            }
            else
            {
                handCanElong = nextHandStatus;
                nextHandStatus = -nextHandStatus;
            }
            
        }
        handCtrl();
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    if(this.GetComponent<FixedJoint>() != null)
        //    {
        //        this.GetComponent<FixedJoint>().connectedBody = null;
        //    }
        //}
    }
}
