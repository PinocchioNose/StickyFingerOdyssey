using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : BaseAction
{
    //public Transform CharacterTransform;

    //#region 人物移动参数
    //public float moveSpeed = 1.0f;
    //public float rotateSpeed = 90.0f;
    ////public float shiftRate = 2.0f;// 按住Shift加速
    ////public float minDistance = 0.5f;// 相机离不可穿过的表面的最小距离（小于等于0时可穿透任何表面）
    //#endregion

    //#region 运动速度和其每个方向的速度分量
    //protected Vector3 direction = Vector3.zero;
    //protected Vector3 speedForward;
    //protected Vector3 speedBack;
    //protected Vector3 speedLeft;
    //protected Vector3 speedRight;
    //protected Vector3 speedUp;
    //protected Vector3 speedDown;
    //#endregion

    //跳跃中使用的参数
    protected bool isJumping;
    protected int jumpStrength;
    protected int maxStrength;
    protected float originJumpSpeed;

    //手臂延展
    protected float armRange;
    protected float maxArmRange;
    GameObject leftForeArm;
    GameObject rightForeArm;

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
        if(Input.GetMouseButtonDown(0))
        {
            if(armRange <= maxArmRange) armRange += 0.04f;
            leftForeArm.transform.Translate(CharacterTransform.transform.forward * -0.4f);
            rightForeArm.transform.Translate(CharacterTransform.transform.forward * -0.4f);

        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (armRange > 0) armRange -= 0.04f;
            leftForeArm.transform.Translate(CharacterTransform.transform.forward * 0.4f);
            rightForeArm.transform.Translate(CharacterTransform.transform.forward * 0.4f);
        }

    }

    protected new void Start()
    {
        isJumping = false;
        jumpStrength = 0;
        // 跳跃力量
        maxStrength = 120;
        originJumpSpeed = 0.0f;
        //手臂延展（恶搞用）
        armRange = 0f;
        maxArmRange = 5.0f;
        leftForeArm = GameObject.Find("LeftForeArm");
        rightForeArm = GameObject.Find("RightForeArm");

        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        
        CharacterTransform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        jumpCtrl();
        handCtrl();

        
    }


}
