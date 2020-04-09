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

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        CharacterTransform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

    }


}
