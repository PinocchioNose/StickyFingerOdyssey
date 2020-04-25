using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : BaseAction
{
    //public Transform CharacterTransform;

  

    //跳跃中使用的参数
    protected bool isJumping;
    protected int jumpStrength;
    protected int maxStrength;
    protected float originJumpSpeed;

    //手臂延展
    protected float armRange;
    GameObject leftForeArm;
    GameObject rightForeArm;

    //手臂旋转
    GameObject leftShoul;
    GameObject rightShoul;

    //摄像机
    GameObject myCamera;

    private float lastTime;

    protected void shoulderCtrl()
    {
        leftShoul.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime, 0.0f, 0.0f));
        rightShoul.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime, 0.0f, 0.0f));
    }
    protected void jumpCtrl()
    {
        if (isJumping)
        {
            //Debug.Log(speedUp);
            speedUp -= grav * Time.deltaTime;
            //Debug.Log("im jumping");
            if (CharacterTransform.transform.position.y < initPosY)
            {
                isJumping = false;
                speedUp = Vector3.zero;
                CharacterTransform.Translate(new Vector3(CharacterTransform.position.x, initPosY, CharacterTransform.position.z)
                    - CharacterTransform.position, Space.World);
            }

        }
    }

    protected void handCtrl()
    {
        //手臂延长
        //用肩膀到前臂的伸长线去做
        if (Input.GetMouseButton(0))
        {
            if (armRange <= maxArmRange)
            {
                leftForeArm.transform.Translate(myCamera.transform.forward
                    * elongSpeed * Time.deltaTime, Space.World);
                rightForeArm.transform.Translate(myCamera.transform.forward
                    * elongSpeed * Time.deltaTime, Space.World);

                armRange += elongSpeed * Time.deltaTime;
                //Debug.Log(armRange);
            }

        }
        else
        {
            if (armRange > 0)
            {

                leftForeArm.transform.Translate(myCamera.transform.forward
                    * -elongSpeed * Time.deltaTime, Space.World);
                rightForeArm.transform.Translate(myCamera.transform.forward
                    * -elongSpeed * Time.deltaTime, Space.World);
                armRange -= elongSpeed * Time.deltaTime;
                //Debug.Log(armRange);
            }
        }



    }

    protected new void Start()
    {
        isJumping = false;
        jumpStrength = 0;
        // 跳跃力量
        maxStrength = 120;
        originJumpSpeed = 0.0f;
        //手臂延展
        armRange = 0.0f;
        //init
        leftForeArm = GameObject.Find("LeftForeArm");
        rightForeArm = GameObject.Find("RightForeArm");
        leftShoul = GameObject.Find("LeftShoulder");
        rightShoul = GameObject.Find("RightShoulder");
        myCamera = GameObject.Find("Tour Camera");

        //Debug.Log(leftForeArm.transform.forward);
        //Debug.Log(rightForeArm.transform.forward);


        lastTime = Time.time;

        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        
        CharacterTransform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        jumpCtrl();
        handCtrl();
        shoulderCtrl();
        

    }


}
