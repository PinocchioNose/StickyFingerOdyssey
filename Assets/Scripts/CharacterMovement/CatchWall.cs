using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchWall : MonoBehaviour
{
    public enum flyStatus
    {
        away,
        approach,
        standBy
    }

    public float strength;
    public float catapultPower;
    public GameObject theBody;


    //记录勾爪点
    private Vector3 contactPoint;
    private FixedJoint joint;

    //回收参数
    private float minRetrieveDist = 0.7f;
    private flyStatus status = flyStatus.standBy;
    private Vector3 lastPos;
    private Vector3 originPos;
    private Quaternion originRotate;

    private float rotateSpeed = 90.0f;
    private float retrieveSpeed = 2.0f;


    //timer
    private float lastTime;
    //private float lastTimeCata;

    //catchlock 为了在解除勾爪时不再次被某障碍物锁定
    private bool catchlock;

    // Start is called before the first frame update
    void Start()
    {
        catchlock = true;
        lastTime = Time.time;
        originPos = this.transform.localPosition;
        lastPos = originPos;
        originRotate = this.transform.localRotation;
        //lastTimeCata = Time.time;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "catchable")
    //    {
    //        Debug.Log("collision here");
    //        this.GetComponent<FixedJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
    //    }
    //}


    //建立勾爪锚点
    void OnCollisionEnter(Collision col)
    {
        if(catchlock)
        {
            if (col.gameObject.tag == "catchable")
            {
                // creates joint
                joint = gameObject.AddComponent<FixedJoint>();
                // sets joint position to point of contact
                joint.anchor = col.contacts[0].point;
                contactPoint = col.contacts[0].point;
                // conects the joint to the other object
                joint.connectedBody = col.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
                // Stops objects from continuing to collide and creating more joints
                joint.enableCollision = false;

            }
        }
        
            
    }
    
    //朝向手臂勾住的点弹射
    void catapult()
    {
        if(this.GetComponent<FixedJoint>() != null)
        {
            if (Time.time - lastTime > 1f)
            {
                theBody.GetComponent<Rigidbody>().AddForce((contactPoint - theBody.transform.position).normalized
                    * catapultPower, ForceMode.Impulse);

                lastTime = Time.time;
            }
        }
        
        
    }

    void retrieve()
    {
        //float dist = (this.transform.localPosition - originPos).magnitude;
        if(status == flyStatus.approach)
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, originRotate, rotateSpeed * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, retrieveSpeed * Time.deltaTime);
            catchlock = true;
        }
    }

    //时刻监视回收状态
    void retrieveMonitor()
    {
        float dist1, dist2;
        Vector3 tempVector = this.transform.localPosition;
        dist1 = (lastPos - originPos).magnitude;
        dist2 = (tempVector - originPos).magnitude;
        if(dist1 > dist2)
        {
            status = flyStatus.approach;
            if(dist2 < minRetrieveDist * minRetrieveDist && this.GetComponent<FixedJoint>() == null)
            {
                retrieve();
            }
        }
        else if(dist1 < dist2)
        {
            status = flyStatus.away;
        }
        else
        {
            status = flyStatus.standBy;
        }
        lastPos = tempVector;
    }

    // Update is called once per frame
    void Update()
    {
        retrieveMonitor();
        //retrieve();
        if(Input.GetMouseButtonDown(0))
        {
            //发射勾爪
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(this.transform.up * strength, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(0) && this.GetComponent<FixedJoint>() != null)
        {
            catchlock = false;
            if(this.GetComponent<FixedJoint>() != null)
            {
                Destroy(this.GetComponent<FixedJoint>());
            }
            else
            {
                Debug.Log("Fixed Joint doesn't exist!");
            }

        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            catapult();
        }
    }
}
