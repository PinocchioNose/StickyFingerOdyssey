using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchWall : MonoBehaviour
{

    // 标记脚本是否起作用的变量，通过一个函数对外提供接口
    private bool isRejecting;
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
    private Rigidbody catchedObjectRb;
    private GameObject catchedObject;

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

    //表示是否有粘性
    private bool sticky;

    // Start is called before the first frame update
    void Start()
    {
        catchlock = true;
        lastTime = Time.time;
        originPos = this.transform.localPosition;
        lastPos = originPos;
        originRotate = this.transform.localRotation;
        isRejecting = false;

        sticky = true;
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


    public int setSticky(bool theStatus)
    {
        if(catchlock)
        {
            sticky = theStatus;
            Debug.Log("set success");
            return 0;
        }
        else
        {
            Debug.Log("set failed");
            return 1;
        }
        
    }

    //建立勾爪锚点
    void OnCollisionEnter(Collision col)
    {
        if(isRejecting)
        {
            return;
        }
        if (catchlock)
        {
            if (col.gameObject.tag == "catchable")
            {
                if(sticky)
                {
                    // creates joint
                    //this.transform.position = col.contacts[0].point;
                    joint = gameObject.AddComponent<FixedJoint>();
                    
                    // sets joint position to point of contact
                    Debug.Log("105 executed.");
                    //joint.anchor = col.contacts[0].point;
                    contactPoint = col.contacts[0].point;

                    // 创建一个空物体来钩住
                    Debug.Log("create empty");
                    catchedObject = new GameObject("tempCatchedObject");
                    catchedObject.transform.position = contactPoint;
                    catchedObject.AddComponent<Rigidbody>();
                    catchedObjectRb = catchedObject.GetComponent<Rigidbody>();
                    catchedObjectRb.constraints = RigidbodyConstraints.FreezeAll;

                    this.transform.position = contactPoint;
                    
                    joint.connectedBody = catchedObject.GetComponent<Rigidbody>();
                    
                    // conects the joint to the other object
                    // joint.connectedBody = col.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
                    // Stops objects from continuing to collide and creating more joints
                    joint.enableCollision = false;
                }
                else
                {
                    Vector3 temp = (theBody.transform.position - col.contacts[0].point).normalized * catapultPower * 0.5f;
                    temp.y = 5.0f;
                    theBody.GetComponent<Rigidbody>().AddForce(temp, ForceMode.Impulse);
                }

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

    public void setReject(bool sta)
    {
        isRejecting = sta;
    }


    

    public void retrieve()
    {
        //float dist = (this.transform.localPosition - originPos).magnitude;
        
        this.GetComponent<Rigidbody>().isKinematic = true;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, originRotate, rotateSpeed * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, retrieveSpeed * Time.deltaTime);
        catchlock = true;

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
            if (dist2 < minRetrieveDist * minRetrieveDist && this.GetComponent<FixedJoint>() == null)
            {
                retrieve();
            }
            //retrieve();
        }
        else if(dist1 < dist2)
        {
            status = flyStatus.away;
        }
        else
        {
            status = flyStatus.standBy;
        }
        //Debug.Log((this.transform.localPosition - originPos).magnitude);
        
        lastPos = tempVector;
    }

    // Update is called once per frame
    void Update()
    {

        // 拉伸太多就会收回
        if ((this.transform.localPosition - originPos).magnitude >= 1.2)
        {
            if (this.GetComponent<FixedJoint>() != null)
            {
                Destroy(this.GetComponent<FixedJoint>());
                Destroy(catchedObject);
                Debug.Log("destoryed");
            }
            this.GetComponent<Rigidbody>().isKinematic = true;
            transform.localRotation = originRotate;
            transform.localPosition = originPos;
            catchlock = true;
        }
        // 如果手臂末端离抓取点太远也立即收回
        if ((this.transform.position - contactPoint).magnitude >= 0.5)
        {
            if (this.GetComponent<FixedJoint>() != null)
            {
                Destroy(this.GetComponent<FixedJoint>());
                Destroy(catchedObject);
                Debug.Log("destoryed");
                this.GetComponent<Rigidbody>().isKinematic = true;
                transform.localRotation = originRotate;
                transform.localPosition = originPos;
                catchlock = true;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(this.transform.position);
            Debug.Log(contactPoint);
            Debug.Log((this.transform.position - contactPoint).magnitude);
        }
        if (!isRejecting)
        {
            retrieveMonitor();
            //retrieve();
            if (Input.GetMouseButtonDown(1))
            {
                //发射勾爪
                
                Rigidbody rb = this.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddForce(this.transform.up * strength, ForceMode.Impulse);
                Debug.Log("232:go!");
            }
            if (Input.GetMouseButtonDown(1) && this.GetComponent<FixedJoint>() != null)
            {
                catchlock = false;
                if (this.GetComponent<FixedJoint>() != null)
                {
                    Destroy(this.GetComponent<FixedJoint>());
                    Destroy(catchedObject);
                    Debug.Log("destoryed");
                }
                else
                {
                    // Debug.Log("Fixed Joint doesn't exist!");
                }
                
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                catapult();
            }
        }
        else
        {
            if(status != flyStatus.standBy)
            {
                this.GetComponent<Rigidbody>().isKinematic = true;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, originRotate, rotateSpeed * Time.deltaTime);
                transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, retrieveSpeed * Time.deltaTime);
                

                if (this.GetComponent<FixedJoint>() != null)
                {
                    Destroy(this.GetComponent<FixedJoint>());
                }
                catchlock = true;
            }
            
        }
    }
}
