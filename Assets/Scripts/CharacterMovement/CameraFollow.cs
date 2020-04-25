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
    private Vector3 look_position;

    [Tooltip("相机视角最小值")]
    public float CameraUpAndDown_min = -0.1f;
    [Tooltip("相机视角最大值")]
    public float CameraUpAndDown_max = 0.25f;

    protected new void Start()
    {
        base.Start();
        TourCamera = gameObject.transform;
        look_position = character_transform.position;
    }

    protected new void LateUpdate()
    {
        // update look_position to original
        look_position = character_transform.position;

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

        look_position += new Vector3(0, -YAxis_Input, 0) * Time.deltaTime;
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

        TourCamera.LookAt(look_position);
    }
}
