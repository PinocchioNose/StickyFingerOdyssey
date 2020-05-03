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

    [Tooltip("相机视角最小值")]
    public float CameraUpAndDown_min = -0.1f;
    [Tooltip("相机视角最大值")]
    public float CameraUpAndDown_max = 0.25f;

    [Tooltip("摄像机与观察点之间的距离")]
    public float relative_radius = 0.35f;
    [Tooltip("Y方向上的俯仰角度")]
    public float camera_FOV = 45f;
    private float origin_angle = 0f; // 最开始的摄像机角度  平视

    void old_camera_follow()
    {
        TourCamera.RotateAround(TourCamera.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        //TourCamera.RotateAround(TourCamera.position, TourCamera.right, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);

        var YAxis_Input = Input.GetAxis("Mouse Y");
        //Debug.Log("YAxis_Input = " + YAxis_Input);
        //抬升摄像机
        relative_pos -= new Vector3(0.0f, YAxis_Input, 0.0f) * Time.deltaTime;
        if (relative_pos.y > CameraUpAndDown_max)
            relative_pos = new Vector3(0, CameraUpAndDown_max, -0.25f);
        else if (relative_pos.y < CameraUpAndDown_min)
            relative_pos = new Vector3(0, CameraUpAndDown_min, -0.25f);

        
        //Vector3 after_rotate = V3RotateAround(relative_pos, TourCamera.right, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);
        Vector3 TargetCameraPosition = character_transform.TransformPoint(relative_pos);//获取相机跟随的相对位置，再转为世界坐标
        

        TourCamera.position = Vector3.SmoothDamp(
            TourCamera.position,
            TargetCameraPosition,
            ref velocity,
            CameraSmoothTime, //最好为0
            Mathf.Infinity,
            Time.deltaTime
        );

        TourCamera.LookAt(character_transform, Vector3.up);
    }

    void new_camera_follow()
    {
        TourCamera.RotateAround(TourCamera.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        
        origin_angle -= Input.GetAxis("Mouse Y") * Time.deltaTime * 2f;
        if (origin_angle > camera_FOV * Mathf.Deg2Rad)
            origin_angle = camera_FOV * Mathf.Deg2Rad;
        if (origin_angle < -camera_FOV * Mathf.Deg2Rad)
            origin_angle = -camera_FOV * Mathf.Deg2Rad;

        Vector3 relative_pos = new Vector3(0, relative_radius * Mathf.Sin(origin_angle), -relative_radius * Mathf.Cos(origin_angle));
        Vector3 TargetCameraPosition = character_transform.TransformPoint(relative_pos);

        TourCamera.position = Vector3.SmoothDamp(
            TourCamera.position,
            TargetCameraPosition,
            ref velocity,
            CameraSmoothTime, //最好为0
            Mathf.Infinity,
            Time.deltaTime
        );

        TourCamera.LookAt(character_transform, Vector3.up);
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
        new_camera_follow();

    }
}
