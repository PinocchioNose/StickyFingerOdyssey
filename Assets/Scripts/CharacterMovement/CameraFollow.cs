using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : BaseAction
{
    private Transform TourCamera;
    public float CameraSmoothTime = 0f;
    private Vector3 relative_pos = new Vector3(0, 2.5f, -3.5f);
    private Vector3 velocity = Vector3.zero;

    protected new void Start()
    {
        base.Start();
        TourCamera = gameObject.transform;
    }

    protected new void LateUpdate()
    {
 
        TourCamera.RotateAround(TourCamera.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        TourCamera.RotateAround(TourCamera.position, TourCamera.right, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);

        //Vector3 after_rotate = V3RotateAround(relative_pos, TourCamera.right, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);
        //Debug.Log("after_rotate" + after_rotate);
        //Debug.Log("angle: " + -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);
        Vector3 TargetCameraPosition = CharacterTransform.TransformPoint(relative_pos);//获取相机跟随的相对位置，再转为世界坐标

        TourCamera.position = Vector3.SmoothDamp(
            TourCamera.position,
            TargetCameraPosition,
            ref velocity,
            CameraSmoothTime, //最好为0
            Mathf.Infinity,
            Time.deltaTime
        );

    }
}
