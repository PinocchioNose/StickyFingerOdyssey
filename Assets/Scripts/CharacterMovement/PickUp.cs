﻿using System.Collections;
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

    private GameObject ParticleSystem;

    private void Awake()
    {
        ParticleSystem = GameObject.Find("ParticleTrailL");
    }

    void Start()
    {
        handCanElong = 0;
        nextHandStatus = 0;

        isRejecting = false;
        armRange = 0.0f;

        ParticleSystem.GetComponent<ParticleSystem>().Stop();
    }
    
    public void setReject(bool sta)
    {
        isRejecting = sta;
    }

    private void OnJointBreak(float breakForce)
    {
        Debug.Log("The fixed joint broke!");
        GameObject.Find("spbtm").GetComponent<funcHandle>().exchange(1);
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
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Left Hand Collied");
        if (handCanElong == 1) handCanElong = 0;
        //Debug.Log("collied");
        if (other.gameObject.tag == "pickable")
        {
            //if(collision.gameObject.name == "lock_up_real")
            //{
            //    collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
            //}
            if (this.GetComponent<FixedJoint>() != null)
            {
                Debug.Log("connected");
                this.GetComponent<FixedJoint>().connectedBody = other.gameObject.GetComponent<Rigidbody>();
            }
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //if (isRejecting) return;
    //    Debug.Log("Left Hand Collied");
    //    if(handCanElong == 1) handCanElong = 0;
    //    //Debug.Log("collied");
    //    if (collision.gameObject.tag == "pickable")
    //    {
    //        //if(collision.gameObject.name == "lock_up_real")
    //        //{
    //        //    collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
    //        //}
    //        if (this.GetComponent<FixedJoint>() != null)
    //        {
    //            Debug.Log("connected");
    //            this.GetComponent<FixedJoint>().connectedBody = collision.gameObject.GetComponent<Rigidbody>();
    //        }
    //    }
    //}

    bool isPlayingParticle = false;
    
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
            if (isPlayingParticle == false)
            {
                ParticleSystem.GetComponent<ParticleSystem>().Play();
                isPlayingParticle = true;
            }
                
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
        if (handCanElong == 0)
        {
            isPlayingParticle = false;
            ParticleSystem.GetComponent<ParticleSystem>().Stop();
        }
            
        //else
        //    ParticleSystem.GetComponent<ParticleSystem>().Play();
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
