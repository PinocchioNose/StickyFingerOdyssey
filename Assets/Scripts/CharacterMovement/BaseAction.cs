using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAction : MonoBehaviour
{
    public Transform CharacterTransform;

    #region 人物移动参数
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 90.0f;
    #endregion

    #region 运动速度和其每个方向的速度分量
    protected Vector3 direction = Vector3.zero;
    protected Vector3 speedForward;
    protected Vector3 speedBack;
    protected Vector3 speedLeft;
    protected Vector3 speedRight;
    protected Vector3 speedUp;
    protected Vector3 speedDown;
    #endregion

    // 模拟重力加速度
    protected Vector3 grav = new Vector3(0, 9.8f, 0);
    // 起始高度
    protected float initPosY;

    // Start is called before the first frame update
    protected void Start()
    {
        CharacterTransform = GameObject.Find("BodyGuard_Test_Prefab").GetComponent<Transform>();
        initPosY = CharacterTransform.transform.position.y;
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    protected void LateUpdate()
    {
        
    }

    /// <summary>
    /// 获取vector绕某一个轴旋转angle之后的向量
    /// </summary>
    /// <param name="source">source vector</param>
    /// <param name="axis">rotate axis</param>
    /// <param name="angle">rotate angle</param>
    /// <returns></returns>
    protected Vector3 V3RotateAround(Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis); // rotate coefficient
        return q * source;
    }
}
