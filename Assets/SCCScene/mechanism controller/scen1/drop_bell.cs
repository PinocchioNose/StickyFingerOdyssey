using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop_bell : MonoBehaviour
{
    public GameObject connect,l_string;
    public float drop_speed,l_speed, l_drop_speed;
    public GameObject[] board;
    public GameObject[] rotate_center;
    public Vector3[] rotate_axis;
    public float[] rotate_speed;
    public int state = 0;                               //0 init;1 moving;2 finish
    float init_pos;
    public float drop_height;

    void Start()
    {
        state = 0;
        init_pos = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {       
        if(state == 1)
        {
            movestring();
            if(init_pos - gameObject.transform.position.y >= drop_height)
            {
                state = 2;
            }
        }
    }

    void OnCollisionEnter(Collision co)
    {
        if(co.gameObject.tag == "character" && state == 0)
        {
            state = 1;
        }
    }

    void movestring()
    {
        connect.transform.position += Vector3.down * drop_speed * Time.deltaTime;
        l_string.transform.localScale += Vector3.up * l_speed * Time.deltaTime;
        l_string.transform.position += Vector3.down * l_drop_speed * Time.deltaTime;
        for(int i=0;i<board.Length;i++)
        {
            board[i].transform.RotateAround(rotate_center[i].transform.position, rotate_axis[i], rotate_speed[i]);
        }
    }
}
