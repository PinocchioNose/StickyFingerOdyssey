using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cable_rot : MonoBehaviour
{
    public GameObject center;
    public GameObject[] objs;
    public float rot_spd;

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i < objs.Length;i++)
        {
            objs[i].transform.RotateAround(center.transform.position, Vector3.up, rot_spd);
        }
    }
}
