using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridge_rotate : MonoBehaviour
{
    public GameObject[] rot_monos,bridge;
    public GameObject bridge_center,rot_center;
    public float start_angel;
    public Vector3 axis_b,axis_r;
    public float rot_speed, bridge_speed;
    public int add_frame;
    int left_frame = 0;
    int i = 0;

    void Start()
    {
        for (i = 0; i < bridge.Length; i++)
        {
            bridge[i].transform.RotateAround(bridge_center.transform.position, axis_b, start_angel);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (left_frame > 0)
        {
            for (i = 0; i < rot_monos.Length; i++)
            {
                rot_monos[i].transform.RotateAround(rot_center.transform.position, axis_r, rot_speed);
            }
            for (i = 0; i < bridge.Length; i++)
            {
                bridge[i].transform.RotateAround(bridge_center.transform.position, axis_b, bridge_speed);
            }
            left_frame--;
        }
    }

    void OnTriggerEnter(Collider co)
    {
        if(co.gameObject.tag == "ball")
        {
            left_frame += add_frame;
            co.gameObject.tag = "Untagged";
        }
    }
}
