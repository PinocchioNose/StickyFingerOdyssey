using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider centralCapcollide;
    public HingeJoint[] DeadJoint; 
    [Space(20)]
    // public CapsuleCollider collcap;

    public HingeJoint hj1, hj2;
    public JointSpring hs1, hs2;
    public float SpringMin = 30, SpringMax = 300;

    [Space(20)]
    public float Resist = 10;//collide to kill
    public Animator anim;
    bool Morto = false;
    public float _Velocity;

    [Space(20)]
    // public bool AtivarAutoConserto;  
    // public Transform checkRootable; 
    public bool Correction;
    [Range(0,5)]
    public float sensitivity = 1f;
    // public float MinRoot, MaxRoot;
    // public float slope; 
    // private float pretime; 
    #region jump
    public float Y_Force_Max = 100;
    [Tooltip("向上跳跃的力相对于向前跳跃的力的比例")] [Range(0, 2)]
    public float Ratio = 0.8f;
    [Tooltip("增量")] [Range(0, 1)]
    public float increment_ratio = 1f;
    static public float jump_force = 0;
    private float y_jump_force = 0;
    private float x_jump_force = 0;
    private Vector3 jump_vector;
    private bool isJumping;
    static public bool isCharging = false; // use for charge jump UI
    #endregion

    [Space(20)]
    public float maxArmRange;
    private float armRange;
    public GameObject leftArm2;
    public float elongSpeed;

    [Space(20)]
    public float rotateSpeed = 90.0f;
    public GameObject fatherArm1;
    public GameObject fatherArm2;

    //void OnCollisionEnter(Collision col)
    //{
    //    if (col.relativeVelocity.magnitude > Resist) 
    //    {
    //        centralCapcollide.enabled = false;
    //        rb.constraints = RigidbodyConstraints.None;
    //        for (int x = 0; x < DeadJoint.Length; x++)
    //        {
    //            DeadJoint[x].useSpring = false;
    //        }
    //        anim.SetBool("ifHalt", true);
    //        Morto = true;
    //    }
    //}
    //protected void handCtrl()
    //{
    //    //手臂延长
    //    //用肩膀到前臂的伸长线去做
    //    if (Input.GetMouseButton(1))
    //    {
    //        if (armRange <= maxArmRange)
    //        {
    //            leftArm2.transform.Translate(new Vector3(0,1,0)
    //                * elongSpeed * Time.deltaTime, Space.Self);

    //            armRange += elongSpeed * Time.deltaTime;
    //            //Debug.Log(armRange);
    //        }

    //    }
    //    else
    //    {
    //        if (armRange > 0)
    //        {

    //            leftArm2.transform.Translate(new Vector3(0, 1, 0)
    //                * -elongSpeed * Time.deltaTime, Space.Self);
    //            armRange -= elongSpeed * Time.deltaTime;
    //            //Debug.Log(armRange);
    //        }
    //    }



    //}

    void ChargeJump()
    {
        // charge
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            if (jump_force < Y_Force_Max)
                jump_force += increment_ratio;
            isCharging = true;
            //Debug.Log("jump_force = " + jump_force);
        }

        if (!Input.GetKey(KeyCode.Space) && jump_force!=0 && !isJumping)
        {
            isJumping = true; 
            isCharging = false;
            jump_vector = new Vector3(0, jump_force * Ratio, 0);
            jump_vector += this.transform.forward * jump_force;
        }
        
    }

    void JumpCtrl()
    {
        if (isJumping)
        {
            rb.AddForce(jump_vector * 1.5f, ForceMode.Impulse);
            isJumping = false;
            jump_force = 0;
        }
    }

    void MoveCtrl()
    {
        // if(Input.GetAxis("Mouse X")!=0)
        // {
        //     transform.Rotate(new Vector3(0,Input.GetAxis("Mouse X"),0));
        // }
        if(transform.eulerAngles.x!=0||transform.eulerAngles.z!=0){
                    float y=transform.eulerAngles.y;
                    transform.eulerAngles=new Vector3(0,y,0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("ifHalt", false);
            if(!Input.GetKey(KeyCode.A)||!Input.GetKey(KeyCode.D))
                this.GetComponent<Rigidbody>().freezeRotation=true;//constraints
                //     =RigidbodyConstraints.FreezeRotationZ|RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotation;
            hs1.spring = SpringMin;
            hs2.spring = SpringMin;
        }
        // 

        if (Input.GetKey(KeyCode.W) == false /*&& Correction == false*/)
        {
            anim.SetBool("ifHalt", true);
            this.GetComponent<Rigidbody>().constraints
                =RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationZ;

            hs1.spring = SpringMax;
            hs2.spring = SpringMax;
            if (Input.GetKey(KeyCode.A)){
                // transform.Rotate(new Vector3(0,-120*Time.deltaTime,0),Space.Self);
                this.GetComponent<Rigidbody>().AddForce(transform.right * -1 * 200.0f);
            }
            if (Input.GetKey(KeyCode.D)){
                this.GetComponent<Rigidbody>().AddForce(transform.right * 200.0f);
                //transform.Rotate(new Vector3(0,120*Time.deltaTime,0),Space.Self);
                // if(transform.eulerAngles.x!=0||transform.eulerAngles.z!=0){
                //     float y=transform.eulerAngles.y;
                //     transform.eulerAngles=new Vector3(0,y,0);
                // }
            }
            if (Input.GetKey(KeyCode.S))
            {
                this.GetComponent<Rigidbody>().AddForce(transform.forward * -1 * 200.0f);
            }
        }
        // if (Input.GetKey(KeyCode.S))
        // {

        // }

        // if (Input.GetKey(KeyCode.A))
        // {

        // }
        // if (Input.GetKey(KeyCode.D))
        // {

        // }
        hj1.spring = hs1;
        hj2.spring = hs2;
    }

    protected void shoulderCtrl()
    {
        //Debug.Log("Shoulder~");
        Vector3 temp = fatherArm1.transform.localEulerAngles;
        //Debug.Log(temp);
        //Vector3 tempAnother = fatherArm1.transform.eulerAngles;

        //Debug.Log(temp.z);
        //fatherArm1.transform.Rotate(new Vector3(0.0f, 0.0f, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime));
        //fatherArm2.transform.Rotate(new Vector3(0.0f, 0.0f, Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime));
        float tempZ = temp.z;
        if(tempZ<=360 && tempZ>270)
        {
            tempZ = tempZ - 360;
        }
        //Debug.Log(tempZ);
        if (tempZ >45 && Input.GetAxis("Mouse Y") < 0)
        {
            fatherArm1.transform.Rotate(new Vector3(0.0f, 0.0f, Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime));
            fatherArm2.transform.Rotate(new Vector3(0.0f, 0.0f, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime));
        }
        else if (tempZ < -45 && Input.GetAxis("Mouse Y") > 0)
        {
            fatherArm1.transform.Rotate(new Vector3(0.0f, 0.0f, Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime));
            fatherArm2.transform.Rotate(new Vector3(0.0f, 0.0f, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime));
        }
        else if (tempZ >= -45 && tempZ <= 45)
        {
            fatherArm1.transform.Rotate(new Vector3(0.0f, 0.0f, Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime));
            fatherArm2.transform.Rotate(new Vector3(0.0f, 0.0f, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime));
        }



    }

    protected void Start()
    {
        _Velocity = GetComponent<Rigidbody>().velocity.magnitude;
        rb = this.gameObject.GetComponent<Rigidbody>();
        centralCapcollide = GetComponent<CapsuleCollider>();

        hs1 = hj1.spring;
        hs2 = hj2.spring;

        armRange = 0.0f;
    }


    void FixedUpdate()
    {
        // not driving and not dying
        if (!Morto)
        {
            if (BoatControlTrigger.ifEnterTrigger == false)
            {
                // charge jump
                ChargeJump();
                MoveCtrl();
                

                // Ctrl
                // MoveCtrl();
                //handCtrl();
                JumpCtrl();
                
            }
            // rotate
            transform.Rotate(0, Input.GetAxis("Mouse X") * 5.0f * sensitivity, 0);
        }

        if (BoatControlTrigger.ifEnterTrigger == true)
            Debug.Log("disable character move");

    }

    void Update()
    {
        shoulderCtrl();
    }

    
}
