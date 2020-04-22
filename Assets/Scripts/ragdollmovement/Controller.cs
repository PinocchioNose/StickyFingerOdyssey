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
    // public float MinRoot, MaxRoot;
    // public float slope; 
    // private float pretime; 



    void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude > Resist) 
        {
            centralCapcollide.enabled = false;
            rb.constraints = RigidbodyConstraints.None;
            for (int x = 0; x < DeadJoint.Length; x++)
            {
                DeadJoint[x].useSpring = false;
            }
            anim.SetBool("ifHalt", true);
            Morto = true;
        }
    }


    void Start()
    {
        _Velocity = GetComponent<Rigidbody>().velocity.magnitude;
        rb = GetComponent<Rigidbody>();
        centralCapcollide = GetComponent<CapsuleCollider>();

        hs1 = hj1.spring;
        hs2 = hj2.spring;
    }


    void Update()
    {
        if (!Morto)
        {

            if (Input.GetKey(KeyCode.W))
            {
                anim.SetBool("ifHalt", false);

                hs1.spring = SpringMin;
                hs2.spring = SpringMin;

                if (Input.GetKey(KeyCode.A))
                {
                    transform.Rotate(0, -120 * Time.deltaTime, 0); 
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Rotate(0, 120 * Time.deltaTime, 0);
                }
            }



            if (Input.GetKey(KeyCode.W) == false && Correction == false)
            {
              anim.SetBool("ifHalt", true);

                hs1.spring = SpringMax;
                hs2.spring = SpringMax;
            }
            if (Input.GetKey(KeyCode.S))
            {
                
            }

            if (Input.GetKey(KeyCode.A))
            {

            }
            if (Input.GetKey(KeyCode.D))
            {

            }
            hj1.spring = hs1;
            hj2.spring = hs2;
        }
    }
}
