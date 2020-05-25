using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : BaseAction
{
    private Transform TourCamera;
    public float CameraSmoothTime = 0f;
    private Vector3 relative_pos = new Vector3(0, 0.25f, -0.25f);
    private Vector3 velocity = Vector3.zero;
    public Transform character_transform;

    //[Tooltip("相机视角最小值")]
    //public float CameraUpAndDown_min = -0.1f;
    //[Tooltip("相机视角最大值")]
    //public float CameraUpAndDown_max = 0.25f;

    [Tooltip("摄像机椭圆轨迹半长轴长度")]
    public float semi_major_Axis = 0.3f;
    [Tooltip("摄像机下部椭圆轨迹半短轴长度")]
    public float semi_minor_Axis_under = 0.1f;
    [Tooltip("摄像机上部部椭圆轨迹半短轴长度")]
    public float semi_minor_Axis_above = 0.2f;
    [Tooltip("Y方向上的俯仰角度")]
    public float camera_FOV = 45f;
    private float origin_angle = 0f; // 最开始的摄像机角度  平视

    public static float DistanceBetweenCamAndSpbtm; // 相机到spbtm的距离

    void camera_follow()
    {
        TourCamera.RotateAround(TourCamera.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        
        origin_angle -= Input.GetAxis("Mouse Y") * Time.deltaTime * 2f;
        if (origin_angle > camera_FOV * Mathf.Deg2Rad)
            origin_angle = camera_FOV * Mathf.Deg2Rad;
        if (origin_angle < -camera_FOV * Mathf.Deg2Rad)
            origin_angle = -camera_FOV * Mathf.Deg2Rad;

        if (origin_angle >= 0)
            relative_pos = new Vector3(0, semi_minor_Axis_above * Mathf.Sin(origin_angle), -semi_major_Axis * Mathf.Cos(origin_angle));
        else
            relative_pos = new Vector3(0, semi_minor_Axis_under * Mathf.Sin(origin_angle), -semi_major_Axis * Mathf.Cos(origin_angle));
        //Vector3 relative_pos = new Vector3(0, semi_minor_Axis * Mathf.Sin(origin_angle), -semi_major_Axis * Mathf.Cos(origin_angle));
        Vector3 TargetCameraPosition = character_transform.TransformPoint(relative_pos);

        TourCamera.position = Vector3.SmoothDamp(
            TourCamera.position,
            TargetCameraPosition,
            ref velocity,
            CameraSmoothTime, //最好为0
            Mathf.Infinity,
            Time.deltaTime
        );

        // TourCamera.LookAt(character_transform, Vector3.up);
        TourCamera.LookAt(character_transform.position);
    }

    protected new void Start()
    {
        base.Start();
        TourCamera = gameObject.transform;
        origin_angle = 0f;
    }

    protected new void LateUpdate()
    {
        //old_camera_follow();
        camera_follow();
        DistanceBetweenCamAndSpbtm = (TourCamera.position - character_transform.position).sqrMagnitude;

    }
}
